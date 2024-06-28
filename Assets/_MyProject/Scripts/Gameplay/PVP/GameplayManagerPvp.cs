using MessageHelpers;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayManagerPvp : GameplayManager
{
    public static Action<PlaceCommand> OpponentAddedCommand;
    public static Action<PlaceCommand> OpponentCanceledCommand;

    private bool iAmReadyToStart;
    private bool opponentIsReadyToStart;

    protected override void Awake()
    {
        Instance = this;
        IsPvpGame = true;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameEnded += LeaveRoom;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameEnded -= LeaveRoom;
    }

    private void LeaveRoom(GameResult _)
    {
        SocketServerCommunication.Instance.LeaveRoom();
    }

    protected override void StartGameplay()
    {
        base.StartGameplay();
        StartCoroutine(DelayStart());
    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(1);
        iAmReadyToStart = true;
        SocketServerCommunication.Instance.RegisterMessage(gameObject, nameof(OpponentIsReadyToStart));
    }

    protected override void EndTurn()
    {
        base.EndTurn();
        string _commands = JsonConvert.SerializeObject(GenerateCommandsJson());
        SocketServerCommunication.Instance.RegisterMessage(gameObject, nameof(OpponentFinishedTurn), _commands);
    }

    private string GenerateCommandsJson()
    {
        List<PlaceCommandJson> _commands = new List<PlaceCommandJson>();
        foreach (var _placeCommand in CommandsHandler.MyCommands)
        {
            _commands.Add(PlaceCommandJson.Create(_placeCommand));
        }

        foreach (var _command in _commands)
        {
            _command.MyPlayer = false;
            _command.PlaceId += 4;
        }

        return JsonConvert.SerializeObject(_commands);
    }

    protected override void Forfeit()
    {
        SocketServerCommunication.Instance.RegisterMessage(gameObject, nameof(OpponentEscaped));
        base.Forfeit();
    }

    public override void AddPowerOfQoomonOnPlace(int _placeId, int _power)
    {
        base.AddPowerOfQoomonOnPlace(_placeId, _power);
        AddPower _addPower = new AddPower { PlaceId = _placeId, Power = _power };
        SocketServerCommunication.Instance.RegisterMessage(gameObject, nameof(DoAddPowerToQoomonOnPlace), JsonConvert.SerializeObject(_addPower));
    }

    private void DoAddPowerToQoomonOnPlace(string _data)
    {
        AddPower _addPower = JsonConvert.DeserializeObject<AddPower>(_data);
        base.AddPowerOfQoomonOnPlace(_addPower.PlaceId, _addPower.Power);
    }

    protected override void SetupPlayers()
    {
        MyPlayer.Setup();
    }

    protected override IEnumerator InitialDraw()
    {
        yield return StartCoroutine(InitialDraw(MyPlayer, startingAmountOfCards));
    }

    public override void DrawCard()
    {
        DrawCard(MyPlayer);
    }

    public override void ReturnToWaitingState()
    {
        if (endTurnHandler.TimeLeft <= 2)
        {
            return;
        }
        
        SocketServerCommunication.Instance.RegisterMessage(gameObject,nameof(OpponentWantsToUndoState));
    }

    public override void Bet()
    {
        SocketServerCommunication.Instance.RegisterMessage(gameObject,nameof(OpponentWantsToBet));
    }

    public override void OpponentAcceptedBet()
    {
        base.OpponentAcceptedBet();
        SocketServerCommunication.Instance.RegisterMessage(gameObject,nameof(OpponentAceptedBet));
    }

    public override void UpdateQommonCosts(int _amount)
    {
        MyPlayer.UpdateQommonCost(_amount);
    }

    protected override void RoundDrawCard()
    {
        DrawCard(MyPlayer);
    }

    protected override bool ReadyToStart()
    {
        return iAmReadyToStart && opponentIsReadyToStart;
    }

    protected override IEnumerator RoundCheckForCardsThatShouldMoveToHand()
    {
        yield return StartCoroutine(CheckForCardsThatShouldMoveToHand(MyPlayer));
    }

    protected override IEnumerator RevealLocation()
    {
        if (!SocketServerCommunication.Instance.MatchData.IsMasterClient)
        {
            yield break;
        }

        StartCoroutine(base.RevealLocation());
    }

    protected override LaneAbility GetLaneAbility()
    {
        LaneAbility _laneAbility = LaneAbilityManager.Instance.GetLaneAbility(excludeLaneAbilities);
        SendLaneAbility _abilityData = new SendLaneAbility { Id = _laneAbility.Id };
        SocketServerCommunication.Instance.RegisterMessage(gameObject,nameof(ShowLaneAbility), JsonConvert.SerializeObject(_abilityData));
        return _laneAbility;
    }

    public void ForcePlace(PlaceCommand _command)
    {
        PlaceCommandJson _commandJSON = PlaceCommandJson.Create(_command);
        _commandJSON.MyPlayer = false;
        _commandJSON.PlaceId += 4;
        
        SocketServerCommunication.Instance.RegisterMessage(gameObject,nameof(OpponentForcePlacedCard), JsonConvert.SerializeObject(_commandJSON));
    }

    public override void DrawCardFromOpponentsDeck(bool _isMy)
    {
        int _amountOfCardsInHand = MyPlayer.AmountOfCardsInHand;
        if (_amountOfCardsInHand >= MaxAmountOfCardsInHand)
        {
            return;
        }
        
        SocketServerCommunication.Instance.RegisterMessage(gameObject, nameof(OpponentWantsCardFromYourDeck));
    }

    public void TellOpponentToAddPowerToQommons(List<CardObject> _cards, int _powerToAdd)
    {
        List<int> _cardPlaces = new List<int>();
        foreach (var _card in _cards)
        {
            LanePlaceIdentifier _identifier = _card.GetComponentInParent<LanePlaceIdentifier>();
            _cardPlaces.Add(_identifier.Id+4);
        }

        AddPowerToPlaces _addPower = new AddPowerToPlaces { CardPlaces = _cardPlaces, PowerToAdd = _powerToAdd };
        SocketServerCommunication.Instance.RegisterMessage(gameObject,nameof(OpponentAddedPowerToQommons), JsonConvert.SerializeObject(_addPower));
    }

    public override void TellOpponentThatIDiscardedACard(CardObject _card)
    {
        DiscardCard _discard = new DiscardCard { CardId = _card.Details.Id };
        SocketServerCommunication.Instance.RegisterMessage(gameObject,nameof(OpponentDiscardedACard), JsonConvert.SerializeObject(_discard));
    }

    public override void DestroyCardsOnTable(List<int> _idList)
    {
        base.DestroyCardsOnTable(_idList);
        DestroyCards _cards = new DestroyCards {PlaceIds = _idList};
        SocketServerCommunication.Instance.RegisterMessage(gameObject,nameof(OpponentDestroyedCardsOnTable), JsonConvert.SerializeObject(_cards));
    }

    public override void ChangeAllInOpponentHandPower(int _amount, GameplayPlayer _player) 
    {
        AddPower _addPower = new AddPower { Power = _amount};
        SocketServerCommunication.Instance.RegisterMessage(gameObject, nameof(ChangeAllOpponentCardsInHandPower), JsonConvert.SerializeObject(_addPower));
    }

    public override void ChangeInOpponentHandRandomCardEnergy(int _lessThan, int _amount, GameplayPlayer _player)
    {
        AddEnergy _addEnergy = new AddEnergy { Energy = _amount , Cost = _lessThan};
        SocketServerCommunication.Instance.RegisterMessage(gameObject, nameof(ChangeOpponentCardEnergy), JsonConvert.SerializeObject(_addEnergy));
    }

    private void ChangeOpponentCardEnergy(string _data)
    {
        AddEnergy _addEnergy = JsonConvert.DeserializeObject<AddEnergy>(_data);
        base.ChangeInOpponentHandRandomCardEnergy(_addEnergy.Cost, _addEnergy.Energy, MyPlayer);
    }

    private void ChangeAllOpponentCardsInHandPower(string _data)
    {
        AddPower _addPower = JsonConvert.DeserializeObject<AddPower>(_data);
        base.ChangeAllInOpponentHandPower(_addPower.Power, MyPlayer);
    }

    private void OpponentIsReadyToStart()
    {
        opponentIsReadyToStart = true;
    }

    private void OpponentFinishedTurn(string _commandsJson)
    {
        _commandsJson = _commandsJson.Replace("\\\"","\"");
        _commandsJson = _commandsJson.Substring(1, _commandsJson.Length - 2);
        List<PlaceCommandJson> _placeCommandsJson = JsonConvert.DeserializeObject<List<PlaceCommandJson>>(_commandsJson);
        List<PlaceCommand> _placeCommands = new List<PlaceCommand>();

        foreach (var _placeCommandJson in _placeCommandsJson)
        {
            PlaceCommand _placeCommand = PlaceCommandJson.ToPlaceCommand(_placeCommandJson);
            _placeCommand.Card.SetCardLocation(CardLocation.Table);
            _placeCommand.Card.Display.Hide();
            OpponentAddedCommand?.Invoke(_placeCommand);
            _placeCommands.Add(_placeCommand);
        }

        CommandsHandler.SetOpponentCommands(_placeCommands);

        OpponentFinished();
    }

    private void OpponentEscaped()
    {
        StopAllCoroutines();
        GameEnded?.Invoke(GameResult.Escaped);
    }

    private void OpponentWantsToUndoState()
    {
        if (iFinished)
        {
            return;
        }

        if (GameplayState != GameplayState.Playing)
        {
            return;
        }

        if (!opponentFinished)
        {
            return;
        }

        foreach (var _command in CommandsHandler.OpponentCommands)
        {
            OpponentCanceledCommand?.Invoke(_command);
        }
        CommandsHandler.OpponentCommands.Clear();

        opponentFinished = false;
        SocketServerCommunication.Instance.RegisterMessage(gameObject,nameof(UndoState));
    }

    private void UndoState()
    {
        GameplayState = GameplayState.Playing;
        iFinished = false;
    }

    private void OpponentWantsToBet()
    {
        BetClickHandler.Instance.ShowOpponentWantsToIncreaseBet();
    }

    private void ShowLaneAbility(string _data)
    {
        SendLaneAbility _abilityData = JsonConvert.DeserializeObject<SendLaneAbility>(_data);
        StartCoroutine(RevealLocation(_abilityData.Id));
    }

    private void OpponentAceptedBet()
    {
        base.OpponentAcceptedBet();
    }

    private void OpponentForcePlacedCard(string _json)
    {
        _json = _json.Replace("\\\"","\"");
        Debug.Log(_json);
        PlaceCommandJson _placeCommandsJson = JsonConvert.DeserializeObject<PlaceCommandJson>(_json);

        PlaceCommand _placeCommand = PlaceCommandJson.ToPlaceCommand(_placeCommandsJson);
        _placeCommand.Card.SetCardLocation(CardLocation.Table);
        _placeCommand.Card.Display.Hide();
        OpponentAddedCommand?.Invoke(_placeCommand);

        CommandsHandler.OpponentCommands.Add(_placeCommand);
    }

    private void OpponentDestroyedCardsOnTable(string _data)
    {
        DestroyCards _destroyCards = JsonConvert.DeserializeObject<DestroyCards>(_data);
        List<int> _actualPlaceIds = new List<int>();
        foreach (var _placeId in _destroyCards.PlaceIds)
        {
            _actualPlaceIds.Add(_placeId-4);
        }
        base.DestroyCardsOnTable(_actualPlaceIds);
    }

    private void OpponentWantsCardFromYourDeck()
    {
        CardObject _drawnCard = MyPlayer.DrawCard();
        if (_drawnCard==null)
        {
            return;
        }

        string _jsonStats = JsonConvert.SerializeObject(_drawnCard.Stats);
        GiveOpponentCard _card = new GiveOpponentCard { CardId = _drawnCard.Details.Id, Stats = _jsonStats };
        SocketServerCommunication.Instance.RegisterMessage(gameObject,nameof(OpponentGaveYouCard), JsonConvert.SerializeObject(_card));
    }

    private void OpponentGaveYouCard(string _data)
    {
        GiveOpponentCard _card = JsonConvert.DeserializeObject<GiveOpponentCard>(_data);
        int _cardId = _card.CardId;
        string _jsonStats = _card.Stats;
        CardObject _createdCard = CardsManager.Instance.CreateCard(_cardId,true);
        _createdCard.Stats = JsonConvert.DeserializeObject<CardStats>(_jsonStats);
        MyPlayer.AddedCardToHand?.Invoke(_createdCard,true);
    }

    private void OpponentAddedPowerToQommons(string _data)
    {
        AddPowerToPlaces _addPower = JsonConvert.DeserializeObject<AddPowerToPlaces>(_data);
        List<int> _placeIds = _addPower.CardPlaces;
        int _powerToAdd = _addPower.PowerToAdd;
        List<CardObject> _cards = new List<CardObject>();
        List<LanePlaceIdentifier> _placeIdentifiers = GameObject.FindObjectsOfType<LanePlaceIdentifier>().ToList();
        foreach (var _placeId in _placeIds)
        {
            LanePlaceIdentifier _place = _placeIdentifiers.Find(_element => _element.Id == _placeId);
            FlashLocation(_placeId,Color.white,3);
            _cards.Add(_place.GetComponentInChildren<CardObject>());
        }
        
        foreach (var _card in _cards)
        {
            _card.Stats.Power += _powerToAdd;
        }
    }

    private void OpponentDiscardedACard(string _data)
    {
        DiscardCard _discard = JsonConvert.DeserializeObject<DiscardCard>(_data);
        ShowOpponentDiscardedACard(_discard.CardId);
    }
}
