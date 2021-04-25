using System;
using DG.Tweening;
using UnityEngine;

namespace LD48 {
    [RequireComponent((typeof(SpriteRenderer)))]
    public class AdjustBlurOnWorldChange : MonoBehaviour {
        public const int CorporealLayer = 8;
        private bool IsCorporeal() => gameObject.layer.Equals(CorporealLayer);

        private static readonly int BlurAmount = Shader.PropertyToID("_BlurAmount");
        private Material _material;
        [SerializeField] private float _blurAmount;
        [SerializeField] private float _alphaAmount;
        [SerializeField] private float _duration;
        private Sequence _sequence;
        private static readonly int AlphaRemoval = Shader.PropertyToID("_AlphaRemoval");

        private void OnEnable() {
            _material = GetComponent<SpriteRenderer>().material;
            if (IsCorporeal()) {
                Character.GhostFormEntered += Blur;
                Character.CorporealFormEntered += Focus;
            }
            else {
                Character.GhostFormEntered += Focus;
                Character.CorporealFormEntered += Blur;
            }
        }

        public void Blur() {
            _sequence = DOTween.Sequence()
                .Append(DOTween.To(() => _material.GetFloat(BlurAmount), v => _material.SetFloat(BlurAmount, v), _blurAmount, _duration))
                .Append(_material.DOFade(_alphaAmount, _duration))
                // .Append(DOTween.To(() => _material.GetFloat(AlphaRemoval), v => _material.SetFloat(AlphaRemoval, v), _alphaAmount, _duration))
                .SetEase(Ease.InCubic);
        }

        private void Focus() {
            _sequence = DOTween.Sequence()
                .Append(DOTween.To(() => _material.GetFloat(BlurAmount), v => _material.SetFloat(BlurAmount, v), 0, _duration))
                .Append(_material.DOFade(1, _duration))
                // .Append(DOTween.To(() => _material.GetFloat(AlphaRemoval), v => _material.SetFloat(AlphaRemoval, v), 0, _duration))
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