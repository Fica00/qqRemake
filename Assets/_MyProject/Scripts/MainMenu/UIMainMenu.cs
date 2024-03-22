using TMPro;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    public static UIMainMenu Instance;
    [SerializeField] private TextMeshProUGUI lineupNameDisplay;
    [SerializeField] private GameObject sceneTransition;

    private void Awake()
    {
        Instance = this;
    }

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

    public void ShowSceneTransition()
    {
        sceneTransition.SetActive(true);
    }
}
