using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI welcomeMessage;
    [SerializeField] private Button collectionButton;
    [SerializeField] private CollectionPanel collectionPanel;
    
    private void Start()
    {
        welcomeMessage.text = "Hello "+DataManager.Instance.PlayerData.Name+"!";
    }

    private void OnEnable()
    {
        collectionButton.onClick.AddListener(ShowCollection);
    }

    private void OnDisable()
    {
        collectionButton.onClick.RemoveListener(ShowCollection);
    }

    private void ShowCollection()
    {
        collectionPanel.Show();
    }
}
