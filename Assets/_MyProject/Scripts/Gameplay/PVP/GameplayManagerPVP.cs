using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManagerPVP : GameplayManager
{
    protected override void Awake()
    {
        Instance = this;
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
            //todo fist ask opponent, if you get confirmation from him set gameplay state and i finish
            GameplayState = GameplayState.Playing;
        }
        iFinished = false;
    }
}
