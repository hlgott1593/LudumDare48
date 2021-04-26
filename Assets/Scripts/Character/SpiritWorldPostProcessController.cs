using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

namespace LD48 {
    [RequireComponent(typeof(Volume))]
    public class SpiritWorldPostProcessController : MonoBehaviour {
        private Volume _volume;
        private Sequence _sequence;
        [SerializeField] private float _duration;
        [SerializeField] private float _endValue;
        private void OnEnable() {
            _volume = GetComponent<Volume>();
            Character.GhostFormEntered += EnableVolume;
            Character.CorporealFormEntered += DisableVolume;
        }

        private void OnDisable() {
            Character.GhostFormEntered -= EnableVolume;
            Character.CorporealFormEntered -= DisableVolume;
            _sequence?.Kill();
        }

        private void DisableVolume() {
            _sequence = DOTween.Sequence()
                .Append(DOTween.To(() => _volume.weight, (v) => _volume.weight = v, 0f, _duration))
                .SetEase(Ease.InCirc);
        }

        private void EnableVolume() {
            _sequence = DOTween.Sequence()
                .Append(DOTween.To(() => _volume.weight, (v) => _volume.weight = v, _endValue, _duration))
                .SetEase(Ease.InCirc);
        }
    }
}