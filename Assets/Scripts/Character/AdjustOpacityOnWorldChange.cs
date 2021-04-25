using System.Collections;
using DG.Tweening;
using SuperTiled2Unity;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LD48 {
    [RequireComponent(typeof(Tilemap))]
    public class AdjustOpacityOnWorldChange : MonoBehaviour {
        public const int CorporealGroundLayer = 11;
        public const int SpiritGroundLayer = 10;
        private bool IsCorporeal() => gameObject.layer.Equals(CorporealGroundLayer);
        private bool IsSprit() => gameObject.layer.Equals(SpiritGroundLayer);

        private Tilemap _thing;
        [SerializeField] private float _alphaAmount;
        [SerializeField] private float _duration;
        private Sequence _sequence;

        private void OnEnable() {
            _thing = GetComponent<Tilemap>();
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
            StopAllCoroutines();
            StartCoroutine(FadeAlpha());
        }

        private IEnumerator FadeAlpha() {
            _thing.color = Color.white;
            while (_thing.color.a > _alphaAmount) {
                _thing.color = new Color(1,1,1,_thing.color.a - Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            _thing.color = new Color(1,1,1,_alphaAmount);
        }

        private IEnumerator AddAlpha() {
            _thing.color = Color.white;
            while (_thing.color.a > 1) {
                _thing.color = new Color(1,1,1,_thing.color.a - Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            _thing.color = Color.white;
        }
        private void Focus() {
            StopAllCoroutines();
            StartCoroutine(AddAlpha());
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

            StopAllCoroutines();
        }
    }
}