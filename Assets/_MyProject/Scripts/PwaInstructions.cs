using UnityEngine;

public class PwaInstructions : MonoBehaviour
{
    [SerializeField] private GameObject holder;
    
    private void Start()
    {
        if (!JavaScriptManager.Instance.IsBrowser)
        {
            return;
        }
       
        holder.SetActive(true);
    }
}
