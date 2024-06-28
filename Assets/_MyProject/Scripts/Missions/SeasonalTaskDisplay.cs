using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SeasonalTaskType
{
    SocialAccount,
    Pwa
}

public class SeasonalTaskDisplay : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI taskDescription;

    public void Setup(SeasonalTaskType _taskType)
    {
        switch (_taskType)
        {
            case SeasonalTaskType.SocialAccount:
                taskDescription.text = "Connect social (In settings)";
                button.onClick.AddListener(ShowSettings);
                break;
            case SeasonalTaskType.Pwa:
                taskDescription.text = "Install to mobile home screen (Tap for info)";
                button.onClick.AddListener(ShowPwaPopup);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        gameObject.SetActive(true);
    }
    
    private void ShowSettings()
    {
        SceneManager.Instance.LoadSettingsPage();
    }
    
    private void ShowPwaPopup()
    {
        MissionPanel.Instance.pwaOverlay.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
}