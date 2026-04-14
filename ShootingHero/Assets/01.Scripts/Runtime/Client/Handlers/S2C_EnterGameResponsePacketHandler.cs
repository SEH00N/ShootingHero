using System.Collections.Generic;
using System.Threading.Tasks;
using ShootingHero.Networks;
using ShootingHero.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShootingHero.Clients
{
    [PacketHandler(typeof(S2C_EnterGameResponsePacket))]
    public class S2C_EnterGameResponsePacketHandler : IPacketHandler<S2C_EnterGameResponsePacket>
    {
        private readonly GameManager gameManager = null;
        private readonly Unit unitPrefab = null;

        public S2C_EnterGameResponsePacketHandler(GameManager gameManager, Unit unitPrefab)
        {
            this.gameManager = gameManager;
            this.unitPrefab = unitPrefab;
        }

        ValueTask IPacketHandler<S2C_EnterGameResponsePacket>.HandlePacket(Session session, S2C_EnterGameResponsePacket packet)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
            asyncOperation.completed += _ => {
                foreach(KeyValuePair<string, Vector2> element in packet.Players)
                {
                    Unit unit = Object.Instantiate(unitPrefab, element.Value, Quaternion.identity);
                    gameManager.AddPlayer(element.Key, unit);
                }

                Unit myPlayer = gameManager.GetPlayer(packet.PlayerID);
                myPlayer.gameObject.AddComponent<UnitInputComponent>();
            };

            return new ValueTask();
        }
    }
}