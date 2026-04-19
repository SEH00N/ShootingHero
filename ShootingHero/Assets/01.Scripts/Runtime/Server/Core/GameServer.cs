using System;
using System.Collections.Generic;
using System.Net.Sockets;
using ShootingHero.Networks;
using ShootingHero.Shared;

namespace ShootingHero.Servers
{
    public class GameServer : ISessionFactory
    {
        private Dictionary<Session, string> playerIDMap = null;

        private Server server = null;

        public void Initialize(DataTableManager dataTableManager, ServerDataTableManager serverDataTableManager, GameManager gameManager)
        {
            GameInstance.PlayMode = EPlayMode.Server;
            GameInstance.DataTableManager = dataTableManager;
            ServerInstance.ServerDataTableManager = serverDataTableManager;
            ServerInstance.GameServer = this;

            playerIDMap = new Dictionary<Session, string>();

            UnityPacketDispatcher unityPacketDispatcher = gameManager.gameObject.AddComponent<UnityPacketDispatcher>();
            server = new ServerBuilder(this, unityPacketDispatcher)
                .AddSingleton<GameServer>(this)
                .AddSingleton<GameManager>(gameManager)
                .AddSingleton<DataTableManager>(dataTableManager)
                .AddSingleton<ServerDataTableManager>(serverDataTableManager)
                .Build(typeof(GameServer).Assembly, typeof(GameDefine).Assembly);
            
            unityPacketDispatcher.Initialize(server);
        }

        public void Listen(int port)
        {
            server.Listen(port);
        }

        public void Send(IPacket packet, Func<string, Session, bool> filter = null)
        {
            server.Rooms.Room(ServerDefine.ROOM_ID).Send(packet, filter);
        }

        public void AddPlayer(string playerID, Session session)
        {
            server.Rooms.Room(ServerDefine.ROOM_ID).Add(playerID, session);
            playerIDMap[session] = playerID;
        }

        public string GetPlayerID(Session session)
        {
            playerIDMap.TryGetValue(session, out string playerID);
            return playerID;
        }

        Session ISessionFactory.Create(NetworkObject networkObject, Socket connectedSocket)
        {
            return new Session();
        }
    }
}