using UnityEngine;
using UnityEngine.Events;

namespace LD48 {
    [RequireComponent(typeof(Collider2D))]
    public class OnTriggerEnter : MonoBehaviour {
        [SerializeField] private UnityEvent _onTriggerEnter;

        private void OnTriggerEnter2D(Collider2D other) {
            _onTriggerEnter?.Invoke();
        }
    }
}