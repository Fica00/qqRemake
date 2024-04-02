using UnityEngine;
using UnityEngine.UI;

public class BotHudHandler : MonoBehaviour
{
    public static BotHudHandler Instance;
    
    [SerializeField] private Button shopButton;
    [SerializeField] private Button mainButton;
    [SerializeField] private Button collectionButton;
    
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        shopButton.onClick.AddListener(ShowShop);
        mainButton.onClick.AddListener(ShowMain);
        collectionButton.onClick.AddListener(ShowCollection);
    }

    private void OnDisable()
    {
        shopButton.onClick.RemoveListener(ShowShop);
        mainButton.onClick.RemoveListener(ShowMain);
        collectionButton.onClick.RemoveListener(ShowCollection);
    }

    private void ShowShop()
    {
    }

    private void ShowMain()
    {
        SceneManager.Instance.LoadMainMenu();
    }

    private void ShowCollection()
    {
        SceneManager.Instance.LoadCollectionPage();
    }
}
