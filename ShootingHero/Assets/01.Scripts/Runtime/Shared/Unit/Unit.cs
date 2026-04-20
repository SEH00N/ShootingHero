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
        private bool isDead = false;

        public string PlayerID => playerID;

        public void Initialize(string playerID, int heigth, int currentHP, int weaponID, string weaponStatus)
        {
            this.playerID = playerID;
            SetHeight(heigth);
            isDead = false;

            int maxHP = GameInstance.DataTableManager.gameConfigTable.GetUnitMaxHP();
            unitHealthComponent.Initialize(maxHP, currentHP);
            unitHealthComponent.OnDeadEvent += HandleDead;

            unitWeaponComponent.SetWeapon(weaponID, weaponStatus);
        }

        public void Respawn(int height)
        {
            gameObject.SetActive(true);
            SetHeight(height);
            isDead = false;

            unitHealthComponent.ResetToMaxHP();
            unitWeaponComponent.SetWeapon(-1, null);
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
            
            if(isDead == true)
                return;

            unitHealthComponent.GetDamage(projectile.Owner, projectile.Damage);
        }

        private void HandleDead(Unit attacker)
        {
            isDead = true;
        }
    }
}