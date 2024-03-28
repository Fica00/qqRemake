using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BetClickHandler : MonoBehaviour
{
    public static BetClickHandler Instance;
    [SerializeField] private TextMeshProUGUI betDisplay;
    [SerializeField] private TextMeshProUGUI nextBetDisplay;

    private Button button;
    private int maxBet = 8;
    private bool didOpponentInitBetIncrease;
    private bool didIBet;

    public bool DidIBetThisRound { get; private set; }

    public bool IsMaxBet => GameplayManager.Instance.CurrentBet == maxBet;

    private void Awake()
    {
        button = GetComponent<Button>();
        Instance = this;
    }

    private void OnEnable()
    {
        GameplayManager.GameEnded += Disable;
        GameplayManager.UpdatedBet += ShowBet;
        button.onClick.AddListener(IncreaseBet);
    }

    private void OnDisable()
    {
        GameplayManager.GameEnded -= Disable;
        GameplayManager.UpdatedBet -= ShowBet;
        button.onClick.RemoveListener(IncreaseBet);
    }
    
    private void Disable(GameResult _result)
    {
        button.interactable = false;
    }
    
    private void IncreaseBet()
    {
        if (!(GameplayManager.Instance.GameplayState==GameplayState.Waiting || GameplayManager.Instance.GameplayState==GameplayState.Playing))
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
        DidIBetThisRound = true;
        GameplayManager.UpdatedRound += TurnOffDidIBetThisRound;
    }

    private void TurnOffDidIBetThisRound()
    {
        GameplayManager.UpdatedRound += TurnOffDidIBetThisRound;
        DidIBetThisRound = false;
    }

    private void Start()
    {
        ShowBet();
        nextBetDisplay.text = string.Empty;
    }

    private void ShowBet()
    {
        int _betAmount = GameplayManager.Instance.CurrentBet;
        betDisplay.text = _betAmount < 10 ? "0 " + _betAmount : "1" + (_betAmount - 10);
        nextBetDisplay.text = string.Empty;
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
        DidIBetThisRound = false;
        GameplayManager.Instance.OpponentAcceptedBet();
    }

    private void ShowNextRoundBet()
    {
        int _currentBet = GameplayManager.Instance.CurrentBet;
        nextBetDisplay.text = _currentBet == maxBet ? "MAX" : "Next: " + _currentBet * 2;
    }
}
