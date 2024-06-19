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
    private bool didOpponentInitBetIncrease;
    private bool didIBet;
    private bool didSomeoneIncreaseInLastRound;
    private bool didDoubledForLastStep;

    public bool DidIBetThisRound { get; private set; }

    private void Awake()
    {
        button = GetComponent<Button>();
        Instance = this;
    }

    private void OnEnable()
    {
        GameplayManager.GameEnded += Disable;
        GameplayManager.UpdatedRound += TryShowNext;
    }

    private void OnDisable()
    {
        GameplayManager.GameEnded -= Disable;
        GameplayManager.UpdatedRound -= TryShowNext;
    }

    private void TryShowNext()
    {
        if (!GameplayManager.Instance.IsLastRound)
        {
            return;
        }
        
        ShowNextRoundBet();
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

        if (GameplayManager.Instance.IsLastRound)
        {
            didSomeoneIncreaseInLastRound = true;
        }
        
        GameplayManager.Instance.Bet();
        didIBet = true;
        DidIBetThisRound = true;
        GameplayManager.UpdatedRound += TurnOffDidIBetThisRound;
        GameplayUI.Instance.ShakeScreen(1);
        pulsingLight.gameObject.SetActive(true);
        ShowNextRoundBet();
        GameplayManager.UpdatedBet += OpponentAcceptedBet;
    }

    private void TurnOffDidIBetThisRound()
    {
        GameplayManager.UpdatedRound -= TurnOffDidIBetThisRound;
        DidIBetThisRound = false;
    }

    private void Start()
    {
        ShowBet();
        nextBetDisplay.text = string.Empty;
    }

    private void OpponentAcceptedBet()
    {
        GameplayManager.UpdatedBet -= OpponentAcceptedBet;
        stakeAnimator.SetTrigger(STAKE_KEY);
        holder.transform.DOScale(Vector3.one, 1);
        ShowBet();
    }

    private void ShowBet()
    {
        int _betAmount = GameplayManager.Instance.CurrentBet;
        betDisplayAnimation.text=betDisplay.text = _betAmount.ToString();
        nextBetDisplay.text = string.Empty;
        if (GameplayManager.Instance.IsLastRound)
        {
            ShowNextRoundBet();
        }
    }

    public void ShowOpponentWantsToIncreaseBet()
    {
        if (GameplayManager.Instance.IsLastRound)
        {
            didSomeoneIncreaseInLastRound = true;
        }

        AudioManager.Instance.PlaySoundEffect(AudioManager.DOUBLE_INITIATED);
        didOpponentInitBetIncrease = true;
        OnPointerDown(null);
        GameplayManager.UpdatedGameState += ManageRoundEnded;
        ShowNextRoundBet();
        GameplayUI.Instance.ShakeScreen(1);
        if (GameplayManager.Instance.IsLastRound && GameplayManager.IsPvpGame)
        {
            if (didIBet)
            {
                AcceptBet();
            }
        }
    }

    private void ManageRoundEnded()
    {
        if (GameplayManager.Instance.GameplayState==GameplayState.ResolvingEndOfRound)
        {
            AcceptBet();
        }
    }

    public void AcceptAutoBet()
    {
        GameplayManager.UpdatedBet += OpponentAcceptedBet;
        AudioManager.Instance.PlaySoundEffect(AudioManager.DOUBLE_RESOLVED);
        pulsingLight.gameObject.SetActive(false);
    }

    private void AcceptBet()
    {
        AudioManager.Instance.PlaySoundEffect(AudioManager.DOUBLE_RESOLVED);
        GameplayManager.UpdatedGameState -= ManageRoundEnded;
        didOpponentInitBetIncrease = false;
        DidIBetThisRound = false;
        pulsingLight.gameObject.SetActive(false);
        holder.transform.DOScale(Vector3.one,1);
        stakeAnimator.SetTrigger(STAKE_KEY);
        GameplayManager.Instance.OpponentAcceptedBet();

        if (GameplayManager.Instance.IsLastRound && GameplayManager.IsPvpGame)
        {
            if (!didIBet)
            {
                IncreaseBet();
            }
        }
    }

    private void ShowNextRoundBet()
    {
        int _currentBetAmount = GetInitialBetAmount();
        
        if (ShouldClearNextBetDisplay())
        {
            nextBetDisplay.text = string.Empty;
            return;
        }
        _currentBetAmount = AdjustBetAmount(_currentBetAmount);

        nextBetDisplay.text = "Next: " + _currentBetAmount;

    }

    private int AdjustBetAmount(int _betAmount)
    {
        if (didSomeoneIncreaseInLastRound && !didDoubledForLastStep)
        {
           
            _betAmount *= 2;
            didDoubledForLastStep=true;
        }
        if(didDoubledForLastStep &&(_betAmount<4))
        {
            _betAmount *= 2;
        }

        return _betAmount;
    }

    private int GetInitialBetAmount()
    {
      return  GameplayManager.Instance.CurrentBet * 2;
    }

    private static bool ShouldClearNextBetDisplay()
    {
        return GameplayManager.Instance.IsLastRound && GameplayManager.Instance.GameplayState == GameplayState.ResolvingEndOfRound;
    }

    public void OnPointerDown(PointerEventData _eventData)
    {
        if (!(GameplayManager.Instance.GameplayState==GameplayState.Waiting || GameplayManager.Instance.GameplayState==GameplayState.Playing))
        {
            return;
        }

        if (didIBet)
        {
            return;
        }

        if (holder.transform.localScale.x!=1)
        {
            holder.transform.DOScale(Vector3.one*.8f, 1);
            return;
        }

        AudioManager.Instance.PlaySoundEffect(AudioManager.DOUBLE_INITIATED);
        holder.transform.DOScale(Vector3.one*.8f, 1);
    }

    public void OnPointerUp(PointerEventData _eventData)
    {
        IncreaseBet();
    }
}