using System.Net;
using System.Net.Sockets;

namespace ShootingHero.Networks
{
    public class Server : NetworkObject
    {
        private Socket listenSocket = null;
        private SocketAsyncEventArgs acceptArgs = null;

        private readonly ISessionFactory sessionFactory = null;
        private readonly PacketSerializer packetSerializer = null;
        private readonly IPacketDispatcher packetDispatcher = null;
        private readonly IRoomManager roomManager = null;

        public IRoomManager Rooms => roomManager;

        internal Server(INetworkObjectBuilder builder) : base(builder)
        {
            sessionFactory = GetInstance<ISessionFactory>();
            packetSerializer = GetInstance<PacketSerializer>();
            packetDispatcher = GetInstance<IPacketDispatcher>();
            roomManager = GetInstance<IRoomManager>();
        }

        public void Listen(int port, int backlog = 10)
        {
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
                listenSocket.Close();
            }
            catch { }
            finally
            {
                acceptArgs.Dispose();
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
