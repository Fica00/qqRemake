using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHandler : MonoBehaviour
{
    [SerializeField] float matchFactor = 1;
    Canvas canvas;
    CanvasScaler scaler;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        StartCoroutine(ManageCanvas());
    }

    IEnumerator ManageCanvas()
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
