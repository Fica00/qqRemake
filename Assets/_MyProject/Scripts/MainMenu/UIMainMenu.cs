using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    public static UIMainMenu Instance;
    public static Action OnFirstTimeExitSettings;

    [SerializeField] private TransitionAnimation transition;
    [SerializeField] private Button deckQuickButton;
    [SerializeField] private DeckQuickPanel deckQuickPanel;
    [SerializeField] private TextMeshProUGUI deckNameDisplay;
    [SerializeField] private Button showLevelRewards;
    [SerializeField] private Button showSettings;
    [SerializeField] private Button showRank;
    [SerializeField] private Button showMissions;
    [SerializeField] private QoomonUnlockingPanel qoomonUnlockingPanel;

    public static bool ShowStartingAnimation;
    private bool hasPickedUpFirstGameReward;
    private bool gotBackToHomeFromSettings;

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
        AudioManager.Instance.ChangeBackgroundMusic(AudioManager.MAIN_MENU);

        bool _didReward = TryRewardAfterFirstGame();
        if (_didReward)
        {
            DialogsManager.Instance.OkDialog.OnOkPressed.AddListener(TryRewardForPwaAndBid);
        }
        else
        {
            TryRewardForPwaAndBid();
        }
    }

    private void TryRewardForPwaAndBid()
    {
        if (JavaScriptManager.Instance.IsOnPc())
        {
            return;
        }
        
        if (JavaScriptManager.Instance.IsPwaPlatform && !DataManager.Instance.PlayerData.HasPickedUpPwaReward && DataManager.Instance.PlayerData.HasPlayedFirstGame)
        {
            JavaScriptManager.Instance.CheckHasBoundAccount(TryToReward);
        }
        
        void TryToReward(bool _didBind)
        {
            if (!_didBind)
            {
                return;
            }
        
            DialogsManager.Instance.OkDialog.OnOkPressed.AddListener(OnOkButtonPressed);
            DialogsManager.Instance.OkDialog.Setup("Your new card is ready!");
        
            void OnOkButtonPressed()
            {
                DataManager.Instance.PlayerData.HasPickedUpPwaReward = true;
                DialogsManager.Instance.OkDialog.OnOkPressed.RemoveListener(OnOkButtonPressed);
                
                int _qoomonId = DataManager.Instance.PlayerData.GetQoomonFromPool();
        
                DataManager.Instance.PlayerData.AddQoomon(_qoomonId);
        
                qoomonUnlockingPanel.Setup(_qoomonId, null);
            }
        }
    }

    private bool TryRewardAfterFirstGame()
    {
        if (!DataManager.Instance.PlayerData.HasFinishedFirstGame || !DataManager.Instance.PlayerData.HasPlayedFirstGame)
        {
            return false;
        }
        
        int _qoomonId = DataManager.Instance.PlayerData.GetQoomonFromPool();

        DataManager.Instance.PlayerData.AddQoomon(_qoomonId);
        DataManager.Instance.PlayerData.HasFinishedFirstGame = false;

        qoomonUnlockingPanel.Setup(_qoomonId, ShowPwaRewardDialog);
        return true;
        void ShowPwaRewardDialog()
        {
            DialogsManager.Instance.OkDialog.Setup("Bind with your social account and add app to home screen to unlock another card!");
        }
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