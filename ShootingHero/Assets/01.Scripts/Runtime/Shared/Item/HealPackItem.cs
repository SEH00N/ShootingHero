using System;
using UnityEngine;

namespace ShootingHero.Shared
{
    public class HealPackItem : ItemBase
    {
        [SerializeField]
        private int healAmount = 0;

        public event Action<Unit, int> OnHealPackUsedEvent = null;

        protected override void OnInteract(Unit unit)
        {
            OnHealPackUsedEvent?.Invoke(unit, healAmount);
            DestroyItem();
        }
    }
}