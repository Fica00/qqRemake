using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    public static UIMainMenu Instance;
    public static Action OnFirstTimeExitSettings;
    public static Action OnShowLineup;
    public static Action OnGoToCollection;

    [SerializeField] private TransitionAnimation transition;
    [SerializeField] private Button deckQuickButton;
    [SerializeField] private DeckQuickPanel deckQuickPanel;
    [SerializeField] private TextMeshProUGUI deckNameDisplay;
    [SerializeField] private Button showLevelRewards;
    [SerializeField] private Button showSettings;
    [SerializeField] private Button showRank;
    [SerializeField] private Button showMissions;
    [SerializeField] private Button discordButton;
    [SerializeField] private QoomonUnlockingPanel qoomonUnlockingPanel;
    [SerializeField] private UIPlayPanel uiPlayPanel;
    [SerializeField] private UIHereIsLineupPanel uiHereIsLineupPanel;
    [SerializeField] private UIGoToCollectionPanel uiGoToCollectionPanel;

    
    public static bool ShowStartingAnimation;
    public static bool HasPlayedFirstAiGame;
    public static bool HasShowenDeckTutorial;
    private bool hasPickedUpFirstGameReward;
    private bool gotBackToHomeFromSettings;
    private bool hasGoToCollectionShowed;

    private void Awake()
    {
        Instance = this;
        if (!ShowStartingAnimation)
        {
            return;
        }
        
        if (HasPlayedFirstAiGame)
        {
            transition.EndTransition(OnGoToCollection);
            hasGoToCollectionShowed = true;
            HasPlayedFirstAiGame = true;
            Debug.Log(1.1);
        }
        else
        {
            transition.EndTransition(OnShowLineup);
        }
        ShowStartingAnimation = false;
        
    }
    
  
    
    
    private void Start()
    {
        // if (hasGoToCollectionShowed)
        // {
        //     uiGoToCollectionPanel.Setup();
        //     hasGoToCollectionShowed = false;
        // }
        AudioManager.Instance.ChangeBackgroundMusic(AudioManager.MAIN_MENU);

        if (HasShowenDeckTutorial)
        {
            bool _didReward = TryRewardAfterFirstGame();
            if (!_didReward)
            {
                bool _canRewardPwa = TryRewardForPwaAndBid();
                if (!_canRewardPwa)
                {
                    TryToAutoMatch();
                }
            }
        }
        
        JavaScriptManager.Instance.CheckHasBoundAccount(SaveIsGuest);
    }

    private void SaveIsGuest(bool _hasBoundedAccount)
    {
        DataManager.Instance.PlayerData.IsGuest = !_hasBoundedAccount;
    }

    private bool TryRewardForPwaAndBid()
    {
        if (JavaScriptManager.Instance.IsOnPc())
        {
            return false;
        }

        if (!JavaScriptManager.Instance.IsPwaPlatform)
        {
            return false;
        }

        if (DataManager.Instance.PlayerData.HasPickedUpPwaReward)
        {
            return false;
        }

        if (!DataManager.Instance.PlayerData.HasPlayedFirstGame)
        {
            return false;
        }

        JavaScriptManager.Instance.CheckHasBoundAccount(TryToReward);
        return true;

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
        
                qoomonUnlockingPanel.Setup(_qoomonId, TryToAutoMatch);
            }
        }
    }

    private void TryToAutoMatch()
    {
        uiPlayPanel.TryAutoMatch();
    }

    private bool TryRewardAfterFirstGame()
    {
        if (!DataManager.Instance.PlayerData.HasFinishedFirstGame)
        {
            return false;
        }
        if (!DataManager.Instance.PlayerData.HasPlayedFirstGame)
        {
            return false;
        }
        
        DialogsManager.Instance.OkDialog.OnOkPressed.AddListener(RewardQoomon);
        DialogsManager.Instance.OkDialog.Setup("You won a new qoomon for completing first game!");
        return true;
        
        

        void RewardQoomon()
        {
            int _qoomonId = DataManager.Instance.PlayerData.GetQoomonFromPool(); //TODO: {STEFAN} Ovde možeš da setuješ qoomona

            DataManager.Instance.PlayerData.AddQoomon(_qoomonId);
            DataManager.Instance.PlayerData.HasFinishedFirstGame = false;

            qoomonUnlockingPanel.Setup(_qoomonId, ManagePwaDialogAndOverlay);
        }

        void ManagePwaDialogAndOverlay()
        {
            DialogsManager.Instance.OkDialog.OnOkPressed.AddListener(() =>
            {
                bool _canReward = TryRewardForPwaAndBid();
                if (!_canReward)
                {
                    TryToAutoMatch();
                }
            });
            DialogsManager.Instance.OkDialog.Setup("Bind with your social account and add app to home screen to unlock another card!");
            DataManager.Instance.CanShowPwaOverlay = true;
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
        discordButton.onClick.AddListener(NoteDiscord);

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
        discordButton.onClick.RemoveListener(NoteDiscord);
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
    
    private void NoteDiscord()
    {
        DataManager.Instance.PlayerData.Statistics.NoteCheckPoint(BackgroundHandler.DISCORD_CLICK);
    }
}