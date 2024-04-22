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
    private bool didSomeoneIncreaseInLastRound;
    private bool didSomeoneBetBeforeLastRound;

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
        GameplayManager.UpdatedRound += TryShowNext;
    }

    private void OnDisable()
    {
        GameplayManager.GameEnded -= Disable;
        GameplayManager.UpdatedBet -= OpponentAcceptedBet;
        GameplayManager.UpdatedRound -= TryShowNext;
    }

    private void TryShowNext()
    {
        if (GameplayManager.Instance.IsLastRound)
        {
            ShowNextRoundBet();
        }
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
        else
        {
            didSomeoneBetBeforeLastRound = true;
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
        else
        {
            didSomeoneBetBeforeLastRound = true;
        }
        AudioManager.Instance.PlaySoundEffect(AudioManager.DOUBLE_INITIATED);
        didOpponentInitBetIncrease = true;
        OnPointerDown(null);
        GameplayManager.UpdatedGameState += ManageRoundEnded;
        ShowNextRoundBet();
        GameplayUI.Instance.ShakeScreen(1);
        if (GameplayManager.Instance.IsLastRound && GameplayManager.IsPvpGame)
        {
            if (didIBet && !didSomeoneBetBeforeLastRound)
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
        AudioManager.Instance.PlaySoundEffect(AudioManager.DOUBLE_RESOLVED);
        pulsingLight.gameObject.SetActive(false);
        GameplayManager.Instance.OpponentAcceptedBet();
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
        int _currentBet = GameplayManager.Instance.CurrentBet;
        _currentBet *= 2;

        if (GameplayManager.Instance.IsLastRound && GameplayManager.Instance.GameplayState == GameplayState.ResolvingEndOfRound)
        {
            nextBetDisplay.text = string.Empty;
            return;
        }

        if (GameplayManager.IsPvpGame && didSomeoneIncreaseInLastRound)
        {
            _currentBet *= 2;
        }

        if (_currentBet>maxBet)
        {
            _currentBet = maxBet;
        }
        
        nextBetDisplay.text = "Next: " + _currentBet;
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
