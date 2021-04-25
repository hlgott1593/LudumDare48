using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LD48.Health {
    public class HealthSystem {
        public IDamagable Owner { get; private set; }
        public float MaxHp { get; private set; }
        public float CurrentHp { get; private set; }
        public bool IsDead => CurrentHp <= 0;
        public bool Invulnerable { get; private set; }
        public void SetInvulnerable() => Invulnerable = true;
        public void SetVulnerable() => Invulnerable = false;

        public HealthSystem(IDamagable owner, HealthConfig healthData) {
            Owner = owner;
            MaxHp = healthData.maxHp;
            CurrentHp = MaxHp;

            Invulnerable = healthData.invulnerable;
        }

        public void Damage(float amount, object damageDealer) {
            // Debug.Log($"Adjusting {Owner.name}'s current health by {amount}.");

            if (Invulnerable) return;
            AdjustHealth(-Math.Abs(amount));
        }

        public void Heal(float amount) {
            // Debug.Log($"Adjusting {Owner.name}'s current health by {amount}.");

            AdjustHealth(Math.Abs(amount));
        }

        /// <summary>
        /// Adjust unit's health:
        /// negative values => damage
        /// positive values => heal
        /// </summary>
        /// <param name="amount"></param>
        private void AdjustHealth(float amount) {
            var prevAmount = CurrentHp;
            var newAmount = Mathf.Clamp(CurrentHp + amount, 0, MaxHp);

            // Debug.Log($"Adjusting {Owner.name}'s current health from {prevAmount} to {newAmount}.");

            CurrentHp = newAmount;

            Owner.OnHealthChanged(prevAmount);

            if (!IsDead) return;

            Owner.OnDeath();
        }

        internal void Refill() => AdjustHealth(MaxHp);
    }
}