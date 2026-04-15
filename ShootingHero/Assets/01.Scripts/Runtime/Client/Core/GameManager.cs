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

        public Session Session => session;

        public void Initialize()
        {
            if(instance != null)
            {
                instance.Release();
                DestroyImmediate(instance);
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            players = new Dictionary<string, Unit>();

            InputManager.Initialize();
            InputManager.EnableInput<PlayerInputReader>();
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

        public Unit GetPlayer(string playerID)
        {
            players.TryGetValue(playerID, out Unit unit);
            return unit;
        }
    }
}
