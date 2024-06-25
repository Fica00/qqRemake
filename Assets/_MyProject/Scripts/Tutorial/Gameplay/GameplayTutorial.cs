using System;
using System.Collections;
using Tutorial;
using UnityEngine;

public class GameplayTutorial : GameplayManager
{
    public new static GameplayTutorial Instance;

    public static Action<CardObject> OnDrawSecondTwoCards;
    [SerializeField] private TutorialMessage manaDisplay;
    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private GameObject opponentsEffect;
    
    public bool cardsPlayed;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    protected override IEnumerator GameplayRoutine()
    {
        yield return new WaitUntil(ReadyToStart);
        SetupTutorialLocation();
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
            if (CurrentRound == 1) { yield return new WaitUntil((() => tutorialManager.isAddsPowerAndHighestPowerPanelShowen));}
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
                    //RoundDrawCard();

                    if (CurrentRound == 2)
                    {
                        CardObject _card = MyPlayer.GetCardFromDeck(3); // Goldie
                        MyPlayer.DrawCard(_card, true);
                        MyPlayer.AddCardToHand(_card);
                    }
                    if (CurrentRound == 3)
                    {
                        CardObject _card1 = MyPlayer.GetCardFromDeck(8); //samu-kitsune 
                        MyPlayer.DrawCard(_card1, true);
                        MyPlayer.AddCardToHand(_card1);
                        
                        CardObject _card2 = MyPlayer.GetCardFromDeck(29); //Dun-dun
                        MyPlayer.DrawCard(_card2, true);
                        MyPlayer.AddCardToHand(_card2);

                        opponentsEffect.SetActive(false);
                    }
                    if (CurrentRound == 4)
                    {
                        CardObject _card = MyPlayer.GetCardFromDeck(7); //Mukong
                        MyPlayer.DrawCard(_card, true);
                        MyPlayer.AddCardToHand(_card);
                    }
                    if (CurrentRound == 5)
                    {
                        CardObject _card1 = MyPlayer.GetCardFromDeck(9); //Geisha-Ko
                        MyPlayer.DrawCard(_card1, true);
                        MyPlayer.AddCardToHand(_card1);
                        
                        CardObject _card2 = MyPlayer.GetCardFromDeck(21); //Arhcer Penny
                        MyPlayer.DrawCard(_card2, true);
                        MyPlayer.AddCardToHand(_card2);
                        
                        CardObject _card3 = MyPlayer.GetCardFromDeck(11); //Gunner Kaka

                        MyPlayer.DrawCard(_card3, true);
                        MyPlayer.AddCardToHand(_card3);
                    }
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

            cardsPlayed = true;
          //  OnFinishedGameplayLoop?.Invoke();
        }
        
        AcceptAutoBet();
        
        bool _playBackgroundMusic = DataManager.Instance.PlayerData.PlayBackgroundMusic;
        DataManager.Instance.PlayerData.PlayBackgroundMusic = false;

        bool _canContinue = false;
        for (int i = 0; i < Lanes.Count; i++)
        {
            Lanes[i].ShowWinner(Continue);
            yield return new WaitUntil(() => _canContinue);
            _canContinue = false;
        }
        yield return new WaitForSeconds(1);
        DataManager.Instance.PlayerData.PlayBackgroundMusic = _playBackgroundMusic;
        GameResult _result = TableHandler.CalculateWinner();
        GameEnded?.Invoke(_result);
        
        void Continue()
        {
            _canContinue = true;
        }
    }

    private void SetupTutorialLocation()  //Reci ne magicnim brojevima
    {
        DataManager.Instance.locationsPicked[0] = 6;
        DataManager.Instance.locationsPicked[1] = 22;
        DataManager.Instance.locationsPicked[2] = 2;
    }

    protected override IEnumerator InitialDraw()
    {
        yield return new WaitForSeconds(1);
        
        CardObject _card = MyPlayer.GetCardFromDeck(1);
        MyPlayer.DrawCard(_card, true);
        MyPlayer.AddCardToHand(_card);
        
        _card = OpponentPlayer.GetCardFromDeck(0);
        OpponentPlayer.DrawCard(_card, true);
        OpponentPlayer.AddCardToHand(_card);
    }

    private IEnumerator SecondDraw()
    {
        
        CardObject _card1 = MyPlayer.GetCardFromDeck(4);
        MyPlayer.DrawCard(_card1, true);
        MyPlayer.AddCardToHand(_card1);
        OnDrawSecondTwoCards?.Invoke(_card1);
        
        CardObject _card2 = MyPlayer.GetCardFromDeck(5);
        MyPlayer.DrawCard(_card2, true);
        MyPlayer.AddCardToHand(_card2);
        OnDrawSecondTwoCards?.Invoke(_card2);

        yield break;
    }

    public void ShowMana()
    {
        StartCoroutine(SecondDraw()); // Ovde ubaci sledece dve karte
        manaDisplay.Setup();
    }
}
