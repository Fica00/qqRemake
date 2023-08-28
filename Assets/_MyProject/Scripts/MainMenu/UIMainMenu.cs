using TMPro;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI welcomeMessage;
    
    private void Start()
    {
        welcomeMessage.text = "Hello "+DataManager.Instance.PlayerData.Name+"!";
    }
}
