using LD48.Health;
using UnityEngine;

namespace LD48 {
    [RequireComponent(typeof(Collider2D))]
    public class DamageOtherOnCollision : MonoBehaviour {
        public int _damageAmount = 1;

        private void OnTriggerEnter2D(Collider2D other) {
            var damagable = other.gameObject.transform.root.GetComponentInChildren<IDamagable>();
            if (damagable as Object == null) return;
            damagable.HealthSystem.Damage(_damageAmount, gameObject);
        }
    }
}