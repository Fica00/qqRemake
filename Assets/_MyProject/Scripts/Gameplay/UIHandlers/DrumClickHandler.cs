using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DrumClickHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI betDisplay;
    [SerializeField] TextMeshProUGUI nextBetDisplay;
    [SerializeField] GameObject soundWawe;

    Button button;
    int maxBet = 16;
    bool didOpponentInitBetIncrease;
    bool didIBet = false;

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

    void Disable(GameResult _result)
    {
        button.interactable = false;
        soundWawe.gameObject.SetActive(false);
    }

    private void Start()
    {
        betDisplay.text = "0 1";
        nextBetDisplay.text = string.Empty;
    }

    void ShowBet()
    {
        int _betAmount = GameplayManager.Instance.CurrentBet;
        betDisplay.text = _betAmount < 10 ? "0 " + _betAmount : "1" + (_betAmount - 10);
        nextBetDisplay.text = string.Empty;
    }

    void IncreaseBet()
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
        GameplayManager.Instance.MyPlayerDisplay.RemoveGlow();
        soundWawe.SetActive(false);
        didIBet = true;
    }

    public void ShowOpponentWantsToIncreaseBet()
    {
        didOpponentInitBetIncrease = true;
        GameplayManager.UpdatedGameState += ManageRoundEnded;
        ShowNextRoundBet();
    }

    void ManageRoundEnded()
    {
        if (GameplayManager.Instance.GameplayState==GameplayState.ResolvingEndOfRound)
        {
            AcceptBet();
        }
    }

    void AcceptBet()
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
