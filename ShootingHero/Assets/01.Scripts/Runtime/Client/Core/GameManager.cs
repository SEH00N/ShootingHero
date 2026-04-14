using UnityEngine;

namespace ShootingHero.Clients
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            InputManager.Initialize();
            InputManager.EnableInput<PlayerInputReader>();
        }
    }
}
