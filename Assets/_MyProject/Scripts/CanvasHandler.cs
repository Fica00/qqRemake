using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHandler : MonoBehaviour
{
    [SerializeField] private float matchFactor = 1;
    private Canvas canvas;
    private CanvasScaler scaler;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        StartCoroutine(ManageCanvas());
    }

    private IEnumerator ManageCanvas()
    {
        if (Screen.height > Screen.width || Application.platform == RuntimePlatform.WindowsEditor)
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            yield return null;
            yield return null;
            canvas.worldCamera = Camera.main;
            scaler = GetComponent<CanvasScaler>();
            scaler.matchWidthOrHeight = matchFactor;
        }
        else
        {
            canvas.renderMode = RenderMode.WorldSpace;
        }
    }
}
