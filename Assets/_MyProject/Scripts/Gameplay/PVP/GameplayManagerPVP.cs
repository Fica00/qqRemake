using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class GameplayManagerPVP : GameplayManager
{
    PhotonView photonView;

    protected override void Awake()
    {
        photonView = GetComponent<PhotonView>();
        Instance = this;
    }

    protected override void EndTurn()
    {
        base.EndTurn();
        photonView.RPC("OpponentFinishedTurn", RpcTarget.Others);
    }

    protected override void YesForfiet()
    {
        base.YesForfiet();
        photonView.RPC("OpponentForfited", RpcTarget.Others);
    }

    protected override void SetupPlayers()
    {
        MyPlayer.Setup();
    }

    protected override void InitialDraw()
    {
        InitialDraw(MyPlayer, startingAmountOfCards);
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


    [PunRPC]
    void OpponentFinishedTurn()
    {
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
    }
}
