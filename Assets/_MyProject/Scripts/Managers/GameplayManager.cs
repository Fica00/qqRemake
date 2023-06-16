using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;
    public static bool IsPVPGame=false;
    public static Action UpdatedRound;
    public static Action UpdatedGameState;
    public static Action UpdatedBet;
    public static Action<GameResult> GameEnded;
    public static Action<int, Color, int> OnFlashPlace;
    public static Action<LaneLocation, bool, Color, int> OnFlashWholePlace;
    public static Action<LaneLocation, bool, Color> OnHighlihtWholePlace;
    public static Action<LaneLocation, bool, Color,int> OnFlashAllSpotsOnLocation;
    public static Action<LaneLocation, bool, Color> OnHideHighlihtWholePlace;
    public GameplayPlayer MyPlayer;
    public GameplayPlayer OpponentPlayer;

    [field: SerializeField] public int MaxAmountOfCardsInHand { get; private set; }
    [field: SerializeField] public int DurationOfRound { get; private set; }
    [field: SerializeField] public TableHandler TableHandler { get; private set; }
    [field: SerializeField] public PlayerDisplay MyPlayerDisplay { get; private set; }
    [field: SerializeField] public PlayerDisplay OpponentPlayerDisplay { get; private set; }

    public CommandsHandler CommandsHandler = new CommandsHandler();

    [SerializeField] protected EndTurnHandler endTurnHandler;
    [SerializeField] protected int maxRounds = 6;
    [SerializeField] protected List<LaneDisplay> lanes;
    [SerializeField] protected GameObject[] flags;
    [SerializeField] protected GameObject[] playsFirstDisplays;

    GameplayState gameplayState;
    int currentRound;

    protected bool opponentFinished;
    protected bool iFinished;
    protected bool resolvedEndOfTheRound;
    protected int startingAmountOfCards = 3;
    protected int currentBet = 1;
    protected List<int> excludeLaneAbilities = new List<int>();
    protected bool locationRevealed;

    public GameplayState GameplayState
    {
        get
        {
            return gameplayState;
        }
        set
        {
            gameplayState = value;
            UpdatedGameState?.Invoke();
        }
    }

    public int CurrentRound
    {
        get
        {
            return currentRound;
        }
        set
        {
            currentRound = value;
            UpdatedRound?.Invoke();
        }
    }

    public List<LaneDisplay> Lanes => lanes;

    public int CurrentBet => currentBet;

    public int MaxAmountOfRounds => maxRounds;

    protected virtual void OnEnable()
    {
        EndTurnHandler.OnEndTurn += EndTurn;
        FlagClickHandler.OnForefiet += Forfiet;
    }

    protected virtual void OnDisable()
    {
        CommandsHandler.Close();
        EndTurnHandler.OnEndTurn -= EndTurn;
        FlagClickHandler.OnForefiet -= Forfiet;
    }

    protected virtual void EndTurn()
    {
        GameplayState = GameplayState.Waiting;
        iFinished = true;
    }

    protected virtual void Forfiet()
    {
        StopAllCoroutines();
        GameEnded?.Invoke(GameResult.ILost);
    }

    protected virtual void Awake()
    {
        Instance = this;
        IsPVPGame = false;
    }

    protected virtual void Start()
    {
        CommandsHandler.Setup();
        CurrentRound = 0;
        SetupPlayers();
        TableHandler.Setup();
        StartCoroutine(GameplayRoutine());

    }

    protected virtual void SetupPlayers()
    {
        MyPlayer.Setup();
        OpponentPlayer.Setup();
    }

    protected virtual IEnumerator InitialDraw()
    {
       yield return StartCoroutine(InitialDraw(MyPlayer, startingAmountOfCards));
       yield return StartCoroutine(InitialDraw(OpponentPlayer, startingAmountOfCards));
    }

    protected IEnumerator InitialDraw(GameplayPlayer _player, int _startingAmountOfCards)
    {
        yield return StartCoroutine(CheckForCardsThatShouldMoveToHand(_player));

        int _amountOfCardsInHand = _player.AmountOfCardsInHand;
        for (int i = _amountOfCardsInHand; i < _startingAmountOfCards; i++)
        {
            DrawCard(_player);
        }
    }

    public virtual void DrawCard()
    {
        DrawCard(MyPlayer);
        DrawCard(OpponentPlayer);
    }

    protected void DrawCard(GameplayPlayer _player)
    {
        int _amountOfCardsInHand = _player.AmountOfCardsInHand;
        if (_amountOfCardsInHand >= MaxAmountOfCardsInHand)
        {
            return;
        }

        CardObject _drawnCard = _player.DrawCard();
        _player.AddCardToHand(_drawnCard);
    }

    protected IEnumerator GameplayRoutine()
    {
        yield return new WaitUntil(ReadyToStart);
        yield return StartCoroutine(InitialDraw());
        yield return new WaitForSeconds(1); //wait for cards in hand to get to position
        while (CurrentRound < maxRounds)
        {
            int _whoPlaysFirst = TableHandler.WhichCardsToRevealFrist();
            ShowFlag(_whoPlaysFirst);
            opponentFinished = false;
            iFinished = false;
            resolvedEndOfTheRound = false;
            GameplayState = GameplayState.ResolvingBeginingOfRound;
            CurrentRound++;
            if (currentRound <= 3)
            {
                locationRevealed = false;
            }
            yield return new WaitForSeconds(1f); //duration of round animation
            StartCoroutine(RevealLocation());
            yield return new WaitUntil(()=> locationRevealed);
            yield return StartCoroutine(RoundCheckForCardsThatShouldMoveToHand());
            RoundDrawCard();

            GameplayState = GameplayState.Playing;
            yield return new WaitUntil(() => iFinished && opponentFinished);

            GameplayState = GameplayState.ResolvingEndOfRound;
            StartCoroutine(RevealCards(_whoPlaysFirst));
            yield return new WaitUntil(() => resolvedEndOfTheRound);
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(2);//wait for a bit, allow user to also see the result
        GameResult _result = TableHandler.CalculateWinner();
        GameEnded?.Invoke(_result);

    }

    protected virtual bool ReadyToStart()
    {
        return true;
    }

    protected virtual IEnumerator RoundCheckForCardsThatShouldMoveToHand()
    {
        yield return StartCoroutine(CheckForCardsThatShouldMoveToHand(MyPlayer));
        yield return StartCoroutine(CheckForCardsThatShouldMoveToHand(OpponentPlayer));
    }

    protected virtual void RoundDrawCard()
    {
        DrawCard(MyPlayer);
        DrawCard(OpponentPlayer);
    }

    protected virtual IEnumerator RevealLocation()
    {
        if (currentRound > 3)
        {
            yield break;
        }

        LaneAbility _laneAbility = GetLaneAbility();
        yield return RevealLocation(_laneAbility.Id);
    }

    protected virtual LaneAbility GetLaneAbility()
    {
        return LaneAbilityManager.Instance.GetLaneAbility(excludeLaneAbilities);
    }

    protected IEnumerator RevealLocation(int _abilityID)
    {
        bool _canContinue = false;
        LaneAbility _laneAbility = LaneAbilityManager.Instance.GetLaneAbility(_abilityID);
        excludeLaneAbilities.Add(_abilityID);
        int _laneIndex = currentRound - 1;
        _laneAbility.Setup(lanes[_laneIndex]);
        lanes[_laneIndex].AbilityDisplay.Reveal(_laneAbility.Description, Reveald);
        _canContinue = false;

        yield return new WaitUntil(() => _canContinue);
        yield return new WaitForSeconds(0.5f); //add small delay
        locationRevealed = true;

        void Reveald()
        {
            _canContinue = true;
        }
    }

    protected IEnumerator CheckForCardsThatShouldMoveToHand(GameplayPlayer _player)
    {
        bool _finished = false;
        _player.CheckForCardsThatShouldMoveToHand(Finished);
        yield return new WaitUntil(() => _finished);

        void Finished()
        {
            _finished = true;
        }
    }

    protected IEnumerator RevealCards(int _whoPlaysFirst)
    {
        foreach (var _command in CommandsHandler.OpponentCommands)
        {
            _command.Card.PrepareForReveal();
        }

        
        yield return StartCoroutine(TableHandler.RevealCards(_whoPlaysFirst == -1 ? CommandsHandler.MyCommands : CommandsHandler.OpponentCommands)); //show first set of cards
        yield return StartCoroutine(TableHandler.RevealCards(_whoPlaysFirst == -1 ? CommandsHandler.OpponentCommands : CommandsHandler.MyCommands)); // show secound set of cards

        yield return new WaitForSeconds(1);//some delay

        CommandsHandler.MyCommands.Clear();
        CommandsHandler.OpponentCommands.Clear();

        resolvedEndOfTheRound = true;
    }

    void ShowFlag(int _whoPlaysFirst)
    {
        if (_whoPlaysFirst == -1)
        {
            flags[0].SetActive(true);
            playsFirstDisplays[0].SetActive(true);
            flags[1].SetActive(false);
            playsFirstDisplays[1].SetActive(false);
        }
        else
        {
            flags[0].SetActive(false);
            playsFirstDisplays[0].SetActive(false);
            flags[1].SetActive(true);
            playsFirstDisplays[1].SetActive(true);
        }
    }

    public virtual void ReturnToWaitingState()
    {
        if (endTurnHandler.TimeLeft > 2)
        {
            GameplayState = GameplayState.Playing;
        }
        iFinished = false;
    }

    public virtual void Bet()
    {
        OpponentAcceptedBet();
    }

    public virtual void OpponentAcceptedBet()
    {
        currentBet *= 2;
        UpdatedBet?.Invoke();
    }

    public void OpponentFinished()
    {
        opponentFinished = true;
    }

    public virtual void UpdateQommonCosts(int _amount)
    {
        MyPlayer.UpdateQommonCost(_amount);
        OpponentPlayer.UpdateQommonCost(_amount);
    }

    public void FlashLocation(int _locationId, Color _color, int _amount)
    {
        OnFlashPlace?.Invoke(_locationId, _color, _amount);
    }

    public void FlashWholeLocation(LaneLocation _location, bool _mySide, Color _color, int _amount)
    {
        OnFlashWholePlace?.Invoke(_location, _mySide, _color, _amount);
    }

    public void HighlihtWholeLocation(LaneLocation _location, bool _mySide, Color _color)
    {
        OnHighlihtWholePlace?.Invoke(_location, _mySide, _color);
    } 
    
    public void FlashAllSpotsOnLocation(LaneLocation _location, bool _mySide, Color _color, int _amount)
    {
        OnFlashAllSpotsOnLocation?.Invoke(_location, _mySide, _color,_amount);
    }

    public void HideHighlihtWholeLocation(LaneLocation _location, bool _mySide, Color _color)
    {
        OnHideHighlihtWholePlace?.Invoke(_location, _mySide, _color);
    }
}
