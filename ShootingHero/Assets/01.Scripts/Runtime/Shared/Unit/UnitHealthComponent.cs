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

        public int CurrentHP => currentHP;

        public void Initialize(int maxHP, int currentHP)
        {
            this.maxHP = maxHP;
            this.currentHP = Mathf.Clamp(currentHP, 0, maxHP);;
        }

        public void ResetToMaxHP()
        {
            currentHP = maxHP;
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