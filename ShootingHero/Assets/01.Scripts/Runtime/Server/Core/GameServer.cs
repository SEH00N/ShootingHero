using System;
using System.Collections.Generic;
using System.Net.Sockets;
using ShootingHero.Networks;
using ShootingHero.Shared;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ShootingHero.Servers
{
    public class GameServer : MonoBehaviour, ISessionFactory
    {
        private static GameServer instance = null;
        public static GameServer Instance => instance;

        private Dictionary<string, Unit> players = null;
        private Dictionary<Session, string> playerIDMap = null;

        private Dictionary<string, ItemBase> items = null;

        private Server server = null;
        public Server Server => server;

        public void Initialize(DataTableManager dataTableManager, UnityPacketDispatcher unityPacketDispatcher)
        {
            instance = this;

            players = new Dictionary<string, Unit>();
            playerIDMap = new Dictionary<Session, string>();

            items = new Dictionary<string, ItemBase>();

            server = new ServerBuilder(this, unityPacketDispatcher)
                .AddSingleton<GameServer>(this)
                .AddSingleton<DataTableManager>(dataTableManager)
                .Build(typeof(GameServer).Assembly, typeof(GameDefine).Assembly);

            ItemTableRow testItemTableRow = dataTableManager.itemTable.GetRow(1);
            for(int i = 0; i < 3; ++i)
            {
                string uuid = Guid.NewGuid().ToString();
                Vector2 position = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
                
                ItemBase item = Instantiate(testItemTableRow.itemPrefab, transform);
                item.Initialize(testItemTableRow.id, uuid, () => {
                    Destroy(item.gameObject);
                    RemoveItem(uuid);
                });
                item.transform.SetPositionAndRotation(position, Quaternion.identity);

                AddItem(uuid, item);
            }
        }

        public void Listen(int port)
        {
            server.Listen(port);
        }

        public void AddPlayer(Session session, string playerID, Unit player)
        {
            players[playerID] = player;
            playerIDMap[session] = playerID;
        }

        public void RemovePlayer(Session session)
        {
            string playerID = GetPlayerID(session);

            playerIDMap.Remove(session);
            if(string.IsNullOrEmpty(playerID) == false)
                players.Remove(playerID);
        }

        public Unit GetPlayer(string playerID)
        {
            players.TryGetValue(playerID, out Unit unit);
            return unit;
        }

        public string GetPlayerID(Session session)
        {
            playerIDMap.TryGetValue(session, out string playerID);
            return playerID;
        }

        public void ForEachPlayer(Action<string, Unit> callback)
        {
            foreach(KeyValuePair<string, Unit> element in players)
                callback?.Invoke(element.Key, element.Value);
        }

        public void AddItem(string itemUUID, ItemBase item)
        {
            items[itemUUID] = item;
        }

        public void RemoveItem(string itemUUID)
        {
            items.Remove(itemUUID);
        }

        public ItemBase GetItem(string itemUUID)
        {
            items.TryGetValue(itemUUID, out ItemBase item);
            return item;
        }

        public void ForEachItem(Action<string, ItemBase> callback)
        {
            foreach(KeyValuePair<string, ItemBase> element in items)
                callback?.Invoke(element.Key, element.Value);
        }

        Session ISessionFactory.Create(NetworkObject networkObject, Socket connectedSocket)
        {
            return new Session();
        }
    }
}