using System;
using UnityEngine;

namespace ShootingHero.Shared
{
    public class UnitHealthComponent : MonoBehaviour
    {
        private int maxHP = 0;
        private int currentHP = 0;

        public event Action<int> OnDamagedEvent = null;
        public event Action OnDeadEvent = null;

        public void Initialize(int maxHP)
        {
            this.maxHP = maxHP;
            currentHP = this.maxHP;
        }

        public void GetDamage(int damage)
        {
            currentHP = Mathf.Clamp(currentHP - damage, 0, maxHP);
            OnDamagedEvent?.Invoke(damage);

            Debug.LogError($"Damaged!! currentHP: {currentHP}");

            if(currentHP <= 0)
                OnDeadEvent?.Invoke();
        }
    }
}