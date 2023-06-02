using UnityEngine;

public class CanvasHandler : MonoBehaviour
{
    Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        if (Screen.height > Screen.width)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
    }
}
