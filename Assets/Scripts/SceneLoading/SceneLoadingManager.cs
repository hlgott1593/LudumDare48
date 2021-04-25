using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LD48
{
    public class SceneLoadingManager : MonoBehaviour
    {
        public float StartFadeDuration=0.2f;
        public float ProgressBarSpeed=2f;
        public float ExitFadeDuration=0.2f;
        public float LoadCompleteDelay=0.5f;
        public Image _progressBarImage;
        
        
        public static string LoadingScreenSceneName = "LoadingScreen";
        protected static string _sceneToLoad = "";
        protected AsyncOperation _asyncOperation;
        protected float _progressFillTarget=0f;
        
        
        
        public static void LoadScene(string sceneToLoad)
        {
            _sceneToLoad = sceneToLoad;	
            SceneManager.LoadScene(LoadingScreenSceneName);
        }
        protected virtual void Start()
        {
            // _tween = new MMTweenType(MMTween.MMTweenCurve.EaseOutCubic);
            // _progressBarImage = LoadingProgressBar.GetComponent<Image>();
            // _loadingTextValue =LoadingText.text;
            
            if (!string.IsNullOrEmpty(_sceneToLoad))
            {
                StartCoroutine(LoadAsynchronously());
            }        
        }

        protected void Update()
        {
            _progressBarImage.fillAmount = _progressFillTarget;
        }

        protected virtual IEnumerator LoadAsynchronously() 
        {
            yield return new WaitForSeconds(StartFadeDuration);
            
            // we start loading the scene
            _asyncOperation = SceneManager.LoadSceneAsync(_sceneToLoad,LoadSceneMode.Single );
            _asyncOperation.allowSceneActivation = false;

            // while the scene loads, we assign its progress to a target that we'll use to fill the progress bar smoothly
            while (_asyncOperation.progress < 0.9f) 
            {
                _progressFillTarget = _asyncOperation.progress;
                yield return null;
            }
            // when the load is close to the end (it'll never reach it), we set it to 100%
            _progressFillTarget = 1f;

            // we wait for the bar to be visually filled to continue
            while (_progressBarImage.fillAmount != _progressFillTarget)
            {
                yield return null;
            }

            // the load is now complete, we replace the bar with the complete animation
            LoadingComplete();
            yield return new WaitForSeconds(LoadCompleteDelay);

            yield return new WaitForSeconds(ExitFadeDuration);

            // we switch to the new scene
            _asyncOperation.allowSceneActivation = true;
        }

        protected virtual void LoadingComplete()
        {
            
        }
    }
}
