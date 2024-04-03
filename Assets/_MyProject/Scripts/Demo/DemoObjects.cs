using UnityEngine;

public class DemoObjects : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(!JavaScriptManager.Instance.IsDemo);
    }
}