using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LD48 {
    [RequireComponent(typeof(Tilemap))]
    public class AdjustOpacityOnWorldChange : MonoBehaviour {
        public const int CorporealGroundLayer = 11;
        public const int SpiritGroundLayer = 10;
        private bool IsCorporeal() => gameObject.layer.Equals(CorporealGroundLayer);
        private bool IsSprit() => gameObject.layer.Equals(SpiritGroundLayer);

        private Tilemap _tilemap;
        [SerializeField] private float _alphaAmount;
        [SerializeField] private float _duration;
        private Sequence _sequence;

        private void OnEnable() {
            _tilemap = GetComponent<Tilemap>();
            if (IsCorporeal()) {
                Character.GhostFormEntered += Blur;
                Character.CorporealFormEntered += Focus;
            }
            else if (IsSprit()) {
                Character.GhostFormEntered += Focus;
                Character.CorporealFormEntered += Blur;
            }
        }

        public void Blur() {
            _sequence = DOTween.Sequence()
                .Append(DOTween.To(() => _tilemap.color.a, v => _tilemap.color = new Color(1,1,1,v), _alphaAmount, _duration))
                .SetEase(Ease.InCubic);
        }

        private void Focus() {
            _sequence = DOTween.Sequence()
                .Append(DOTween.To(() => _tilemap.color.a, v => _tilemap.color = new Color(1,1,1,v), 1, _duration))
                .SetEase(Ease.InCubic);
        }

        private void OnDisable() {
            if (IsCorporeal()) {
                Character.GhostFormEntered -= Blur;
                Character.CorporealFormEntered -= Focus;
            }
            else {
                Character.GhostFormEntered -= Focus;
                Character.CorporealFormEntered -= Blur;
            }

            _sequence?.Kill();
        }
    }
}