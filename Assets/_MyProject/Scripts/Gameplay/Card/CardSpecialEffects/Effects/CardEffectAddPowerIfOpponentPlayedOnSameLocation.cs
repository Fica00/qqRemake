using System.Collections.Generic;
using UnityEngine;

public class CardEffectAddPowerIfOpponentPlayedOnSameLocation : CardEffectBase
{
    [SerializeField] private int powerToAdd;
    
    public override void Subscribe()
    {
        CheckIfOpponentPlayedOnSameLocation();
    }

    private void CheckIfOpponentPlayedOnSameLocation()
    {
        List<PlaceCommand> _commandsThisRound = cardObject.IsMy ? GameplayManager.Instance.CommandsHandler.OpponentCommandsThisTurn : GameplayManager.Instance.CommandsHandler.MyCommandsThisTurn;
        foreach (var _command in _commandsThisRound)
        {
            if (_command.Card.LaneLocation == cardObject.LaneLocation)
            {
                for (int i = 0; i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; i++)
                {
                    cardObject.Stats.Power += powerToAdd;
                }
                LanePlaceIdentifier _placeIdentifier = _command.Card.GetComponentInParent<LanePlaceIdentifier>();
                GameplayManager.Instance.FlashLocation(_placeIdentifier.Id, new Color(0, 0, 0, 0), 2);
                break;
            }
        }
    }
}
