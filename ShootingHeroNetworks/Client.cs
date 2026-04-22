using System;
using System.Net;
using System.Net.Sockets;

namespace ShootingHero.Networks
{
    public class Client : NetworkObject
    {
        private readonly Session session = null;
        private readonly PacketSerializer packetSerializer = null;
        private readonly IPacketDispatcher packetDispatcher = null;

        internal Client(INetworkObjectBuilder builder) : base(builder)
        {
            session = GetInstance<Session>();
            packetSerializer = GetInstance<PacketSerializer>();
            packetDispatcher = GetInstance<IPacketDispatcher>();
        }

        public void Connect(string host, int port)
        {
            Socket socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            socket.DualMode = true;

            EndPoint remoteEndPoint;
            if(IPAddress.TryParse(host, out IPAddress ipAddress) == true)
                remoteEndPoint = new IPEndPoint(ipAddress, port);
            else
                remoteEndPoint = new DnsEndPoint(host, port);

            SocketAsyncEventArgs connectArgs = new SocketAsyncEventArgs();
            connectArgs.RemoteEndPoint = remoteEndPoint;
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

            session.Open(connectArgs.ConnectSocket, packetSerializer, packetDispatcher);
        }
    }
}