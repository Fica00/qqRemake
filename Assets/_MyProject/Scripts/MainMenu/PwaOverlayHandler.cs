using TMPro;
using UnityEngine;

public enum PwaOverlay
{
   DidNotOpen,
   DidNotShow,
   Showed
}

public class PwaOverlayHandler : OverlayHandler
{
    [SerializeField] private TextMeshProUGUI message;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
    }


    public void SetupWithText(bool _isAndroid)
    {
        base.Setup();
        
        message.text = _isAndroid
            ? "TO INSTALL THE APP, YOU NEED TO ADD THIS WEBSITE TO YOUR HOME SCREEN. IN YOUR BROWSER MENU, TAP THE THREE DOTS ICON AND CHOOSE \"ADD TO HOME SCREEN\". THEN OPEN THE APP FROM YOUR HOME SCREEN."
            
            : "TO INSTALL THE APP, YOU NEED TO ADD THIS WEBSITE TO YOUR HOME SCREEN. IN YOUR BROWSER MENU, TAP THE SHARE ICON AND CHOOSE \"ADD TO HOME SCREEN\". THEN OPEN THE APP FROM YOUR HOME SCREEN.";
    }
}