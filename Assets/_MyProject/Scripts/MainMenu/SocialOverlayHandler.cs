
public class SocialOverlayHandler : OverlayHandler
{
    private void OnEnable()
    {
        closeButton.onClick.AddListener(Close);
    }
    
    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
    }
}