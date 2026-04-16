using UnityEngine;

namespace ShootingHero.Shared
{
    public class Unit : MonoBehaviour
    {
        [SerializeField]
        private UnitMovementComponent unitMovementComponent = null;
        public UnitMovementComponent UnitMovementComponent => unitMovementComponent;

        [SerializeField]
        private UnitWeaponComponent unitWeaponComponent = null;
        public UnitWeaponComponent UnitWeaponComponent => unitWeaponComponent;
    }
}