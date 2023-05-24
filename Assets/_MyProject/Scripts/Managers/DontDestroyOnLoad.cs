using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    static DontDestroyOnLoad Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
