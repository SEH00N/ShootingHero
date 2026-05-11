using ShootingHero.Shared;
using UnityEngine;

namespace ShootingHero.Clients
{
    public class ClientBootstrap : MonoBehaviour
    {
        [SerializeField]
        private GameManager gameManager = null;

        [SerializeField]
        private DataTableManager dataTableManager = null;

        public void StartClient()
        {
            InputManager.Initialize();
            gameManager.Initialize();

            GameClient gameClient = new GameClient();
            gameClient.Intialize(dataTableManager, gameManager, Random.Range(0, 4));
            gameClient.Connect("localhost", 9999);
        }
    }
}
