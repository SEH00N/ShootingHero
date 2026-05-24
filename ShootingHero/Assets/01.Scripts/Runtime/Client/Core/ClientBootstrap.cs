using ShootingHero.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace ShootingHero.Clients
{
    public class ClientBootstrap : MonoBehaviour
    {
        [SerializeField]
        private GameManager gameManager = null;

        [SerializeField]
        private DataTableManager dataTableManager = null;

        private void Start()
        {
            GameInstance.PlayMode = EPlayMode.Client;
            GameInstance.DataTableManager = dataTableManager;

            InputManager.Initialize();
            gameManager.Initialize();

            SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        }
    }
}
