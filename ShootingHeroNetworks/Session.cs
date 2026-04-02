using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace ShootingHero.Networks
{
    public class Session
    {
        private readonly object sendLocker = null;

        private SendQueue sendQueue = null;
        private SocketAsyncEventArgs sendArgs = null;

        private ReceiveBuffer receiveBuffer = null;
        private SocketAsyncEventArgs receiveArgs = null;

        private Socket connectedSocket = null;
        private PacketSerializer packetSerializer = null;
        private IPacketDispatcher packetDispatcher = null;

        private int isClosed = 1;
        public bool IsOpened => Volatile.Read(ref isClosed) == 0 && connectedSocket != null && connectedSocket.Connected == true;

        public event Action<Exception> OnErrorEvent = null;
        public event Action<Session> OnClosedEvent = null;

        public Session()
        {
            sendLocker = new object();
        }

        public void Open(Socket connectedSocket, PacketSerializer packetSerializer, IPacketDispatcher packetDispatcher)
        {
            if (connectedSocket == null)
                throw new ArgumentNullException(nameof(connectedSocket));

            if (packetSerializer == null)
                throw new ArgumentNullException(nameof(packetSerializer));

            if (packetDispatcher == null)
                throw new ArgumentNullException(nameof(packetDispatcher));

            this.connectedSocket = connectedSocket;
            this.packetSerializer = packetSerializer;
            this.packetDispatcher = packetDispatcher;

            Volatile.Write(ref isClosed, 0);

            sendQueue = new SendQueue();
            sendArgs = new SocketAsyncEventArgs();
            sendArgs.Completed += HandleSent;

            receiveBuffer = new ReceiveBuffer(NetworkDefine.PACKET_MAX_SIZE);
            receiveArgs = new SocketAsyncEventArgs();
            receiveArgs.Completed += HandleReceived;

            ReceiveAsync();
        }

        public void Close()
        {
            try
            {
                if(Volatile.Read(ref isClosed) == 1)
                    return;

                Volatile.Write(ref isClosed, 1);
                connectedSocket?.Close();
            }
            catch { }
            finally
            {
                receiveArgs?.Dispose();
                sendArgs?.Dispose();

                receiveArgs = null;
                sendArgs = null;
                connectedSocket = null;

                lock (sendLocker)
                {
                    sendQueue?.Dispose();
                    sendQueue = null;
                }

                OnClosedEvent?.Invoke(this);
            }
        }

        public void SendAsync(IPacket packet)
        {
            if (packet == null)
                throw new ArgumentNullException(nameof(packet));

            SendAsync(new PacketSendQueueContext(packetSerializer, packet));
        }

        internal void SendAsync(ISendQueueContext sendQueueContext)
        {
            if (sendQueueContext == null)
                throw new ArgumentNullException(nameof(sendQueueContext));

            if (IsOpened == false)
            {
                sendQueueContext.Dispose();
                Close();
                throw new InvalidOperationException("Session is not opened");
            }

            List<ArraySegment<byte>> bufferList = null;
            lock (sendLocker)
            {
                sendQueue.Enqueue(sendQueueContext);
                if (sendQueue.TryFlush(out bufferList) == false)
                    return;
            }

            sendArgs.BufferList = bufferList;
            bool isPending = connectedSocket.SendAsync(sendArgs);
            if (isPending == false)
                HandleSent(null, sendArgs);
        }

        private void HandleSent(object sender, SocketAsyncEventArgs sendArgs)
        {
            if (IsOpened == false)
            {
                Close();
                return;
            }

            if (sendArgs.SocketError != SocketError.Success || sendArgs.BytesTransferred <= 0)
            {
                Close();
                return;
            }

            List<ArraySegment<byte>> bufferList = null;
            lock (sendLocker)
            {
                sendQueue.Clear();
                if (sendQueue.TryFlush(out bufferList) == false)
                    return;
            }

            sendArgs.BufferList = bufferList;
            bool isPending = connectedSocket.SendAsync(sendArgs);
            if (isPending == false)
                HandleSent(null, sendArgs);
        }

        private void ReceiveAsync()
        {
            if (IsOpened == false)
            {
                Close();
                return;
            }

            receiveBuffer.CleanUp();
            receiveArgs.SetBuffer(receiveBuffer.FreeBuffer);

            bool isPending = connectedSocket.ReceiveAsync(receiveArgs);
            if (isPending == false)
                HandleReceived(null, receiveArgs);
        }

        private void HandleReceived(object sender, SocketAsyncEventArgs receiveArgs)
        {
            if (IsOpened == false)
            {
                Close();
                return;
            }

            if (receiveArgs.SocketError != SocketError.Success || receiveArgs.BytesTransferred <= 0)
            {
                Close();
                return;
            }

            receiveBuffer.MoveWriteIndex(receiveArgs.BytesTransferred);
            while(true)
            {
                int processedSize = HandlePacket(receiveBuffer.UsedBuffer);
                if(processedSize <= 0)
                    break;

                receiveBuffer.MoveReadIndex(processedSize);
            }

            ReceiveAsync();
        }

        private int HandlePacket(ArraySegment<byte> buffer)
        {
            if(buffer.Count < NetworkDefine.PACKET_SIZE_HEADER)
                return 0;

            ushort packetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            if(packetSize > NetworkDefine.PACKET_MAX_SIZE || packetSize > buffer.Count)
                return 0;
            
            ArraySegment<byte> packetData = new ArraySegment<byte>(buffer.Array, buffer.Offset + NetworkDefine.PACKET_SIZE_HEADER, packetSize - NetworkDefine.PACKET_SIZE_HEADER);

            try
            {
                IPacket packet = packetSerializer.Deserialize(packetData);
                if(packet != null)
                    packetDispatcher.Dispatch(this, packet);
            }
            catch(Exception err)
            {
                OnErrorEvent?.Invoke(err);
            }

            return packetSize;
        }
    }
}
