using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class EndTurnHandler : MonoBehaviour
{
    public static Action OnEndTurn;
    
    [SerializeField] private TextMeshProUGUI textDisplay;
    [SerializeField] private Button button;
    [SerializeField] private GameObject roundDisplay;
    [SerializeField] private GameObject leaveDisplay;
    
    [SerializeField] private GradiantValueBar gradiantBar;
    [SerializeField] private Sprite playing;
    [SerializeField] private Sprite waiting;
    [SerializeField] private GradiantSprite endTurn;

    private int roundDuration;
    private float timeLeft;
    private Coroutine roundDurationRoutine;
    private bool hasPlayed;

    public float TimeLeft => timeLeft;

    private void OnEnable()
    {
        GameplayManager.UpdatedGameState += HandleGameState;
        GameplayManager.GameEnded += ManageGameEnded;
        button.onClick.AddListener(EndTurn);
    }

    private void OnDisable()
    {
        GameplayManager.UpdatedGameState -= HandleGameState;
        GameplayManager.GameEnded -= ManageGameEnded;
        button.onClick.RemoveAllListeners();
    }

    private void HandleGameState()
    {
        switch (GameplayManager.Instance.GameplayState)
        {
            case GameplayState.ResolvingBeginingOfRound:
                textDisplay.text = "Playing";
                button.interactable = false;
                gradiantBar.SetForeground(playing,true);
                timeLeft = roundDuration;
                break;
            case GameplayState.Playing:
                hasPlayed = false;
                if (roundDurationRoutine==null)
                {
                    roundDurationRoutine = StartCoroutine(RoundDurationRoutine());
                }
                textDisplay.text = "End Turn";
                button.interactable = true;
                gradiantBar.SetSprites(endTurn);
                break;
            case GameplayState.Waiting:
                textDisplay.text = "Waiting";
                button.interactable = false;
                gradiantBar.SetForeground(waiting,true);
                break;
            case GameplayState.ResolvingEndOfRound:
                if (roundDurationRoutine != null)
                {
                    StopCoroutine(roundDurationRoutine);
                    roundDurationRoutine = null;
                }
                textDisplay.text = "Playing";
                button.interactable = false;
                gradiantBar.SetForeground(playing,true);
                gradiantBar.SetAmount(0);
                break;
        }
    }

    private void ManageGameEnded(GameResult _result)
    {
        button.onClick.RemoveListener(EndTurn);
        button.onClick.AddListener(LeaveScene);
        gradiantBar.SetSprites(endTurn);
        Destroy(roundDisplay);
        Destroy(textDisplay.gameObject);
        leaveDisplay.SetActive(true);
        button.interactable = true;
        StopAllCoroutines();
    }

    private void LeaveScene()
    {
        SceneManager.Instance.LoadMainMenu();
    }

    private void EndTurn()
    {
        if (hasPlayed)
        {
            return;
        }

        hasPlayed = true;
        OnEndTurn?.Invoke();
    }

    private void Start()
    {
        roundDuration = GameplayManager.Instance.DurationOfRound;
    }

    private IEnumerator RoundDurationRoutine()
    {
        if (SceneManager.IsGameplayTutorialScene)
        {
            yield break;
        }
        while (timeLeft > 0)
        {
            float _value = timeLeft / roundDuration;
            gradiantBar.SetAmount(_value);
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        EndTurn();
    }
}
