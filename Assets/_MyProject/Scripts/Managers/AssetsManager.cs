using UnityEngine;

public class AssetsManager : MonoBehaviour
{
    public static AssetsManager Instance;
    [field: SerializeField] public GameObject Loading { get; private set; }

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
