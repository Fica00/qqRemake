using TMPro;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lineupNameDisplay;
    
    private void Start()
    {
        DataManager.Instance.Subscribe();
        ShowLineupName();
        // StartCoroutine(FirebaseManager.Instance.Patch("https://qqweb-b75ae-default-rtdb.firebaseio.com/users/vhk4fBSyVcWT6ku6qIkFSHksNfP2/.json"))
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
            DataManager.Instance.PlayerData.GetDeck(DataManager.Instance.PlayerData.SelectedDeck).Name;
    }
}
