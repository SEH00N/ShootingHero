using ShootingHero.Shared;
using UnityEngine;

namespace ShootingHero.Servers
{
    public class HealPackItemComponentServer : DedicatedMonoBehaviour
    {
        [SerializeField]
        private HealPackItem healPackItem = null;

        protected override EPlayMode DedicatedType => EPlayMode.Server;

        protected override void OnAwake()
        {
            base.OnAwake();
            healPackItem.OnHealPackUsedEvent += HandleHealPackItemUsed;
        }

        private void HandleHealPackItemUsed(Unit unit, int healAmount)
        {
            unit.UnitHealthComponent.GetDamage(unit, -healAmount);
        }
    }
}