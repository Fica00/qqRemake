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
        PlayerData.UpdatedDeckName += ShowLineupName;
        PlayerData.UpdatedSelectedDeck += ShowLineupName;
    }

    private void OnDisable()
    {
        PlayerData.UpdatedDeckName -= ShowLineupName;
        PlayerData.UpdatedSelectedDeck -= ShowLineupName;
    }

    private void ShowLineupName()
    {
        lineupNameDisplay.text =
            DataManager.Instance.PlayerData.GetSelectedDeck().Name;
    }
}
