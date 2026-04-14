using System.Collections.Generic;
using ShootingHero.Shared;
using UnityEngine;

namespace ShootingHero.Clients
{
    public class GameManager : MonoBehaviour
    {
        private Dictionary<string, Unit> players = null;

        public void Initialize()
        {
            DontDestroyOnLoad(gameObject);

            players = new Dictionary<string, Unit>();

            InputManager.Initialize();
            InputManager.EnableInput<PlayerInputReader>();
        }

        public void Release()
        {
            
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
