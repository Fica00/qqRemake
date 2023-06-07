using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class EndTurnHandler : MonoBehaviour
{
    public static Action OnEndTurn;
    
    [SerializeField] TextMeshProUGUI textDisplay;
    [SerializeField] Image foregroundImage;
    [SerializeField] Button button;
    [SerializeField] Color clickableColor;
    [SerializeField] Color unclickableColor;

    int roundDuration;
    float timeLeft;
    Coroutine roundDurationRoutine;

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

    void HandleGameState()
    {
        switch (GameplayManager.Instance.GameplayState)
        {
            case GameplayState.ResolvingBeginingOfRound:
                textDisplay.text = "Playing";
                button.interactable = false;
                foregroundImage.color = unclickableColor;
                timeLeft = roundDuration;
                break;
            case GameplayState.Playing:
                roundDurationRoutine = StartCoroutine(RoundDurationRoutine());
                textDisplay.text = "End Turn";
                button.interactable = true;
                foregroundImage.color = clickableColor;
                break;
            case GameplayState.Waiting:
                textDisplay.text = "Waiting";
                button.interactable = false;
                foregroundImage.color = unclickableColor;
                break;
            case GameplayState.ResolvingEndOfRound:
                textDisplay.text = "Playing";
                button.interactable = false;
                foregroundImage.color = unclickableColor;
                break;
            default:
                break;
        }
    }

    void ManageGameEnded(GameResult _result)
    {
        button.onClick.RemoveListener(EndTurn);
        button.onClick.AddListener(LeaveScene);
        textDisplay.text = "Leave";
        foregroundImage.fillAmount = 1;
        foregroundImage.color = clickableColor;
        button.interactable = true;
        StopAllCoroutines();
    }

    void LeaveScene()
    {
        SceneManager.LoadMainMenu();
    }

    void EndTurn()
    {
        if (roundDurationRoutine != null)
        {
            StopCoroutine(roundDurationRoutine);
        }
        OnEndTurn?.Invoke();
    }

    private void Start()
    {
        roundDuration = GameplayManager.Instance.DurationOfRound;
    }

    IEnumerator RoundDurationRoutine()
    {
        while (timeLeft > 0)
        {
            foregroundImage.fillAmount = timeLeft / roundDuration;
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        EndTurn();
    }
}
