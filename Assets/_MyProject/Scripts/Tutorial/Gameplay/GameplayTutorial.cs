using System.Collections;
using Tutorial;
using UnityEngine;

public class GameplayTutorial : GameplayManager
{
    public new static GameplayTutorial Instance;
    [SerializeField] private TutorialMessage manaDisplay;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    protected override IEnumerator GameplayRoutine()
    {
        yield return new WaitUntil(ReadyToStart);
        yield return StartCoroutine(InitialDraw());
        yield return new WaitForSeconds(1); //wait for cards in hand to get to position
        while (CurrentRound < maxRounds)
        {
            CommandsHandler.MyOriginalCommandsThisTurn.Clear();
            int _whoPlaysFirst = TableHandler.WhichCardsToRevealFrist();
            ShowFlag(_whoPlaysFirst);
            opponentFinished = false;
            iFinished = false;
            resolvedEndOfTheRound = false;
            GameplayState = GameplayState.ResolvingBeginingOfRound;
            CurrentRound++;
            if (CurrentRound <= 3)
            {
                locationRevealed = false;
            }
            yield return new WaitForSeconds(1f); //duration of round animation
            StartCoroutine(RevealLocation());
            StartCoroutine(ShowRevealText());
            yield return new WaitUntil(() => locationRevealed);
            if (CurrentRound!=1)
            {
                yield return StartCoroutine(RoundCheckForCardsThatShouldMoveToHand());
                if (!DrewCardDirectlyToHand||CurrentRound==1)
                {
                    RoundDrawCard();
                }
            }

            DrewCardDirectlyToHand = false;
            
            GameplayState = GameplayState.Playing;
            yield return new WaitUntil(() => iFinished && opponentFinished);

            GameplayState = GameplayState.ResolvingEndOfRound;
            var _addQommonOnEndOfTheTurn = FindObjectOfType<LaneAbilityOnTurnXAllPutCardHere>();
            if (_addQommonOnEndOfTheTurn)
            {
                if (_addQommonOnEndOfTheTurn.Round == CurrentRound)
                {
                    yield return new WaitForSeconds(2);
                }
            }
            StartCoroutine(RevealCards(_whoPlaysFirst));
            yield return new WaitUntil(() => resolvedEndOfTheRound);
            yield return new WaitForSeconds(2.5f);
        }

        bool _canContinue = false;
        for (int i = 0; i < Lanes.Count; i++)
        {
            Lanes[i].ShowWinner(Continue);
            yield return new WaitUntil(() => _canContinue);
            _canContinue = false;
        }
        yield return new WaitForSeconds(1);
        GameResult _result = TableHandler.CalculateWinner();
        GameEnded?.Invoke(_result);
        
        void Continue()
        {
            _canContinue = true;
        }
    }

    protected override IEnumerator InitialDraw()
    {
        yield return new WaitForSeconds(1);
        Debug.Log(1);
        CardObject _card = MyPlayer.GetCardFromDeck(1);
        MyPlayer.DrawCard(_card, true);
        MyPlayer.AddCardToHand(_card);

        _card = OpponentPlayer.GetCardFromDeck(1);
        OpponentPlayer.DrawCard(_card, true);
        OpponentPlayer.AddCardToHand(_card);
    }

    public void ShowMana()
    {
        StartCoroutine(base.InitialDraw());
        manaDisplay.Setup();
    }
}
