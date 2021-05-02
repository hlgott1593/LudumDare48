using UnityEngine;

namespace LD48 {
    [RequireComponent(typeof(ParticleSystem))]
    public class DestroyIfParticleFinishedPlaying : MonoBehaviour {
        private ParticleSystem _particle;

        private void Awake() {
            _particle = GetComponent<ParticleSystem>();
        }

        private void Update() {
            if (_particle.isPlaying) return;
            Destroy(gameObject);
            enabled = false;
        }
    }
}