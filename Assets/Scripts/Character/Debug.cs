using UnityEngine;

namespace LD48 {
    public class Debug : MonoBehaviour {
        public void IncreaseTime() {
            AdjustTime(1.25f);
        }

        public void DecreaseTime() {
            AdjustTime(0.75f);
        }

        public void AdjustTime(float adjustment) {
#if UNITY_EDITOR
            Time.timeScale *= adjustment;
#endif
        }
    }
}