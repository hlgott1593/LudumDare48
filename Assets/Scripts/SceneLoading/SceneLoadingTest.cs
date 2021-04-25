using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace LD48
{
    public class SceneLoadingTest : MonoBehaviour
    {
        public CanvasGroup Fade;
        public float ExitFadeDuration=0.2f;
        
        private bool _loading = false;
        
        public void StartLoad(string sceneName)
        {
            if (_loading) return;
            _loading = true;
            StartCoroutine(Load(sceneName));

        }
        
        IEnumerator Load(string sceneName)
        {
            var tween2 = DOTween.Sequence().Append(DOTween.To(() => Fade.alpha, (v) => Fade.alpha = v, 1f, ExitFadeDuration)).SetEase(Ease.InCirc);
            yield return new WaitForSeconds(ExitFadeDuration);
            SceneLoadingManager.LoadScene(sceneName);
        }

    }
}
