using System.Net;
using System.Net.Sockets;

namespace ShootingHero.Networks
{
    public class Client : NetworkObject
    {
        private readonly Session session = null;
        private readonly PacketFactory packetFactory = null;
        private readonly PacketHandlerFactory packetHandlerFactory = null;

        internal Client(INetworkObjectBuilder builder) : base(builder)
        {
            session = GetSingleton<Session>();
            packetFactory = GetSingleton<PacketFactory>();
            packetHandlerFactory = GetSingleton<PacketHandlerFactory>();
        }

        public void Connect(string address, int port)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress ipAddress = IPAddress.Parse(address);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);

            SocketAsyncEventArgs connectArgs = new SocketAsyncEventArgs();
            connectArgs.RemoteEndPoint = ipEndPoint;
            connectArgs.Completed += HandleConnected;

            bool isPending = socket.ConnectAsync(connectArgs);
            if (isPending == false)
                HandleConnected(null, connectArgs);
        }

        public void Disconnect()
        {
            session.Close();
        }

        private void HandleConnected(object sender, SocketAsyncEventArgs connectArgs)
        {
            if (connectArgs.SocketError != SocketError.Success)
                return;

            session.Open(connectArgs.ConnectSocket, packetFactory, packetHandlerFactory);
        }
    }
}