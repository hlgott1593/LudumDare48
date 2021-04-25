using UnityEngine;

namespace LD48.Health {
    public interface IDamagable {
        HealthSystem HealthSystem { get; }
        GameObject Behaviour { get; }


        void OnHealthChanged(float prevAmount);
        void OnDeath();
    }
}