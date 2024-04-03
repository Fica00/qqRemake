using UnityEngine;

public class DemoObjects : MonoBehaviour
{
    [SerializeField] private bool showOnDemo = true;
    [SerializeField] private bool showOnDev = false;
    
    private void Start()
    {
        if (JavaScriptManager.Instance.IsDemo)
        {
            gameObject.SetActive(showOnDemo);
        }
        else
        {
            gameObject.SetActive(showOnDev);
        }
    }
}