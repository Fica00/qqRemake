
public class FirstTimePlayOverlay : OverlayHandler
{
    private void OnEnable()
    {
        closeButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
    }

    //play button logic is in uiplaypanel
    //to refactor later
}