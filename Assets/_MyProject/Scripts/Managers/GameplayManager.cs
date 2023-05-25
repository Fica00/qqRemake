using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;
    public static Action UpdatedRound;
    public GameplayPlayer myPlayer;
    [field: SerializeField] public int MaxAmountOfCardsInHand { get; private set; }

    [SerializeField] int maxRounds = 6;
    [SerializeField] List<LaneDisplay> lanes;

    GameplayState gameplayState;
    int currentRound;

    public GameplayState GameplayState => gameplayState;


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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrentRound = 0;
        SetupPlayers();
        InitialDraw();
        StartCoroutine(GameplayRoutine());
    }

    void SetupPlayers()
    {
        myPlayer.Setup();
    }

    void InitialDraw()
    {
        int _startingAmountOfCards = 3;
        Draw(myPlayer);
        //todo let bot also draw

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
            Debug.Log($"Player(isMy:{_player.IsMy}) has max amount of cards");
            return;
        }

        CardObject _drawnCard = _player.DrawCard();
        _player.AddCardToHand(_drawnCard);
    }

    IEnumerator GameplayRoutine()
    {
        yield return new WaitForSeconds(1); //wait for cards in hand to get to position
        while (CurrentRound <= maxRounds)
        {
            gameplayState = GameplayState.ResolvingBeginingOfRound;
            CurrentRound++;
            yield return new WaitForSeconds(1f); //duration of round animation
            yield return RevealLocation();
            yield return StartCoroutine(CheckForCardsThatShouldMoveToHand());
            DrawCard();

            gameplayState = GameplayState.Playing;
            //wait until both players finished their turns
            break;
        }

    }

    void DrawCard()
    {
        myPlayer.DrawCard();
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

    IEnumerator CheckForCardsThatShouldMoveToHand()
    {
        bool _finished = false;
        myPlayer.CheckForCardsThatShouldMoveToHand(Finished);
        yield return new WaitUntil(() => _finished);

        void Finished()
        {
            _finished = true;
        }
    }
}
