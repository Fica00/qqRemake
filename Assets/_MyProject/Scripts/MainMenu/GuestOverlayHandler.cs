using System;
using UnityEngine;
using UnityEngine.UI;

public class GuestOverlayHandler : MonoBehaviour
{
    public static GuestOverlayHandler Instance;

    [SerializeField] private GameObject guestOverlay;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button settingsButton;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        closeButton.onClick.AddListener(CloseOverlay);
        settingsButton.onClick.AddListener(ShowSettings);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(CloseOverlay);
        settingsButton.onClick.RemoveListener(ShowSettings);
    }

    public void TryShowGuestOverlay(bool _isGuest)
    {
        if (_isGuest && !DataManager.Instance.IsGuestOverlayShown)
        {
            guestOverlay.SetActive(true);
            DataManager.Instance.IsGuestOverlayShown = true;
        }
    }

    private void ShowSettings()
    {
        guestOverlay.SetActive(false);
        SceneManager.Instance.LoadSettingsPage();
    }

    private void CloseOverlay() => guestOverlay.SetActive(false);
}