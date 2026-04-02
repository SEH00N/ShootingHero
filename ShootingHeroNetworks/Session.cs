using System;
using System.Collections.Generic;
using System.Net.Sockets;

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

        public event Action<Exception> OnPacketHandleErrorEvent = null;

        public Session()
        {
            sendLocker = new object();
        }

        public void Open(Socket connectedSocket, PacketSerializer packetSerializer, IPacketDispatcher packetDispatcher)
        {
            this.connectedSocket = connectedSocket;
            this.packetSerializer = packetSerializer;
            this.packetDispatcher = packetDispatcher;

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
                connectedSocket.Close();
            }
            catch { }
            finally
            {
                receiveArgs.Dispose();
                sendArgs.Dispose();
                
                lock (sendLocker)
                {
                    sendQueue.Dispose();
                }
            }
        }

        public void SendAsync(IPacket packet)
        {
            SendAsync(new PacketSendQueueContext(packetSerializer, packet));
        }

        internal void SendAsync(ISendQueueContext sendQueueContext)
        {
            if(sendQueue == null)
            {
                sendQueueContext.Dispose();
                throw new Exception("Session is not open");
            }

            if(connectedSocket.Connected == false)
            {
                sendQueueContext.Dispose();
                Close();
                return;
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
            if(connectedSocket.Connected == false)
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
                    packetDispatcher.Dispatch(packet);
            }
            catch(Exception err)
            {
                OnPacketHandleErrorEvent?.Invoke(err);
            }

            return packetSize;
        }
    }
}
