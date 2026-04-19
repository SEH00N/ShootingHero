using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShootingHero.Shared
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance = null;
        public static GameManager Instance => instance;

        private Dictionary<string, Unit> players = null;
        private Dictionary<string, ItemBase> items = null;

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
        }

        public void Release()
        {
            
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
    }
}