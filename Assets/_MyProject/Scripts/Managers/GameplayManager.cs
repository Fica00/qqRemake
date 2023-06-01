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
    public GameplayPlayer MyPlayer;
    public GameplayPlayer BotPlayer;
    [field: SerializeField] public int MaxAmountOfCardsInHand { get; private set; }
    [field: SerializeField] public int DurationOfRound { get; private set; }
    [field: SerializeField] public TableHandler TableHandler { get; private set; }

    public CommandsHandler CommandsHandler;

    [SerializeField] EndTurnHandler endTurnHandler;
    [SerializeField] int maxRounds = 6;
    [SerializeField] List<LaneDisplay> lanes;
    [SerializeField] GameObject[] flags;

    GameplayState gameplayState;
    int currentRound;

    bool opponentFinished;
    bool iFinished;
    bool resolvedEndOfTheRound;

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

    private void OnEnable()
    {
        EndTurnHandler.OnEndTurn += EndTurn;
        FlagClickHandler.OnClick += Forfiet;
    }

    private void OnDisable()
    {
        CommandsHandler.Close();
        EndTurnHandler.OnEndTurn -= EndTurn;
        FlagClickHandler.OnClick -= Forfiet;
    }

    void EndTurn()
    {
        GameplayState = GameplayState.Waiting;
        iFinished = true;
    }

    void Forfiet()
    {
        UIManager.Instance.YesNoDialog.OnYesPressed.AddListener(YesForfiet);
        UIManager.Instance.YesNoDialog.Setup("Do you want to forfeit the match?");
    }

    void YesForfiet()
    {
        StopAllCoroutines();
        GameEnded?.Invoke(GameResult.ILost);
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CommandsHandler = new CommandsHandler();
        CommandsHandler.Setup();
        CurrentRound = 0;
        SetupPlayers();
        TableHandler.Setup();
        InitialDraw();
        StartCoroutine(GameplayRoutine());
    }

    void SetupPlayers()
    {
        MyPlayer.Setup();
        BotPlayer.Setup();
    }

    void InitialDraw()
    {
        int _startingAmountOfCards = 3;
        Draw(MyPlayer);
        Draw(BotPlayer);

        void Draw(GameplayPlayer _player)
        {
            int _amountOfCardsInHand = _player.AmountOfCardsInHand;
            for (int i = _amountOfCardsInHand; i < _startingAmountOfCards; i++)
            {
                DrawCard(_player);
            }
        }
    }

    void DrawCard(GameplayPlayer _player)
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

        GameResult _result = TableHandler.CalculateWinner();
        GameEnded?.Invoke(_result);

    }

    IEnumerator RevealLocation()
    {
        bool _canContinue = false;
        switch (currentRound)
        {
            case 1:
                //todo generate ability
                lanes[0].AbilityDisplay.Reveal("Ability 1", Reveald);
                break;
            case 2:
                //todo generate ability
                lanes[1].AbilityDisplay.Reveal("Ability 2", Reveald);
                break;
            case 3:
                //todo generate ability
                lanes[2].AbilityDisplay.Reveal("Ability 3", Reveald);
                break;
            default:
                _canContinue = true;
                break;
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

    public void ReturnToWaitingState()
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
}
