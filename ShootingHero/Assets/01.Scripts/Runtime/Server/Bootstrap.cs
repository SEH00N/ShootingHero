using UnityEngine;
using ShootingHero.Networks;
using System.Net.Sockets;

namespace ShootingHero.Servers
{
    public class Bootstrap : MonoBehaviour
    {
        private class SessionFactory : ISessionFactory
        {
            public Session Create(NetworkObject networkObject, Socket connectedSocket)
            {
                return new Session();
            }
        }

        private void Awake()
        {
            Server server = new ServerBuilder(new SessionFactory()).Build();
            server.Listen(10);
        }
    }
}