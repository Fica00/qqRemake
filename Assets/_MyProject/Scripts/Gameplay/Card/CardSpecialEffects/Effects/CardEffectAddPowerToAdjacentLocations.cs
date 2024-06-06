using System;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectAddPowerToAdjacentLocations : CardEffectBase
{
    [SerializeField] private int amountOfPower;
    private int _powerAdded;
    
    public override void Subscribe()
    {
        ChangePower(amountOfPower);
    }

    private void OnDisable()
    {
        if (_powerAdded==0)
        {
            return;
        }

        try
        {
            ChangePower(-_powerAdded);
        }
        catch
        {
            // ignored
        }

        _powerAdded = 0;
    }

    void ChangePower(int _amountOfPower)
    {
        List<LaneDisplay> _effectedLocations = new List<LaneDisplay>();
        switch (cardObject.LaneLocation)
        {
            case LaneLocation.Top:
                _effectedLocations.Add(GameplayManager.Instance.Lanes[1]);
                break;
            case LaneLocation.Mid:
                _effectedLocations.Add(GameplayManager.Instance.Lanes[0]);
                _effectedLocations.Add(GameplayManager.Instance.Lanes[2]);
                break;
            case LaneLocation.Bot:
                _effectedLocations.Add(GameplayManager.Instance.Lanes[1]);
                break;
            case LaneLocation.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        int _powerToAdd = 0;

        for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfOngoingEffects; _i++)
        {
            _powerToAdd += _amountOfPower;
        }

        int _index = cardObject.IsMy ? 0 : 1;
        _powerAdded = _powerToAdd;

        foreach (var _location in _effectedLocations)
        {
            _location.LaneSpecifics.ChangeExtraPower(_index,_powerToAdd);
            _location.ShowEnlargedPowerAnimation(cardObject.IsMy);
        }
    }
}
