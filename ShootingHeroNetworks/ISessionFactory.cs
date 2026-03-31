using System.Net.Sockets;

namespace ShootingHero.Networks
{
    public interface ISessionFactory
    {
        Session Create(NetworkObject networkObject, Socket connectedSocket);
    }
}