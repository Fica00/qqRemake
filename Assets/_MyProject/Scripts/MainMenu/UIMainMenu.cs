using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    public static UIMainMenu Instance;
    
    [SerializeField] private TransitionAnimation transition;
    [SerializeField] private Button showRanked;
    [SerializeField] private GameObject rankedHolder;
    [SerializeField] private Button deckQuickButton;
    [SerializeField] private DeckQuickPanel deckQuickPanel;
    [SerializeField] private TextMeshProUGUI deckNameDisplay;
    [SerializeField] private Button showLevelRewards;
    [SerializeField] private LevelRewardsPanel levelRewardsPanel; 
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
    }

    private void OnEnable()
    {
        PlayerData.UpdatedDeckName += ShowDeckName;
        PlayerData.UpdatedSelectedDeck += ShowDeckName;
        showRanked.onClick.AddListener(ShowRanked);
        deckQuickButton.onClick.AddListener(ShowQuickDeck);
        showLevelRewards.onClick.AddListener(ShowLevelRewards);

        ShowDeckName();
    }

    private void OnDisable()
    {
        PlayerData.UpdatedDeckName -= ShowDeckName;
        PlayerData.UpdatedSelectedDeck -= ShowDeckName;
        showRanked.onClick.RemoveListener(ShowRanked);
        deckQuickButton.onClick.RemoveListener(ShowQuickDeck);
        showLevelRewards.onClick.RemoveListener(ShowLevelRewards);
    }

    public void ShowSceneTransition(Action _callBack)
    {
        transition.StartTransition(_callBack);
    }

    private void ShowRanked()
    {
        rankedHolder.SetActive(false);
    }

    private void ShowQuickDeck()
    {
        deckQuickPanel.Setup();
    }

    private void ShowDeckName()
    {
        deckNameDisplay.text = "Lineup\n"+DataManager.Instance.PlayerData.GetSelectedDeck().Name;
    }
    
    private void ShowLevelRewards()
    {
        levelRewardsPanel.Setup();
    }
}
