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
    int roundDuration;
    float timeLeft;
    Coroutine roundDurationRoutine;
    public float TimeLeft => timeLeft;

    private void OnEnable()
    {
        GameplayManager.UpdatedGameState += HandleGameState;
        button.onClick.AddListener(EndTurn);
    }

    private void OnDisable()
    {
        GameplayManager.UpdatedGameState -= HandleGameState;
        button.onClick.RemoveListener(EndTurn);
    }

    void HandleGameState()
    {
        switch (GameplayManager.Instance.GameplayState)
        {
            case GameplayState.ResolvingBeginingOfRound:
                textDisplay.text = "Playing";
                button.interactable = false;
                timeLeft = roundDuration;
                break;
            case GameplayState.Playing:
                roundDurationRoutine = StartCoroutine(RoundDurationRoutine());
                textDisplay.text = "End Turn";
                button.interactable = true;
                break;
            case GameplayState.Waiting:
                textDisplay.text = "Waiting";
                button.interactable = false;
                break;
            case GameplayState.ResolvingEndOfRound:
                textDisplay.text = "Playing";
                button.interactable = false;
                break;
            default:
                break;
        }
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
