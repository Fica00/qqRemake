using System.Collections;
using Photon.Pun;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class GameplayManagerPVP : GameplayManager
{
    public static Action<PlaceCommand> OpponentAddedCommand;
    public static Action<PlaceCommand> OpponentCanceledCommand;
    private PhotonView photonView;

    private bool iAmReadyToStart;
    private bool opponentIsReadyToStart;

    protected override void Awake()
    {
        photonView = GetComponent<PhotonView>();
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

    private void LeaveRoom(GameResult _result)
    {
        PhotonManager.Instance.LeaveRoom();
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
        photonView.RPC("OpponentIsReadyToStart",RpcTarget.Others);
    }

    protected override void EndTurn()
    {
        base.EndTurn();
        string _commands = JsonConvert.SerializeObject(GenerateCommandsJson());
        photonView.RPC("OpponentFinishedTurn", RpcTarget.Others,_commands);
    }

    private string GenerateCommandsJson()
    {
        List<PlaceCommandJson> _commands = new List<PlaceCommandJson>();
        foreach (var _placeCommand in CommandsHandler.MyCommands)
        {
            _commands.Add(PlaceCommandJson.Create(_placeCommand));
        }

        //convert my command to opponent command
        foreach (var _command in _commands)
        {
            _command.MyPlayer = false;
            _command.PlaceId += 4;
        }

        return JsonConvert.SerializeObject(_commands);
    }

    protected override void Forfiet()
    {
        photonView.RPC(nameof(OpponentEscaped), RpcTarget.Others);
        base.Forfiet();
    }

    public override void AddPowerOfQoomonOnPlace(int _placeId, int _power)
    {
        base.AddPowerOfQoomonOnPlace(_placeId, _power);
        photonView.RPC(nameof(DoAddPowerToQoomonOnPlace), RpcTarget.Others, _placeId,_power);
    }

    [PunRPC]
    private void DoAddPowerToQoomonOnPlace(int _placeId, int _power)
    {
        base.AddPowerOfQoomonOnPlace(_placeId, _power);
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
        if (endTurnHandler.TimeLeft > 2)
        {
            photonView.RPC("OpponentWantsToUndoState", RpcTarget.Others);
        }
    }

    protected override void AcceptAutoBet()
    {
        if (!PhotonManager.Instance.IsMasterClient)
        {
            return;
        }
        base.AcceptAutoBet();
    }

    public override void Bet()
    {
        photonView.RPC("OpponentWantsToBet", RpcTarget.Others);
    }

    public override void OpponentAcceptedBet()
    {
        base.OpponentAcceptedBet();
        photonView.RPC("OpponentAceptedBet",RpcTarget.Others);
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
        if (!PhotonNetwork.IsMasterClient)
        {
            yield break;
        }

        StartCoroutine(base.RevealLocation());
    }

    protected override LaneAbility GetLaneAbility()
    {
        LaneAbility _laneAbility = LaneAbilityManager.Instance.GetLaneAbility(excludeLaneAbilities);
        photonView.RPC("ShowLaneAbility",RpcTarget.Others,_laneAbility.Id);
        return _laneAbility;
    }

    public void ForcePlace(PlaceCommand _command)
    {
        PlaceCommandJson _commandJSON = PlaceCommandJson.Create(_command);
        _commandJSON.MyPlayer = false;
        _commandJSON.PlaceId += 4;
        
        photonView.RPC("OpponentForcePlacedCard",RpcTarget.Others,JsonConvert.SerializeObject(_commandJSON));
    }

    public void TellOpponentToDestroyCardsOnTable(List<CardObject> _qommons,bool _destroyMyCards)
    {
        List<int> _placeIds = new List<int>();
        
        foreach (var _qommon in _qommons)
        {
            LanePlaceIdentifier _identifier = _qommon.GetComponentInParent<LanePlaceIdentifier>();
            int _placeId = _identifier.Id;
            _placeId += _destroyMyCards ? 4 : -4;
            _placeIds.Add(_placeId);
        }
        
        photonView.RPC("OpponentDestroyedCardsOnTable",RpcTarget.Others,_placeIds);
    }

    public override void DrawCardFromOpponentsDeck(bool _isMy)
    {
        int _amountOfCardsInHand = MyPlayer.AmountOfCardsInHand;
        if (_amountOfCardsInHand >= MaxAmountOfCardsInHand)
        {
            return;
        }
        photonView.RPC("OpponentWantsCardFromYourDeck",RpcTarget.Others);
    }

    public void TellOpponentToAddPowerToQommons(List<CardObject> _cards, int _powerToAdd)
    {
        List<int> _cardPlaces = new List<int>();
        foreach (var _card in _cards)
        {
            LanePlaceIdentifier _identifier = _card.GetComponentInParent<LanePlaceIdentifier>();
            _cardPlaces.Add(_identifier.Id+4);
        }
        
        photonView.RPC("OpponentAddedPowerToQommons",RpcTarget.Others,_cardPlaces,_powerToAdd);
    }

    public override void TellOpponentThatIDiscardedACard(CardObject _card)
    {
        photonView.RPC(nameof(OpponentDiscardedACard),RpcTarget.Others,_card.Details.Id);
    }

    public override void ChangeCardEnergy(CardObject _randomCardInHand, int _amount)
    {
        photonView.RPC(nameof(ChangeCardEnergyRPC), RpcTarget.Others, _randomCardInHand.Details.Id);
    }

    [PunRPC]
    private void ChangeCardEnergyRPC(int _cardID, int _amount) 
    {
        CardObject _randomCardInHand = MyPlayer.GetCardFromHand(_cardID);
        _randomCardInHand.Stats.Energy += _amount;
    }

    [PunRPC]
    private void OpponentIsReadyToStart()
    {
        opponentIsReadyToStart = true;
    }

    [PunRPC]
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

    [PunRPC]
    private void OpponentEscaped()
    {
        StopAllCoroutines();
        GameEnded?.Invoke(GameResult.Escaped);
    }

    [PunRPC]
    private void OpponentWantsToUndoState()
    {
        if (iFinished)
        {
            return;
        }

        if (GameplayManager.Instance.GameplayState != GameplayState.Playing)
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
        photonView.RPC("UndoState", RpcTarget.Others);
    }

    [PunRPC]
    private void UndoState()
    {
        GameplayState = GameplayState.Playing;
        iFinished = false;
    }

    [PunRPC]
    private void OpponentWantsToBet()
    {
        FindObjectOfType<BetClickHandler>().ShowOpponentWantsToIncreaseBet();
    }

    [PunRPC]
    private void ShowLaneAbility(int _laneId)
    {
        StartCoroutine(RevealLocation(_laneId));
    }

    [PunRPC]
    private void OpponentAceptedBet()
    {
        base.OpponentAcceptedBet();
    }

    [PunRPC]
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

    [PunRPC]
    private void OpponentDestroyedCardsOnTable(List<int> _placeIds)
    {
        List<CardObject> _cards = new List<CardObject>();
        List<LanePlaceIdentifier> _placeIdentifiers = GameObject.FindObjectsOfType<LanePlaceIdentifier>().ToList();
        foreach (var _placeId in _placeIds)
        {
            LanePlaceIdentifier _place = _placeIdentifiers.Find(_element => _element.Id == _placeId);
            _cards.Add(_place.GetComponentInChildren<CardObject>());
        }
        
        foreach (var _card in _cards)
        {
            OpponentPlayer.DestroyCardFromTable(_card);
        }
    }

    [PunRPC]
    private void OpponentWantsCardFromYourDeck()
    {
        CardObject _drawnCard = MyPlayer.DrawCard();
        if (_drawnCard==null)
        {
            return;
        }

        string _jsonStats = JsonConvert.SerializeObject(_drawnCard.Stats);
        photonView.RPC("",RpcTarget.Others, _drawnCard.Details.Id, _jsonStats);
        
    }

    [PunRPC]
    private void OpponentGaveYouCard(int _cardId, string _jsonStats)
    {
        CardObject _createdCard = CardsManager.Instance.CreateCard(_cardId,true);
        _createdCard.Stats = JsonConvert.DeserializeObject<CardStats>(_jsonStats);
        MyPlayer.AddedCardToHand?.Invoke(_createdCard,true);
    }

    [PunRPC]
    private void OpponentAddedPowerToQommons(List<int> _placeIds, int _powerToAdd)
    {
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

    [PunRPC]
    private void OpponentDiscardedACard(int _cardId)
    {
        ShowOpponentDiscardedACard(_cardId);
    }
}
