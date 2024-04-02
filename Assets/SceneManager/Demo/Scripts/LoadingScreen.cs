using SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : LoadingCanvas
{
    [SerializeField] private Image progressBar;

    public override void UpdateProgress(float _progressPercentage)
    {
        progressBar.fillAmount = _progressPercentage;
    }
}
