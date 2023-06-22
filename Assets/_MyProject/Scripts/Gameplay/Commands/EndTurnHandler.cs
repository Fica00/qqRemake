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
    [SerializeField] GameObject roundDisplay;
    [SerializeField] GameObject leaveDisplay;
    [SerializeField] private Sprite waitingSprite;
    [SerializeField] private Sprite playingSprite;
    [SerializeField] private Sprite timerSprite;

    int roundDuration;
    float timeLeft;
    Coroutine roundDurationRoutine;
    bool hasPlayed;

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
                foregroundImage.sprite = playingSprite;
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
                foregroundImage.sprite = timerSprite;
                break;
            case GameplayState.Waiting:
                textDisplay.text = "Waiting";
                button.interactable = false;
                foregroundImage.sprite = waitingSprite;
                break;
            case GameplayState.ResolvingEndOfRound:
                if (roundDurationRoutine != null)
                {
                    StopCoroutine(roundDurationRoutine);
                    roundDurationRoutine = null;
                }
                textDisplay.text = "Playing";
                button.interactable = false;
                foregroundImage.sprite = playingSprite;
                foregroundImage.fillAmount = 0;
                break;
            default:
                break;
        }
    }

    void ManageGameEnded(GameResult _result)
    {
        button.onClick.RemoveListener(EndTurn);
        button.onClick.AddListener(LeaveScene);
        foregroundImage.fillAmount = 1;
        foregroundImage.sprite = timerSprite;
        Destroy(roundDisplay);
        Destroy(textDisplay.gameObject);
        leaveDisplay.SetActive(true);
        button.interactable = true;
        StopAllCoroutines();
    }

    void LeaveScene()
    {
        SceneManager.LoadMainMenu();
    }

    void EndTurn()
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
