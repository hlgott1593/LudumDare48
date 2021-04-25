using System;
using UnityEngine;

namespace LD48.Health {
    [Serializable]
    public class HealthConfig {
        [SerializeField] public float maxHp;
        [SerializeField] public bool invulnerable;

        public HealthConfig(float maxHp, bool invulnerable) {
            this.maxHp = maxHp;
            this.invulnerable = invulnerable;
        }
        
        public HealthConfig(HealthConfig data) {
            maxHp = data.maxHp;
            invulnerable = data.invulnerable;
        }
    }
}