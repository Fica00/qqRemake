using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    public static UIMainMenu Instance;
    
    [SerializeField] private TransitionAnimation transition;
    [SerializeField] private Button deckQuickButton;
    [SerializeField] private DeckQuickPanel deckQuickPanel;
    [SerializeField] private TextMeshProUGUI deckNameDisplay;
    [SerializeField] private Button showLevelRewards;
    [SerializeField] private Button showSettings;
    [SerializeField] private Button showRank;
    [SerializeField] private Button showMissions;
    public static bool ShowStartingAnimation;

    private void Awake()
    {
        Instance = this;
        if (!ShowStartingAnimation)
        {
            return;
        }
        transition.EndTransition(null);
        ShowStartingAnimation = false;
    }

    private void Start()
    {
        DataManager.Instance.Subscribe();
        MissionManager.Instance.Setup();
        AudioManager.Instance.ChangeBackgroundMusic(AudioManager.MAIN_MENU);
        GuestOverlayHandler.Instance.TryShowGuestOverlay(AuthHandler.IsGuest);
        SocketServerCommunication.Instance.ResetMatchData();

    }

    private void OnEnable()
    {
        PlayerData.UpdatedDeckName += ShowDeckName;
        PlayerData.UpdatedSelectedDeck += ShowDeckName;
        deckQuickButton.onClick.AddListener(ShowQuickDeck);
        showLevelRewards.onClick.AddListener(ShowLevelRewards);
        showSettings.onClick.AddListener(ShowSettings);
        showRank.onClick.AddListener(ShowRankRewards);
        showMissions.onClick.AddListener(ShowMissions);

        ShowDeckName();
    }

    private void OnDisable()
    {
        PlayerData.UpdatedDeckName -= ShowDeckName;
        PlayerData.UpdatedSelectedDeck -= ShowDeckName;
        deckQuickButton.onClick.RemoveListener(ShowQuickDeck);
        showLevelRewards.onClick.RemoveListener(ShowLevelRewards);
        showSettings.onClick.RemoveListener(ShowSettings);
        showRank.onClick.RemoveListener(ShowRankRewards);
        showMissions.onClick.RemoveListener(ShowMissions);
    }

    public void ShowSceneTransition(Action _callBack)
    {
        transition.StartTransition(_callBack);
    }

    private void ShowQuickDeck()
    {
        deckQuickPanel.Setup();
    }

    private void ShowDeckName()
    {
        deckNameDisplay.text = DataManager.Instance.PlayerData.GetSelectedDeck().Name;
    }
    
    private void ShowLevelRewards()
    {
        SceneManager.Instance.LoadLevelPage();
    }

    private void ShowSettings()
    {
        SceneManager.Instance.LoadSettingsPage();
    }
    
    private void ShowRankRewards()
    {
        SceneManager.Instance.LoadRankRewardsPage();
    }

    private void ShowMissions()
    {
        SceneManager.Instance.LoadMissionsPage();
    }

}
