using UnityEngine;

namespace ShootingHero.Shared
{
    public class Unit : MonoBehaviour, IProjectileCollider
    {
        [SerializeField]
        private UnitMovementComponent unitMovementComponent = null;
        public UnitMovementComponent UnitMovementComponent => unitMovementComponent;

        [SerializeField]
        private UnitWeaponComponent unitWeaponComponent = null;
        public UnitWeaponComponent UnitWeaponComponent => unitWeaponComponent;

        [SerializeField]
        private UnitHealthComponent unitHealthComponent = null;
        public UnitHealthComponent UnitHealthComponent => unitHealthComponent;

        private string playerID = "";
        private int currentHeight = 0;

        public string PlayerID => playerID;

        public void Initialize(string playerID)
        {
            this.playerID = playerID;
            unitHealthComponent.Initialize(100);
        }

        public int GetHeight()
        {
            return currentHeight;
        }

        public void SetHeight(int height)
        {
            currentHeight = height;
        }

        public void Collide(Projectile projectile, Vector2 point)
        {
            if(GameInstance.PlayMode != EPlayMode.Server)
                return;

            unitHealthComponent.GetDamage(projectile.Damage);
        }
    }
}