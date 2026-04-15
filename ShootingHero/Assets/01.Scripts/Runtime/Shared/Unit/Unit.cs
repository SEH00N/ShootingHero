using UnityEngine;

namespace ShootingHero.Shared
{
    public class Unit : MonoBehaviour
    {
        [SerializeField]
        private UnitMovementComponent unitMovementComponent = null;
        public UnitMovementComponent UnitMovementComponent => unitMovementComponent;
    }
}