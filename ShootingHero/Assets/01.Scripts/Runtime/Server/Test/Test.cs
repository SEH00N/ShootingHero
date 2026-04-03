using System.Collections.Generic;
using System.Net.Sockets;
using ShootingHero.Networks;
using ShootingHero.Shared;
using UnityEngine;

namespace ShootingHero.Servers
{
    public class Test : MonoBehaviour, ISessionFactory
    {
        private Dictionary<Session, string> sessionMap = null;
        public Dictionary<Session, string> SessionMap => sessionMap;

        private void Awake()
        {
            sessionMap = new Dictionary<Session, string>();

            Server server = new ServerBuilder(this)
                .AddSingleton<Test>(this)
                .Build(typeof(Test).Assembly, typeof(S2C_TestPacket).Assembly);

            server.Listen(9999);
        }
        
        Session ISessionFactory.Create(NetworkObject networkObject, Socket connectedSocket)
        {
            return new Session();
        }
    }
}
