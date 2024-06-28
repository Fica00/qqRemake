using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;
    public static bool IsPvpGame;
    public static Action UpdatedRound;
    public static Action UpdatedGameState;
    public static Action OnFinishedGameplayLoop;
    public static Action<GameResult> GameEnded;
    public static Action<int, Color, int> OnFlashPlace;
    public static Action<LaneLocation, bool, Color, int> OnFlashWholePlace;
    public static Action<LaneLocation, bool, Color> OnHighlihtWholePlace;
    public static Action<LaneLocation, bool> OnHighlihtWholePlaceDotted;
    public static Action<LaneLocation, bool, Color, int> OnFlashAllSpotsOnLocation;
    public static Action<LaneLocation, bool, Color> OnHideHighlightWholePlace;
    public static Action<LaneLocation, bool> OnHideHighlightWholePlaceDotted;
    public static Action OnGameplayStarted;
    public static bool DrewCardDirectlyToHand;
    
    public GameplayPlayer MyPlayer;
    public GameplayPlayer OpponentPlayer;
    public Dictionary<LaneDisplay, LaneAbility> LaneAbilities = new Dictionary<LaneDisplay, LaneAbility>();

    [field: SerializeField] public int MaxAmountOfCardsInHand { get; private set; }
    [field: SerializeField] public int DurationOfRound { get; private set; }
    [field: SerializeField] public TableHandler TableHandler { get; private set; }

    public CommandsHandler CommandsHandler = new CommandsHandler();

    [SerializeField] protected EndTurnHandler endTurnHandler;
    [SerializeField] protected int maxRounds = 6;
    [SerializeField] protected List<LaneDisplay> lanes;
    [SerializeField] protected GameObject[] flags;
    [SerializeField] protected GameObject[] playsFirstDisplays;

    private GameplayState gameplayState = GameplayState.StartingAnimation;
    private int currentRound;

    protected bool opponentFinished;
    protected bool iFinished;
    protected bool resolvedEndOfTheRound;
    protected int startingAmountOfCards = 3;
    protected List<int> excludeLaneAbilities = new List<int>();
    protected bool locationRevealed;

    public bool IFinished => iFinished;

    public bool IsLastRound => CurrentRound == maxRounds;

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

    public int MaxAmountOfRounds => maxRounds;

    protected virtual void OnEnable()
    {
        EndTurnHandler.OnEndTurn += EndTurn;
        FlagClickHandler.OnForefiet += Forfeit;
        GameEnded += UpdateQommonsWinLose;
        GameEnded += TriggerGameEndEvents;
    }

    protected virtual void OnDisable()
    {
        CommandsHandler.Close();
        EndTurnHandler.OnEndTurn -= EndTurn;
        FlagClickHandler.OnForefiet -= Forfeit;
        GameEnded -= UpdateQommonsWinLose;
        GameEnded -= TriggerGameEndEvents;
    }

    protected virtual void EndTurn()
    {
        GameplayState = GameplayState.Waiting;
        iFinished = true;
        MyPlayer.FinishedTurn?.Invoke();
    }

    protected virtual void Forfeit()
    {
        StopAllCoroutines();
        GameEnded?.Invoke(GameResult.IForefiet);
    }
    
    public void ForceEndGame(GameResult _result)
    {
        StopAllCoroutines();
        GameEnded?.Invoke(_result);
    }
    
    private void UpdateQommonsWinLose(GameResult _result)
    {
        switch (_result)
        {
            case GameResult.IForefiet or GameResult.ILost:
                FirebaseManager.Instance.UpdateCardsWinLoseCount(DataManager.Instance.PlayerData.CardIdsInDeck, false);
                break;
            case GameResult.IWon or GameResult.Escaped:
                FirebaseManager.Instance.UpdateCardsWinLoseCount(DataManager.Instance.PlayerData.CardIdsInDeck, true);
                break;
        }
    }
    
    private void TriggerGameEndEvents(GameResult _result)
    {
        if(!DataManager.Instance.PlayerData.HasPlayedFirstGame)
        {
            DataManager.Instance.PlayerData.HasPlayedFirstGame = true;
            DataManager.Instance.PlayerData.HasFinishedFirstGame = true;
        }
        else
        {
            DataManager.Instance.CanShowPwaOverlay = true;
        }
        
        if (_result is not (GameResult.IWon or GameResult.Escaped))
        {
            return;
        }

        if (BetClickHandler.Instance.CurrentBet>2)
        {
            EventsManager.WinMatchWithADouble?.Invoke();
        }
        EventsManager.WinMatch?.Invoke();
        
        CheckForPowerEvents(TableHandler.GetPower(true,LaneLocation.Top));
        CheckForPowerEvents(TableHandler.GetPower(true,LaneLocation.Mid));
        CheckForPowerEvents(TableHandler.GetPower(true,LaneLocation.Bot));
        
        CheckForCardEvents(TableHandler.GetCards(true, LaneLocation.Top).Count);
        CheckForCardEvents(TableHandler.GetCards(true, LaneLocation.Mid).Count);
        CheckForCardEvents(TableHandler.GetCards(true, LaneLocation.Bot).Count);

        void CheckForPowerEvents(float _power)
        {
            if (_power<=100)
            {
                EventsManager.WinALocationWithPowerLess100?.Invoke();
            }
            else if (_power>=200)
            {
                EventsManager.WinALocationWithPowerMore200?.Invoke();
            }
        }

        void CheckForCardEvents(int _cardAmount)
        {
            if (_cardAmount==1)
            {
                EventsManager.WinALocationWith1Card?.Invoke();
            }
            else if (_cardAmount==4)
            {
                EventsManager.WinALocationWith4Card?.Invoke();
            }
        }
    }

    protected virtual void Awake()
    {
        Instance = this;
        IsPvpGame = false;
    }

    private void Start()
    {
        AudioManager.Instance.ChangeBackgroundMusic(AudioManager.GAME);
        GameplayUI.Instance.StartingAnimations(StartGameplay);   
    }

    public virtual void AddPowerOfQoomonOnPlace(int _placeId, int _power)
    {
        LanePlaceIdentifier _place = FindObjectsOfType<LanePlaceIdentifier>().ToList().Find(_place => _place.Id == _placeId);
        CardObject _cardOnPlace = _place.GetComponentInChildren<CardObject>();
        _cardOnPlace.Stats.Power += _power;
    }

    protected virtual void StartGameplay()
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
        for (int i = 0; i < startingAmountOfCards-_amountOfCardsInHand; i++)
        {
            DrawCard(_player);
        }
    }

    public virtual void DrawCard()
    {
        DrawCard(MyPlayer);
        DrawCard(OpponentPlayer);
    }

    public void DrawCard(GameplayPlayer _player)
    {
        int _amountOfCardsInHand = _player.AmountOfCardsInHand;
        if (_amountOfCardsInHand >= MaxAmountOfCardsInHand)
        {
            return;
        }

        CardObject _drawnCard = _player.DrawCard();
        if (_drawnCard==null)
        {
            return;
        }
        _player.AddCardToHand(_drawnCard);
    }

    public virtual void DrawCardFromOpponentsDeck(bool _isMy)
    {
        GameplayPlayer _player = _isMy ? MyPlayer : OpponentPlayer;
        GameplayPlayer _opponentPlayer = _isMy ? OpponentPlayer : MyPlayer;
        int _amountOfCardsInHand = _player.AmountOfCardsInHand;
        if (_amountOfCardsInHand >= MaxAmountOfCardsInHand)
        {
            return;
        }
        
        CardObject _drawnCard = _opponentPlayer.DrawCard();
        if (_drawnCard==null)
        {
            return;
        }

        int _cardId = _drawnCard.Details.Id;
        Destroy(_drawnCard.gameObject);
        _player.AddCardToHand(CardsManager.Instance.CreateCard(_cardId, _isMy));
    }

    public virtual void DestroyCardsOnTable(List<int> _idList)
    {
        List<CardObject> _cards = new List<CardObject>();
        List<LanePlaceIdentifier> _placeIdentifiers = FindObjectsOfType<LanePlaceIdentifier>().ToList();

        foreach (var _placeId in _idList)
        {
            LanePlaceIdentifier _place = _placeIdentifiers.Find(_element => _element.Id == _placeId);
            _cards.Add(_place.GetComponentInChildren<CardObject>());
        }

        foreach (var _card in _cards)
        {
            MyPlayer.DestroyCardFromTable(_card);
        }
    }

    public virtual void ChangeInMyHandRandomCardsPower(List<int> _randomCardsId, int _amount, GameplayPlayer _player)
    {
        var _myCardsInHand = _player.CurrentCardsInHand;

        if (_myCardsInHand.Count == 0)
        {
            return;
        }

        List<CardObject> _randomCards = new List<CardObject>();

        for (int i = 0; i < _randomCardsId.Count; i++)
        {
            foreach (var _card in _myCardsInHand)
            {
                if (_card.Details.Id == _randomCardsId[i])
                {
                    _randomCards.Add(_card);
                }
            }
        }

        if (_randomCards.Count == 0)
        {
            return;
        }

        foreach (var _card in _randomCards) 
        {
            _card.Stats.Power += _amount;
        }
    }

    public virtual void ChangeInOpponentHandRandomCardEnergy(int _lessThan, int _amount, GameplayPlayer _player) 
    {
        var _opponentsCardsInHand = _player.CurrentCardsInHand;

        if (_opponentsCardsInHand.Count == 0)
        {
            return;
        }

        CardObject _randomCardInHand = _opponentsCardsInHand.OrderBy(_ => Guid.NewGuid()).First(_qoomon => _qoomon.Stats.Energy < _lessThan);

        if (_randomCardInHand == null)
        {
            return;
        }

        _randomCardInHand.Stats.Energy += _amount;
    }

    public virtual void ChangeAllInOpponentHandPower(int _amount, GameplayPlayer _player)
    {
        var _opponentsCardsInHand = _player.CurrentCardsInHand;

        if (_opponentsCardsInHand.Count == 0)
        {
            return;
        }

        foreach (var _card in _opponentsCardsInHand)
        {
            _card.Stats.Power += _amount;
        }
    }

    public virtual void ChangeAllMyDeckPower(int _amount, GameplayPlayer _player)
    {
        var _deckCards = _player.CardsInDeck;

        if (_deckCards.Count == 0)
        {
            return;
        }

        foreach (var _card in _deckCards)
        {
            _card.Stats.Power += _amount;
        }
    }

    protected virtual IEnumerator GameplayRoutine()
    {
        yield return new WaitUntil(ReadyToStart);
        OnGameplayStarted?.Invoke();
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
            if (currentRound <= 3)
            {
                locationRevealed = false;
            }
            yield return new WaitForSeconds(1f); //duration of round animation
            
            AudioManager.Instance.PlaySoundEffect(AudioManager.REVEAL);
            StartCoroutine(RevealLocation());
            StartCoroutine(ShowRevealText());
            yield return new WaitUntil(() => locationRevealed);
            yield return StartCoroutine(RoundCheckForCardsThatShouldMoveToHand());
            if (!DrewCardDirectlyToHand||CurrentRound==1)
            {
                RoundDrawCard();
            }

            DrewCardDirectlyToHand = false;

            GameplayState = GameplayState.Playing;
            yield return new WaitUntil(() => iFinished && opponentFinished);

            GameplayState = GameplayState.ResolvingEndOfRound;
            var _addQommonOnEndOfTheTurn = FindObjectOfType<LaneAbilityOnTurnXAllPutCardHere>();
            if (_addQommonOnEndOfTheTurn)
            {
                if (_addQommonOnEndOfTheTurn.Round == currentRound)
                {
                    yield return new WaitForSeconds(2);
                }
            }
            StartCoroutine(RevealCards(_whoPlaysFirst));
            yield return new WaitUntil(() => resolvedEndOfTheRound);
            yield return new WaitForSeconds(1);
            OnFinishedGameplayLoop?.Invoke();
        }

        if (ModeHandler.ModeStatic!=GameMode.Friendly)
        {
            AcceptAutoBet();
        }
        
        bool _playBackgroundMusic = DataManager.Instance.PlayerData.PlayBackgroundMusic;
        DataManager.Instance.PlayerData.PlayBackgroundMusic = false;
        yield return new WaitForSeconds(0.5f);
        
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

    protected void AcceptAutoBet()
    {
        BetClickHandler.Instance.AcceptAutoBet();
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
        Destroy(_laneAbility.gameObject);
    }
    
    protected IEnumerator ShowRevealText()
    {
        if (currentRound > 3)
        {
            yield break;
        }

        int _laneCounter = 0;
        for (int _i =currentRound; _i < 3; _i++)
        {
            string _text = string.Empty;
            if (_laneCounter==0)
            {
                _text = "Will be revealed next turn";
            }
            else if (_laneCounter == 1)
            {
                _text = "Will be revealed in 2 turns";
            }

            _laneCounter++;
            lanes[_i].AbilityDisplay.Reveal(_text);
        }
    }

    protected virtual LaneAbility GetLaneAbility()
    {
        return LaneAbilityManager.Instance.GetLaneAbility(excludeLaneAbilities);
    }

    protected IEnumerator RevealLocation(int _abilityID)
    {
        bool _canContinue = false;
        LaneAbility _laneAbility = LaneAbilityManager.Instance.GetLaneAbility(_abilityID);
        LaneAbilities.Add(Lanes[currentRound - 1], _laneAbility);
        excludeLaneAbilities.Add(_abilityID);
        int _laneIndex = currentRound - 1;
        _laneAbility.Setup(lanes[_laneIndex]);
        lanes[_laneIndex].AbilityDisplay.Reveal(_laneAbility.Description,_laneAbility.FontSize, Revealed);
        _canContinue = false;

        yield return new WaitUntil(() => _canContinue);
        yield return new WaitForSeconds(0.5f); //add small delay
        locationRevealed = true;

        void Revealed()
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
        while (CommandsHandler.MyCommands.Count>0|| CommandsHandler.OpponentCommands.Count>0)
        {
            foreach (var _command in CommandsHandler.OpponentCommands)
            {
                _command.Card.PrepareForReveal();
            }
            
            AddCommands(CommandsHandler.MyCommands,true);
            AddCommands(CommandsHandler.OpponentCommands,false);

            yield return StartCoroutine(TableHandler.RevealCards(_whoPlaysFirst == -1 ? CommandsHandler.MyCommands : CommandsHandler.OpponentCommands)); //show first set of cards
            yield return StartCoroutine(TableHandler.RevealCards(_whoPlaysFirst == -1 ? CommandsHandler.OpponentCommands : CommandsHandler.MyCommands)); // show secound set of cards

            yield return new WaitForSeconds(2);//some delay

            void AddCommands(List<PlaceCommand> _commands, bool _isMy)
            {
                List<PlaceCommand> _commandsThisTurn =
                    _isMy ? CommandsHandler.MyCommandsThisTurn : CommandsHandler.OpponentCommandsThisTurn;
                
                foreach (var _command in _commands.ToList())
                {
                    if (_commandsThisTurn.Contains(_command))
                    {
                        continue;
                    }
                    
                    _commandsThisTurn.Add(_command);
                }
            }
        }

        CommandsHandler.MyCommandsThisTurn.Clear();
        CommandsHandler.OpponentCommandsThisTurn.Clear();
        resolvedEndOfTheRound = true;
    }

    protected void ShowFlag(int _whoPlaysFirst)
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

        foreach (var _command in CommandsHandler.MyCommands)
        {
            _command.Card.GetComponent<CardInteractions>().CanDrag = true;
        }
        iFinished = false;
    }

    public virtual void Bet()
    {
        StartCoroutine(BetRoutine());
        
        IEnumerator BetRoutine()
        {
            int _currentRound = currentRound;
            yield return new WaitForSeconds(3);
            if (_currentRound!=CurrentRound)
            {
                yield break;
            }
            OpponentAcceptedBet();
        }
    }


    public virtual void OpponentAcceptedBet()
    {
        BetClickHandler.Instance.OpponentAcceptedBet();
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
    
    public void HighlihtWholeLocationDotted(LaneLocation _location, bool _mySide)
    {
        OnHighlihtWholePlaceDotted?.Invoke(_location, _mySide);
    }

    public void FlashAllSpotsOnLocation(LaneLocation _location, bool _mySide, Color _color, int _amount)
    {
        OnFlashAllSpotsOnLocation?.Invoke(_location, _mySide, _color, _amount);
    }

    public void HideHighlihtWholeLocation(LaneLocation _location, bool _mySide, Color _color)
    {
        OnHideHighlightWholePlace?.Invoke(_location, _mySide, _color);
    }

    public void HideHighlihtWholeLocationDotted(LaneLocation _location, bool _mySide)
    {
        OnHideHighlightWholePlaceDotted?.Invoke(_location,_mySide);
    }
    private void OnDestroy()
    {
        DOTween.KillAll();
    }

    public virtual void TellOpponentThatIDiscardedACard(CardObject _card)
    {
        if (_card.IsMy)
        {
            return;
        }
        ShowOpponentDiscardedACard(_card.Details.Id);
    }

    protected void ShowOpponentDiscardedACard(int _cardId)
    {
        OpponentDiscardedCardDisplay.Instance.Show(_cardId);
    }

    public void SetCurrentRoundWithoutUpdate(int _amount)
    {
        currentRound = _amount;
    }

    public List<int> LaneIdForQoomonsToDestroy(List<CardObject> _qommons)
    {
        List<int> _placeIds = new List<int>();

        foreach (var _qommon in _qommons)
        {
            LanePlaceIdentifier _identifier = _qommon.GetComponentInParent<LanePlaceIdentifier>();
            int _placeId = _identifier.Id;
            _placeIds.Add(_placeId);
        }

        return _placeIds;
    }
}