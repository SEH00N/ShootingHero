using System;
using System.Collections.Generic;
using System.Net.Sockets;
using ShootingHero.Networks;
using ShootingHero.Shared;
using UnityEngine;

namespace ShootingHero.Servers
{
    public class GameServer : MonoBehaviour, ISessionFactory
    {
        private Dictionary<string, Unit> players = null;
        private Server server = null;
        public Server Server => server;

        public void Initialize(Unit unitPrefab, UnityPacketDispatcher unityPacketDispatcher)
        {
            players = new Dictionary<string, Unit>();
            server = new ServerBuilder(this, unityPacketDispatcher)
                .AddSingleton<GameServer>(this)
                .AddSingleton<Unit>(unitPrefab)
                .Build(typeof(GameServer).Assembly, typeof(GameDefine).Assembly);
        }

        public void Listen(int port)
        {
            server.Listen(port);
        }

        public void AddPlayer(string playerID, Unit player)
        {
            players[playerID] = player;
        }

        public void ForEachPlayer(Action<string, Unit> callback)
        {
            foreach(KeyValuePair<string, Unit> element in players)
                callback?.Invoke(element.Key, element.Value);
        }

        Session ISessionFactory.Create(NetworkObject networkObject, Socket connectedSocket)
        {
            return new Session();
        }
    }
}