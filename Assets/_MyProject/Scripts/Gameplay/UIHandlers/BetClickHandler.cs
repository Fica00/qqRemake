using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BetClickHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private const string STAKE_KEY = "Stake";
    public static BetClickHandler Instance;
    [SerializeField] private GameObject holder;
    [SerializeField] private GameObject pulsingLight;
    [SerializeField] private TextMeshProUGUI betDisplay;
    [SerializeField] private TextMeshProUGUI betDisplayAnimation;
    [SerializeField] private TextMeshProUGUI nextBetDisplay;
    [SerializeField] private Animator stakeAnimator;

    private Button button;
    private int maxBet = 8;
    private bool didOpponentInitBetIncrease;
    private bool didIBet;

    public bool DidIBetThisRound { get; private set; }


    private void Awake()
    {
        button = GetComponent<Button>();
        Instance = this;
    }

    private void OnEnable()
    {
        GameplayManager.GameEnded += Disable;
        GameplayManager.UpdatedBet += OpponentAcceptedBet;
    }

    private void OnDisable()
    {
        GameplayManager.GameEnded -= Disable;
        GameplayManager.UpdatedBet -= OpponentAcceptedBet;
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

        GameplayManager.Instance.Bet();
        didIBet = true;
        DidIBetThisRound = true;
        GameplayManager.UpdatedRound += TurnOffDidIBetThisRound;
        GameplayUI.Instance.ShakeScreen(1);
        pulsingLight.gameObject.SetActive(true);
        ShowNextRoundBet();
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

    private void OpponentAcceptedBet()
    {
        stakeAnimator.SetTrigger(STAKE_KEY);
        ShowBet();
    }

    private void ShowBet()
    {
        int _betAmount = GameplayManager.Instance.CurrentBet;
        betDisplayAnimation.text=betDisplay.text = _betAmount.ToString();
        nextBetDisplay.text = string.Empty;
    }

    public void ShowOpponentWantsToIncreaseBet()
    {
        didOpponentInitBetIncrease = true;
        OnPointerDown(null);
        GameplayManager.UpdatedGameState += ManageRoundEnded;
        ShowNextRoundBet();
        GameplayUI.Instance.ShakeScreen(1);
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
        pulsingLight.gameObject.SetActive(false);
        holder.transform.DOScale(Vector3.one,1);
        stakeAnimator.SetTrigger(STAKE_KEY);
        GameplayManager.Instance.OpponentAcceptedBet();
    }

    private void ShowNextRoundBet()
    {
        int _currentBet = GameplayManager.Instance.CurrentBet;
        nextBetDisplay.text = _currentBet == maxBet ? "MAX" : "Next: " + _currentBet * 2;
    }

    public void OnPointerDown(PointerEventData _eventData)
    {
        if (!(GameplayManager.Instance.GameplayState==GameplayState.Waiting || GameplayManager.Instance.GameplayState==GameplayState.Playing))
        {
            return;
        }
        holder.transform.DOScale(Vector3.one*.8f, 1);
    }

    public void OnPointerUp(PointerEventData _eventData)
    {
        IncreaseBet();
    }

    public void OfferBet()
    {
        didOpponentInitBetIncrease = true;
        OnPointerDown(null);
        ShowNextRoundBet();
        GameplayManager.Instance.Bet();
        GameplayManager.UpdatedRound += TurnOffDidIBetThisRound;
        GameplayUI.Instance.ShakeScreen(1);
        pulsingLight.gameObject.SetActive(true);
    }
}
