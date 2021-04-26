using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using LD48.Audio;
using UnityEngine;
using Random = UnityEngine.Random;


namespace LD48
{
    public class SceneLoadingTest : MonoBehaviour
    {
        public CanvasGroup Fade;
        public float ExitFadeDuration=0.2f;

        [SerializeField] protected string LoreText = "";
        [SerializeField] protected List<AudioClip> SfxStart = new List<AudioClip>();
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
            SceneLoadingManager.LoreText = LoreText;
            SceneLoadingManager.LoadScene(sceneName);
        }
        

        public void ExitGame()
        {
            Application.Quit();
        }
        

        public void PlayStartSfx()
        {
            if (SfxStart.Count == 0) return;
            var chosen = Mathf.FloorToInt(Random.Range(0, SfxStart.Count - 1));
            var clip = SfxStart[chosen];
            if (clip != null)
            {
                AudioManager.Instance.PlaySfx(clip);   
            }
        }

    }
}
