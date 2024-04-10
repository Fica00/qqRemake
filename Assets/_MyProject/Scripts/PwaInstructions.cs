using UnityEngine;

public class PwaInstructions : MonoBehaviour
{
    [SerializeField] private GameObject holder;
    
    private void Start()
    {
        Debug.Log("Checking for browser: "+JavaScriptManager.Instance.IsBrowser);
        if (JavaScriptManager.Instance.IsBrowser)
        {
            holder.SetActive(true);
        }
    }
}
