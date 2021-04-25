using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LD48 {
    public class OnTriggerExit : MonoBehaviour {
        public string _methodName;

        private void OnTriggerExit2D(Collider2D other) {
            SendMessageUpwards(_methodName, other);
        }
    }
}