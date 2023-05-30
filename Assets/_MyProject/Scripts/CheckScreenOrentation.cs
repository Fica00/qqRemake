using UnityEngine;
using UnityEngine.UI;

public class CheckScreenOrentation : MonoBehaviour
{
    private CanvasScaler canvasScaler;

    private void Awake()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        CheckOrientation();
    }
    private void CheckOrientation()
    {
        if (Screen.width > Screen.height)
        {
            Debug.Log("Landscape detected");
            if (canvasScaler != null)
            {
                Debug.Log("Removing scaler");
                Destroy(canvasScaler);
            }
        }
    }
}
