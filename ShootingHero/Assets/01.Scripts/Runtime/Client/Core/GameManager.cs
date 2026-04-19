using System.Collections.Generic;
using ShootingHero.Networks;
using ShootingHero.Shared;
using UnityEngine;

namespace ShootingHero.Clients
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance = null;
        public static GameManager Instance => instance;

        private Session session = null;
        private Dictionary<string, Unit> players = null;

        private Dictionary<string, ItemBase> items = null;

        public Session Session => session;

        public void Initialize()
        {
            if(instance != null)
            {
                instance.Release();
                Destroy(instance);
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            players = new Dictionary<string, Unit>();
            items = new Dictionary<string, ItemBase>();

            InputManager.Initialize();
        }

        public void Release()
        {
            
        }

        public void SetSession(Session session)
        {
            this.session = session;
        }

        public void AddPlayer(string playerID, Unit player)
        {
            players[playerID] = player;
        }

        public void RemovePlayer(string playerID)
        {
            players.Remove(playerID);
        }

        public Unit GetPlayer(string playerID)
        {
            players.TryGetValue(playerID, out Unit unit);
            return unit;
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
    }
}
