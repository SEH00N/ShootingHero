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

        [SerializeField]
        private UnitHealthComponent unitHealthComponent = null;
        public UnitHealthComponent UnitHealthComponent => unitHealthComponent;

        [SerializeField]
        private UnitProjectileCollider unitProjectileCollider = null;
        public UnitProjectileCollider UnitProjectileCollider => unitProjectileCollider;

        private int characterID = 0;
        private string playerID = "";
        private int currentHeight = 0;
        private bool isDead = false;

        public int CharacterID => characterID;
        public string PlayerID => playerID;
        public bool IsDead => isDead;

        public void Initialize(int characterID, string playerID, int heigth, int currentHP, int weaponID, string weaponStatus)
        {
            this.characterID = characterID;
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

        private void HandleDead(Unit attacker)
        {
            isDead = true;
        }
    }
}