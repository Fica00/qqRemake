using System.Collections;
using Photon.Pun;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameplayManagerPVP : GameplayManager
{
    public static Action<PlaceCommand> OpponentAddedCommand;
    public static Action<PlaceCommand> OpponentCanceledCommand;
    PhotonView photonView;

    bool iAmReadyToStart;
    bool opponentIsReadyToStart;

    protected override void Awake()
    {
        photonView = GetComponent<PhotonView>();
        Instance = this;
        IsPVPGame = true;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameEnded += LeaveRoom;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameEnded += LeaveRoom;
    }

    private void LeaveRoom(GameResult _result)
    {
        PhotonManager.Instance.LeaveRoom();
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(DelayStart());
    }

    IEnumerator DelayStart()
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

    string GenerateCommandsJson()
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
        base.Forfiet();
        photonView.RPC("OpponentForfited", RpcTarget.Others);
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

    public override void IncreaseBet()
    {
        base.IncreaseBet();
        photonView.RPC("OpponentIncreasedBet", RpcTarget.Others);
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


    [PunRPC]
    void OpponentIsReadyToStart()
    {
        opponentIsReadyToStart = true;
    }

    [PunRPC]
    void OpponentFinishedTurn(string _commandsJson)
    {
        _commandsJson = _commandsJson.Replace("\\\"","\"");
        _commandsJson = _commandsJson.Substring(1, _commandsJson.Length - 2);
        Debug.Log(_commandsJson);
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
    void OpponentForfited()
    {
        StopAllCoroutines();
        UIManager.Instance.OkDialog.Setup("Opponent has forfieted the match!\nYouWin!");
        GameEnded?.Invoke(GameResult.IWon);
    }

    [PunRPC]
    void OpponentWantsToUndoState()
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
    void UndoState()
    {
        GameplayState = GameplayState.Playing;
        iFinished = false;
    }

    [PunRPC]
    void OpponentIncreasedBet()
    {
        base.IncreaseBet();
        OpponentPlayerDisplay.RemoveGlow();
    }

    [PunRPC]
    void ShowLaneAbility(int _laneId)
    {
        StartCoroutine(RevealLocation(_laneId));
    }
}
