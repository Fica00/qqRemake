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
        if (Application.isMobilePlatform || Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                yield break;   
            }
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            yield return null;
            yield return null;
            canvas.worldCamera = Camera.main;
            scaler = GetComponent<CanvasScaler>();
            scaler.matchWidthOrHeight = matchFactor;
        }
        else
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                yield break;   
            }
            canvas.renderMode = RenderMode.WorldSpace;
        }
    }
}
