using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;
    public static Action UpdatedRound;
    public static Action UpdatedGameState;
    public static Action<GameResult> GameEnded;
    public static Action<int, Color, int> FlashPlace;
    public static Action<LaneLocation, bool, Color, int> FlashWholePlace;
    public static Action<LaneLocation, bool, Color> HighlihtWholePlace;
    public static Action<LaneLocation, bool, Color> HideHighlihtWholePlace;
    public GameplayPlayer MyPlayer;
    public GameplayPlayer BotPlayer;
    [field: SerializeField] public int MaxAmountOfCardsInHand { get; private set; }
    [field: SerializeField] public int DurationOfRound { get; private set; }
    [field: SerializeField] public TableHandler TableHandler { get; private set; }

    public CommandsHandler CommandsHandler;

    [SerializeField] protected EndTurnHandler endTurnHandler;
    [SerializeField] protected int maxRounds = 6;
    [SerializeField] protected List<LaneDisplay> lanes;
    [SerializeField] protected GameObject[] flags;

    GameplayState gameplayState;
    int currentRound;

    protected bool opponentFinished;
    protected bool iFinished;
    protected bool resolvedEndOfTheRound;
    protected int startingAmountOfCards = 3;

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

    protected void OnEnable()
    {
        EndTurnHandler.OnEndTurn += EndTurn;
        FlagClickHandler.OnClick += Forfiet;
    }

    protected void OnDisable()
    {
        CommandsHandler.Close();
        EndTurnHandler.OnEndTurn -= EndTurn;
        FlagClickHandler.OnClick -= Forfiet;
    }

    protected void EndTurn()
    {
        GameplayState = GameplayState.Waiting;
        iFinished = true;
    }

    protected void Forfiet()
    {
        UIManager.Instance.YesNoDialog.OnYesPressed.AddListener(YesForfiet);
        UIManager.Instance.YesNoDialog.Setup("Do you want to forfeit the match?");
    }

    protected void YesForfiet()
    {
        StopAllCoroutines();
        GameEnded?.Invoke(GameResult.ILost);
    }

    protected virtual void Awake()
    {
        Instance = this;
    }

    protected void Start()
    {
        CommandsHandler = new CommandsHandler();
        CommandsHandler.Setup();
        CurrentRound = 0;
        SetupPlayers();
        TableHandler.Setup();
        InitialDraw();
        StartCoroutine(GameplayRoutine());
    }

    protected virtual void SetupPlayers()
    {
        MyPlayer.Setup();
        BotPlayer.Setup();
    }

    protected virtual void InitialDraw()
    {
        InitialDraw(MyPlayer, startingAmountOfCards);
        InitialDraw(BotPlayer, startingAmountOfCards);
    }

    protected void InitialDraw(GameplayPlayer _player, int _startingAmountOfCards)
    {
        int _amountOfCardsInHand = _player.AmountOfCardsInHand;
        for (int i = _amountOfCardsInHand; i < _startingAmountOfCards; i++)
        {
            DrawCard(_player);
        }
    }

    public virtual void DrawCard()
    {
        DrawCard(MyPlayer);
        DrawCard(BotPlayer);
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

    IEnumerator GameplayRoutine()
    {
        yield return new WaitForSeconds(1); //wait for cards in hand to get to position
        while (CurrentRound < maxRounds)
        {
            opponentFinished = false;
            iFinished = false;
            resolvedEndOfTheRound = false;
            GameplayState = GameplayState.ResolvingBeginingOfRound;
            CurrentRound++;
            yield return new WaitForSeconds(1f); //duration of round animation
            yield return RevealLocation();
            yield return StartCoroutine(CheckForCardsThatShouldMoveToHand(MyPlayer));
            yield return StartCoroutine(CheckForCardsThatShouldMoveToHand(BotPlayer));
            DrawCard(MyPlayer);
            DrawCard(BotPlayer);

            GameplayState = GameplayState.Playing;
            yield return new WaitUntil(() => iFinished && opponentFinished);

            GameplayState = GameplayState.ResolvingEndOfRound;
            StartCoroutine(RevealCards());
            yield return new WaitUntil(() => resolvedEndOfTheRound);
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(2);//wait for a bit, allow user to also see the result
        GameResult _result = TableHandler.CalculateWinner();
        GameEnded?.Invoke(_result);

    }

    IEnumerator RevealLocation()
    {
        bool _canContinue = false;

        if (currentRound <= 3)
        {
            LaneAbility _laneAbility = LaneAbilityManager.Instance.GetLaneAbility();
            int _laneIndex = currentRound - 1;
            _laneAbility.Setup(lanes[_laneIndex]);
            lanes[_laneIndex].AbilityDisplay.Reveal(_laneAbility.Description, Reveald);
            _canContinue = false;
        }
        else
        {
            _canContinue = true;
        }

        yield return new WaitUntil(() => _canContinue);
        yield return new WaitForSeconds(0.5f); //add small delay

        void Reveald()
        {
            _canContinue = true;
        }
    }

    IEnumerator CheckForCardsThatShouldMoveToHand(GameplayPlayer _player)
    {
        bool _finished = false;
        _player.CheckForCardsThatShouldMoveToHand(Finished);
        yield return new WaitUntil(() => _finished);

        void Finished()
        {
            _finished = true;
        }
    }

    IEnumerator RevealCards()
    {
        int _whoPlaysFirst = TableHandler.WhichCardsToRevealFrist();
        ShowFlag(_whoPlaysFirst);
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
            flags[1].SetActive(false);
        }
        else
        {
            flags[0].SetActive(false);
            flags[1].SetActive(true);
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

    public void OpponentFinished()
    {
        opponentFinished = true;
    }

    public void UpdateQommonCosts(int _amount)
    {
        MyPlayer.UpdateQommonCost(_amount);
        BotPlayer.UpdateQommonCost(_amount);
    }

    public void FlashLocation(int _locationId, Color _color, int _amount)
    {
        FlashPlace?.Invoke(_locationId, _color, _amount);
    }

    public void FlashWholeLocation(LaneLocation _location, bool _mySide, Color _color, int _amount)
    {
        FlashWholePlace?.Invoke(_location, _mySide, _color, _amount);
    }

    public void HighlihtWholeLocation(LaneLocation _location, bool _mySide, Color _color)
    {
        HighlihtWholePlace?.Invoke(_location, _mySide, _color);
    }

    public void HideHighlihtWholeLocation(LaneLocation _location, bool _mySide, Color _color)
    {
        HideHighlihtWholePlace?.Invoke(_location, _mySide, _color);
    }
}
