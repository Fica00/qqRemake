using TMPro;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lineupNameDisplay;
    
    private void Start()
    {
        DataManager.Instance.Subscribe();
        ShowLineupName();
    }

    private void OnEnable()
    {
        DataManager.Instance.PlayerData.UpdatedDeckName += ShowLineupName;
        DataManager.Instance.PlayerData.UpdatedSelectedDeck += ShowLineupName;
    }

    private void OnDisable()
    {
        DataManager.Instance.PlayerData.UpdatedDeckName -= ShowLineupName;
        DataManager.Instance.PlayerData.UpdatedSelectedDeck -= ShowLineupName;
    }

    private void ShowLineupName()
    {
        lineupNameDisplay.text =
            DataManager.Instance.PlayerData.GetDeck(DataManager.Instance.PlayerData.SelectedDeck).Name;
    }
}
