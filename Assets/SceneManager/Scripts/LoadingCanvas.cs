using System;
using UnityEngine;

namespace SceneManagement
{
    public class LoadingCanvas : MonoBehaviour
    {
        private void OnEnable()
        {
            DontDestroyOnLoad(gameObject);
        }

        public virtual void UpdateProgress(float _progressPercentage)
        {
            throw new NotImplementedException();
        }
    }
}