using System;
using UnityEngine;

public class CardEffectAddPowerToLaneBelow : CardEffectBase
{
    [SerializeField] private int amountOfPower;
    
    public override void Subscribe()
    {
        AddPower();
    }

    void AddPower()
    {
        LaneDisplay _lane = null;

        switch (cardObject.LaneLocation)
        {
            case LaneLocation.Top:
                _lane = GameplayManager.Instance.Lanes[1];
                break;
            case LaneLocation.Mid:
                _lane = GameplayManager.Instance.Lanes[2];
                break;
            case LaneLocation.Bot:
                _lane = null;
                break;
            case LaneLocation.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (_lane==null)
        {
            return;
        }

        int _index = cardObject.IsMy ? 0 : 1;

        for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfOngoingEffects; _i++)
        {
            _lane.LaneSpecifics.ChangeExtraPower(_index,amountOfPower);
        }
    }
}
