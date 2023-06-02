using System.Collections;
using UnityEngine;

public class CanvasHandler : MonoBehaviour
{
    Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        //StartCoroutine(ManageCanvas());
    }

    IEnumerator ManageCanvas()
    {
        if (Screen.height > Screen.width)
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            yield return null;
            canvas.worldCamera = Camera.main;
            yield return null;
            canvas.scaleFactor = 1;
        }
    }
}
