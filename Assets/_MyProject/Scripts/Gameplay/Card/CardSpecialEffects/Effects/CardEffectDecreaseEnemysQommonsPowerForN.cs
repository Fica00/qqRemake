using System.Collections.Generic;
using UnityEngine;

public class CardEffectDecreaseEnemysQommonsPowerForN : CardEffectBase
{
    [SerializeField] private int powerToDeduce;
    public override void Subscribe()
    {
        List<CardObject> _opponentsCards = GameplayManager.Instance.TableHandler.GetCards(!cardObject.IsMy,cardObject.LaneLocation);
        foreach (var _card in _opponentsCards)
        {
            for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.GetAmountOfOngoingEffects(cardObject.IsMy); _i++)
            {
                _card.Stats.Power -= powerToDeduce;
            }
        }
    }
}
