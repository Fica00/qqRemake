using System;
using UnityEngine;

public class CardEffectAddPowerToLaneBelow : CardEffectBase
{
    [SerializeField] private int amountOfPower;
    private int powerAdded;
    private bool subscribed;
    
    public override void Subscribe()
    {
        AddPower();
        LaneSpecifics.UpdatedAmountOfOngoingEffects += Recalculate;
        subscribed = true;
    }

    private void OnDisable()
    {
        if (!subscribed)
        {
            return;
        }
        
        LaneSpecifics.UpdatedAmountOfOngoingEffects -= Recalculate;

    }

    private void Recalculate()
    {
        LaneDisplay _lane = GetLane();
        if (_lane)
        {
            ChangeLanePower(_lane,-powerAdded);
        }

        powerAdded = 0;
        AddPower();
    }

    void AddPower()
    {
        LaneDisplay _lane = GetLane();

        if (_lane==null)
        {
            return;
        }

        int _powerToAdd = 0;
        for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfOngoingEffects; _i++)
        {
            _powerToAdd += amountOfPower;
        }

        ChangeLanePower(_lane,_powerToAdd);
        powerAdded = _powerToAdd;
        
        _lane.ShowEnlargedPowerAnimation(cardObject.IsMy);
    }

    private LaneDisplay GetLane()
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

        return _lane;
    }

    private void ChangeLanePower(LaneDisplay _lane, int _powerToAdd)
    {
        int _index = cardObject.IsMy ? 0 : 1;
        _lane.LaneSpecifics.ChangeExtraPower(_index,_powerToAdd);
    }
}
