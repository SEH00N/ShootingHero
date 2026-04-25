using ShootingHero.Shared;
using UnityEngine;

namespace ShootingHero.Servers
{
    public struct CreateUnitData
    {
        public UnitDataDTO unitData;

        public CreateUnitData(Unit unit)
        {
            int characterID = unit.CharacterID;
            Vector2 position = unit.transform.position;
            int height = unit.GetHeight();
            int currentHP = unit.UnitHealthComponent.CurrentHP;
            WeaponBase weapon = unit.UnitWeaponComponent.Weapon;
            int weaponID = weapon == null ? -1 : weapon.WeaponID;
            string weaponStatus = weapon == null ? null : weapon.GetStatus();

            unitData = new UnitDataDTO() {
                CharacterID = characterID,
                Position = position,
                Height = height,
                CurrentHP = currentHP,
                CurrentWeaponID = weaponID,
                CurrentWeaponStatus = weaponStatus
            };
        }
    }
}