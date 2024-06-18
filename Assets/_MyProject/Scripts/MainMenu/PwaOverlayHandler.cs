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

    public override void Close()
    {
        base.Close();
        DataManager.Instance.CanShowPwaOverlay = false;
    }

    public void SetupWithText(bool _isAndroid)
    {
        base.Setup();
        
        message.text = _isAndroid
            ? "INSTALL THE APP FOR BETTER EXPERIENCE. IN YOUR MENU, TAP THE THREE DOTS ICON AND CHOOSE \"ADD TO HOME SCREEN/INSTALL APP\" AND LAUNCH THE APP"
            : "INSTALL THE APP FOR BETTER EXPERIENCE. IN YOUR MENU, TAP THE SHARE DOTS ICON AND CHOOSE \"ADD TO HOME SCREEN\" AND LAUNCH THE APP";
    }
}