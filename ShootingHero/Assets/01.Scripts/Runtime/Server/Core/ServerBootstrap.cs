using System;
using ShootingHero.Servers;
using ShootingHero.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace ShootingHero.Clients
{
    public class ServerBootstrap : MonoBehaviour
    {
        [SerializeField]
        private GameManager gameManager = null;

        [SerializeField]
        private DataTableManager dataTableManager = null;

        [SerializeField]
        private ServerDataTableManager serverDataTableManager = null;

        public async void StartServer()
        {
            gameManager.Initialize();
            
            GameServer gameServer = new GameServer();
            gameServer.Initialize(dataTableManager, serverDataTableManager, gameManager);
            gameServer.Listen(9999);

            await SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);

            ItemTableRow testItemTableRow = dataTableManager.itemTable.GetRow(1);
            for (int i = 0; i < 3; ++i)
            {
                string uuid = Guid.NewGuid().ToString();
                Vector2 position = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));

                ItemBase item = Instantiate(testItemTableRow.itemPrefab, position, Quaternion.identity);
                item.Initialize(testItemTableRow.id, uuid, () => {
                    Destroy(item.gameObject);
                    gameManager.RemoveItem(uuid);
                });

                gameManager.AddItem(uuid, item);
            }
        }
    }
}
