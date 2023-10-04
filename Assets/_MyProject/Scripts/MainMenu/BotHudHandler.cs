using System;
using UnityEngine;
using UnityEngine.UI;

public class BotHudHandler : MonoBehaviour
{
    public static BotHudHandler Instance;
    
    [SerializeField] private Button shopButton;
    [SerializeField] private Button mainButton;
    [SerializeField] private Button collectionButton;
    
    [SerializeField] private CollectionPanel collectionPanel;
    [SerializeField] private GameObject mainMenuPanel;


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

    public void ShowShop()
    {
        UIManager.Instance.OkDialog.Setup("This feature is not implemented yet");
    }

    public void ShowMain()
    {
        CloseAll();
        mainMenuPanel.SetActive(true);
    }

    public void ShowCollection()
    {
        CloseAll();
        collectionPanel.Show();
    }

    private void CloseAll()
    {
        mainMenuPanel.SetActive(false);
        collectionPanel.Close();
    }
}
