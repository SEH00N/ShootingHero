using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ShootingHero.Networks
{
    public class Server : NetworkObject
    {
        private readonly ISessionFactory sessionFactory = null;
        private readonly PacketSerializer packetSerializer = null;
        private readonly IPacketDispatcher packetDispatcher = null;
        private readonly IRoomManager roomManager = null;

        private Socket listenSocket = null;
        private SocketAsyncEventArgs acceptArgs = null;
        private int isClosed = 0;

        public IRoomManager Rooms => roomManager;
        public bool IsOpened => Volatile.Read(ref isClosed) == 0;

        internal Server(INetworkObjectBuilder builder) : base(builder)
        {
            sessionFactory = GetInstance<ISessionFactory>();
            packetSerializer = GetInstance<PacketSerializer>();
            packetDispatcher = GetInstance<IPacketDispatcher>();
            roomManager = GetInstance<IRoomManager>();
        }

        public void Listen(int port, int backlog = 10)
        {
            Volatile.Write(ref isClosed, 0);

            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            listenSocket.Listen(backlog);

            acceptArgs = new SocketAsyncEventArgs();
            acceptArgs.Completed += HandleAccepted;

            AcceptAsync();
        }

        public void Close()
        {
            try
            {
                if(Volatile.Read(ref isClosed) == 1)
                    return;

                Volatile.Write(ref isClosed, 1);
                listenSocket?.Close();
            }
            catch { }
            finally
            {
                acceptArgs?.Dispose();
                (this as IAsyncDisposable).DisposeAsync();

                acceptArgs = null;
                listenSocket = null;
            }
        }

        private void AcceptAsync()
        {
            acceptArgs.AcceptSocket = null;

            bool isPending = listenSocket.AcceptAsync(acceptArgs);
            if (isPending == false)
                HandleAccepted(null, acceptArgs);
        }

        private void HandleAccepted(object sender, SocketAsyncEventArgs acceptArgs)
        {
            if (acceptArgs.SocketError != SocketError.Success || acceptArgs.AcceptSocket == null)
            {
                AcceptAsync();
                return;
            }

            Session session = sessionFactory.Create(this, acceptArgs.AcceptSocket);
            session.Open(acceptArgs.AcceptSocket, packetSerializer, packetDispatcher);

            AcceptAsync();
        }
    }
}
