using System;
using TMPro;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    public static UIMainMenu Instance;
    [SerializeField] private TextMeshProUGUI lineupNameDisplay;
    [SerializeField] private TransitionAnimation transition;

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

    public void ShowSceneTransition(Action _callBack)
    {
        transition.StartTransition(_callBack);
    }
}
