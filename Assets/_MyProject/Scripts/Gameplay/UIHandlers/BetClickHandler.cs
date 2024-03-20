using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BetClickHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI betDisplay;
    [SerializeField] private TextMeshProUGUI nextBetDisplay;

    private Button button;
    private int maxBet = 16;
    private bool didOpponentInitBetIncrease;
    private bool didIBet;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(IncreaseBet);
        GameplayManager.UpdatedBet += ShowBet;
        GameplayManager.GameEnded += Disable;
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(IncreaseBet);
        GameplayManager.UpdatedBet -= ShowBet;
        GameplayManager.GameEnded -= Disable;
    }

    private void Disable(GameResult _result)
    {
        button.interactable = false;
    }

    private void Start()
    {
        betDisplay.text = "0 1";
        nextBetDisplay.text = string.Empty;
    }

    private void ShowBet()
    {
        int _betAmount = GameplayManager.Instance.CurrentBet;
        betDisplay.text = _betAmount < 10 ? "0 " + _betAmount : "1" + (_betAmount - 10);
        nextBetDisplay.text = string.Empty;
    }

    private void IncreaseBet()
    {
        if (!(GameplayManager.Instance.GameplayState==GameplayState.Waiting||GameplayManager.Instance.GameplayState==GameplayState.Playing))
        {
            return;
        }
        
        if (didOpponentInitBetIncrease)
        {
            AcceptBet();
            return;
        }

        if (didIBet)
        {
            return;
        }

        ShowNextRoundBet();
        GameplayManager.Instance.Bet();
        didIBet = true;
    }

    public void ShowOpponentWantsToIncreaseBet()
    {
        didOpponentInitBetIncrease = true;
        GameplayManager.UpdatedGameState += ManageRoundEnded;
        ShowNextRoundBet();
    }

    private void ManageRoundEnded()
    {
        if (GameplayManager.Instance.GameplayState==GameplayState.ResolvingEndOfRound)
        {
            AcceptBet();
        }
    }

    private void AcceptBet()
    {
        GameplayManager.UpdatedGameState -= ManageRoundEnded;
        didOpponentInitBetIncrease = false;
        GameplayManager.Instance.OpponentAcceptedBet();
    }

    public void ShowNextRoundBet()
    {
        int _currentBet = GameplayManager.Instance.CurrentBet;
        nextBetDisplay.text = _currentBet == maxBet ? "MAX" : "Next: " + (_currentBet * 2);
    }
}
