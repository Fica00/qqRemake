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
                    return AutoBetStatus switch
                    {
                        AutoBetStatus.NonInitialized => 1,
                        AutoBetStatus.Initialized => 1,
                        AutoBetStatus.Accepted => 2,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                case BetStatus.FirstIncreaseRequest:
                    return AutoBetStatus switch
                    {
                        AutoBetStatus.NonInitialized => 1,
                        AutoBetStatus.Initialized => 1,
                        AutoBetStatus.Accepted => 2,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                case BetStatus.FirstIncreaseAccepted:
                    return AutoBetStatus switch
                    {
                        AutoBetStatus.NonInitialized => 2,
                        AutoBetStatus.Initialized => 2,
                        AutoBetStatus.Accepted => 4,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                case BetStatus.SecondIncreaseRequest:
                    return AutoBetStatus switch
                    {
                        AutoBetStatus.NonInitialized => 2,
                        AutoBetStatus.Initialized => 2,
                        AutoBetStatus.Accepted => 4,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                case BetStatus.SecondIncreaseAccepted:
                    return AutoBetStatus switch
                    {
                        AutoBetStatus.NonInitialized => 4,
                        AutoBetStatus.Initialized => 4,
                        AutoBetStatus.Accepted => 8,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void Awake()
    {        
        button = GetComponent<Button>();
        Instance = this;
        
        if (ModeHandler.ModeStatic == GameMode.Friendly)
        {
            holder.SetActive(false);
        }
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

        if (!SceneManager.IsGameplayTutorialScene & !DataManager.Instance.PlayerData.CanLoseRankPoints)
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

        if (!SceneManager.IsGameplayTutorialScene && !DataManager.Instance.PlayerData.CanLoseRankPoints)
        {
            DialogsManager.Instance.OkDialog.Setup("Doubling will be available after rank 10.");
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
    }
    
    private void ManageRoundEnded()
    {
        if (!didOpponentInitBetIncrease)
        {
            return;
        }
        
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
                return AutoBetStatus switch
                {
                    AutoBetStatus.NonInitialized => string.Empty,
                    AutoBetStatus.Initialized => "Next 2",
                    AutoBetStatus.Accepted => string.Empty,
                    _ => throw new ArgumentOutOfRangeException()
                };
            case BetStatus.FirstIncreaseRequest:
                return AutoBetStatus switch
                {
                    AutoBetStatus.NonInitialized => "Next 2",
                    AutoBetStatus.Initialized => "Next 4",
                    AutoBetStatus.Accepted => string.Empty,
                    _ => throw new ArgumentOutOfRangeException()
                };
            case BetStatus.FirstIncreaseAccepted:
                return AutoBetStatus switch
                {
                    AutoBetStatus.NonInitialized => string.Empty,
                    AutoBetStatus.Initialized => "Next 4",
                    AutoBetStatus.Accepted => string.Empty,
                    _ => throw new ArgumentOutOfRangeException()
                };
            case BetStatus.SecondIncreaseRequest:
                return AutoBetStatus switch
                {
                    AutoBetStatus.NonInitialized => "Next 4",
                    AutoBetStatus.Initialized => "Next 8",
                    AutoBetStatus.Accepted => string.Empty,
                    _ => throw new ArgumentOutOfRangeException()
                };
            case BetStatus.SecondIncreaseAccepted:
                return AutoBetStatus switch
                {
                    AutoBetStatus.NonInitialized => string.Empty,
                    AutoBetStatus.Initialized => "Next 8",
                    AutoBetStatus.Accepted => string.Empty,
                    _ => throw new ArgumentOutOfRangeException()
                };
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void AcceptAutoBet()
    {
        AudioManager.Instance.PlaySoundEffect(AudioManager.DOUBLE_RESOLVED);
        pulsingLight.gameObject.SetActive(false);
        IncreaseAutoBetStatus();
        stakeAnimator.SetTrigger(STAKE_KEY);
        holder.transform.DOScale(Vector3.one, 1);
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
        stakeAnimator.SetTrigger(STAKE_KEY);
        holder.transform.DOScale(Vector3.one, 1);
        IncreaseBetStatus();
    }
}
