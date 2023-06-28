
using System;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectDrawXCardsIfOpponentPlayedQommonHereThisTurn : CardEffectBase
{
    [SerializeField] private int amountOfCardsToDraw;
    
    public override void Subscribe()
    {
        List<PlaceCommand> _commandsThisRound = cardObject.IsMy ? GameplayManager.Instance.CommandsHandler.OpponentCommands : GameplayManager.Instance.CommandsHandler.MyCommands;
        foreach (var _command in _commandsThisRound)
        {
            if (_command.Card.LaneLocation == cardObject.LaneLocation)
            {
                for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; _i++)
                {
                    for (int _j = 0; _j < amountOfCardsToDraw; _j++)
                    {
                        GameplayManager.Instance.DrawCard();
                    }
                }

                break;
            }
        }
    }
}
