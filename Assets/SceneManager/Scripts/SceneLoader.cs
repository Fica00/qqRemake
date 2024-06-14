using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        public static Action OnLoadedScene;
        [SerializeField] private LoadingCanvas loadingPrefab;
        [SerializeField] private float minimumLoadingDuration;

        /// <summary>
        /// Defines the maximum percentage of the progress bar that the scene loading will occupy when the minimum loading duration is specified.
        /// The remaining percentage up to 100% will be completed during the minimum wait time.
        /// </summary>
        protected float CapLoadingScenePercentage = 60;
        
        private LoadingCanvas loadingDisplay;
        private float totalProgress;

        private float TotalProgress
        {
            get => totalProgress;
            set
            {
                totalProgress = value;
                if (!loadingDisplay)
                {
                    return;
                }
                
                loadingDisplay.UpdateProgress((TotalProgress/2)*CapLoadingScenePercentage);
            }
        }


        protected void LoadScene(string _key, bool _useAsyncLoading)
        {
            if (_useAsyncLoading)
            {
                LoadAsync(_key);
            }
            else
            {
                UnitySceneManager.LoadScene(_key);
            }
        }

        private void LoadAsync(string _key)
        {
            
            TotalProgress = 0;
            Scene _startingScene = UnitySceneManager.GetActiveScene();
            float _startingTime = Time.time;
            loadingDisplay = Instantiate(loadingPrefab);
            
            AsyncOperation _loadingOperation = UnitySceneManager.LoadSceneAsync(_key, LoadSceneMode.Additive);
            _loadingOperation.allowSceneActivation = false;

            StartCoroutine(LoadScene(_loadingOperation,_startingScene,_startingTime));
            
        }

        private IEnumerator LoadScene(AsyncOperation _operation, Scene _previousScene, float _startingTime)
        {
            
            bool _finishedLoading = false;
            float _previousProgress=0;
            while (!_operation.isDone)
            {
                if (!_finishedLoading && _operation.progress >= .9f)
                {
                    _operation.allowSceneActivation = true;
                    _finishedLoading = true;
                    yield return StartCoroutine(UnloadScene(_previousScene,_operation));
                }
                yield return null;
                TotalProgress += _operation.progress - _previousProgress;
                _previousProgress = _operation.progress;
            }
            
            StartCoroutine(FinishLoading(_startingTime));
        }

        private IEnumerator UnloadScene(Scene _previousScene, AsyncOperation _loadingOperation)
        {
           
            while (!_loadingOperation.isDone)
            {
                yield return null;
            }
            
            AsyncOperation _unloadingOperation = UnitySceneManager.UnloadSceneAsync(_previousScene);

            float _previousProgress=0;
            while (!_unloadingOperation.isDone)
            {
                yield return null;
                TotalProgress += _unloadingOperation.progress - _previousProgress;
                _previousProgress = _unloadingOperation.progress;
            }
            
        }

        private IEnumerator FinishLoading(float _startingTime)
        {
            
            yield return StartCoroutine(DoWaitForMinimumTime(_startingTime));
            Destroy(loadingDisplay.gameObject);
            OnLoadedScene?.Invoke();
        }
        
        private IEnumerator DoWaitForMinimumTime(float _startingTime)
        {
            float _endTime = _startingTime + minimumLoadingDuration;
            float _continueFrom = CapLoadingScenePercentage / 100f;
            float _rest = 1 - _continueFrom;

            while (Time.time < _endTime)
            {
                float _currentProgress = Mathf.Clamp01((Time.time - _startingTime) / minimumLoadingDuration);
                TotalProgress = _continueFrom + _rest * _currentProgress;
                loadingDisplay.UpdateProgress(TotalProgress);
                yield return null;
            }
        }
    }
}