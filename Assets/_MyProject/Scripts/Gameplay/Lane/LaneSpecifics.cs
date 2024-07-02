using System;
using System.Collections.Generic;
using UnityEngine;

public class LaneSpecifics
{
    public static Action UpdatedExtraPower;
    public static Action UpdatedAmountOfOngoingEffects;

    private int globalAmountOfOngoingEffects = 1;

    public List<int> CantPlaceCommonsThatCost = new List<int>();
    public List<int> CantPlaceCommonsOnRound = new List<int>();
    private int[] extraPower = new int[2]; //0 for my player, 1 for opponent
    private int[] amountOfOngoingEffects = new int[2]; //0 for my player, 1 for opponent
    public int MaxAmountOfQommons = 4;
    public int AmountOfRevealEffects = 1;

    public int GlobalAmountOfOngoingEffects
    {
        get => globalAmountOfOngoingEffects;
        set
        {
            globalAmountOfOngoingEffects = value;
            UpdatedAmountOfOngoingEffects?.Invoke();
        }
    }
    public bool CanRemoveCards = true;

    public int[] ExtraPower => extraPower;

    public void ChangeExtraPower(int _index, int _amount)
    {
        Debug.Log("<color=green>Adding extra lane ability power: "+ _amount);
        extraPower[_index] += _amount;
        UpdatedExtraPower?.Invoke();
    }

    public int GetAmountOfOngoingEffects(bool _forMe)
    {
        int _myAmountOfEffects = _forMe ? amountOfOngoingEffects[0] : amountOfOngoingEffects[1];
        if (globalAmountOfOngoingEffects==0)
        {
            return 0;
        }
        return _myAmountOfEffects + globalAmountOfOngoingEffects;
    }
}
