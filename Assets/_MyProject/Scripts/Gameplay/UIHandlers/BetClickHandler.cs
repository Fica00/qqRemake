using System;
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
    public bool DidIBetThisRound { get; private set; }
    public BetStatus BetStatus { get; private set; } = BetStatus.DefaultBet;
    public AutoBetStatus AutoBetStatus { get; private set; } = AutoBetStatus.NonInitialized;

    public int CurrentBet
    {
        get
        {
            switch (BetStatus)
            {
                case BetStatus.DefaultBet:
                    return 1;
                case BetStatus.FirstIncreaseRequest:
                    return 1;
                case BetStatus.FirstIncreaseAccepted:
                    return 2;
                case BetStatus.SecondIncreaseRequest:
                    return 2;
                case BetStatus.SecondIncreaseAccepted:
                    return 4;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void Awake()
    {
        if (ModeHandler.ModeStatic == GameMode.Friendly)
        {
            holder.SetActive(false);
            return;
        }
        
        button = GetComponent<Button>();
        Instance = this;
    }

    private void OnEnable()
    {
        GameplayManager.GameEnded += Disable;
        GameplayManager.UpdatedRound += TryAutoBet;
    }

    private void OnDisable()
    {
        GameplayManager.GameEnded -= Disable;
        GameplayManager.UpdatedRound -= TryAutoBet;
    }

    private void Disable(GameResult _result)
    {
        button.interactable = false;
    }
    
    private void TryAutoBet()
    {
        if (!GameplayManager.Instance.IsLastRound)
        {
            return;
        }

        if (ModeHandler.ModeStatic==GameMode.Friendly)
        {
            return;
        }
        
        IncreaseAutoBetStatus();
    }

    public void OnPointerUp(PointerEventData _eventData)
    {
        InitIncreaseBet();
    }
    
    private void InitIncreaseBet()
    {
        if (GameplayManager.Instance.GameplayState is not (GameplayState.Waiting or GameplayState.Playing))
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
        IncreaseBetStatus();
    }

    private void TurnOffDidIBetThisRound()
    {
        GameplayManager.UpdatedRound -= TurnOffDidIBetThisRound;
        DidIBetThisRound = false;
    }
    
    public void ShowOpponentWantsToIncreaseBet()
    {
        AudioManager.Instance.PlaySoundEffect(AudioManager.DOUBLE_INITIATED);
        didOpponentInitBetIncrease = true;
        OnPointerDown(null);
        GameplayManager.UpdatedGameState += ManageRoundEnded;
        GameplayUI.Instance.ShakeScreen(1);
        IncreaseBetStatus();
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
        IncreaseBetStatus();
    }
    
    private void ManageRoundEnded()
    {
        if (GameplayManager.Instance.GameplayState==GameplayState.ResolvingEndOfRound)
        {
            AcceptBet();
        }
    }
    
    private void IncreaseBetStatus()
    {
        BetStatus = (BetStatus)((int)BetStatus + 1);
        ShowBet();
    }
    
    private void Start()
    {
        ShowBet();
    }
    
    private void ShowBet()
    {
        betDisplay.text = CurrentBet.ToString();
        betDisplayAnimation.text = CurrentBet.ToString();
        nextBetDisplay.text = GetNextBetText();
    }

    private string GetNextBetText()
    {
        switch (BetStatus)
        {
            case BetStatus.DefaultBet:
                return string.Empty;
            case BetStatus.FirstIncreaseRequest:
                return "Next 2";
            case BetStatus.FirstIncreaseAccepted:
                return string.Empty;
            case BetStatus.SecondIncreaseRequest:
                return "Next 4";
            case BetStatus.SecondIncreaseAccepted:
                return string.Empty;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void AcceptAutoBet()
    {
        AudioManager.Instance.PlaySoundEffect(AudioManager.DOUBLE_RESOLVED);
        pulsingLight.gameObject.SetActive(false);
        IncreaseAutoBetStatus();
    }
    
    private void IncreaseAutoBetStatus()
    {
        AutoBetStatus = (AutoBetStatus)((int)AutoBetStatus + 1);
        ShowBet();
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

    public void OpponentAcceptedBet()
    {
        IncreaseBetStatus();
    }
}
