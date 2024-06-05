using System;
using System.Collections.Generic;
using UnityEngine;

public class LaneSpecifics
{
    public static Action UpdatedExtraPower;

    public List<int> CantPlaceCommonsThatCost = new List<int>();
    public List<int> CantPlaceCommonsOnRound = new List<int>();
    private int[] extraPower = new int[2]; //0 for my player, 1 for opponent
    public int MaxAmountOfQommons = 4;
    public int AmountOfRevealEffects = 1;
    public int AmountOfOngoingEffects = 1;
    public bool CanRemoveCards = true;

    public int[] ExtraPower => extraPower;

    public void ChangeExtraPower(int _index, int _amount)
    {
        Debug.Log("<color=green>Adding extra lane ability power: "+ _amount);
        extraPower[_index] += _amount;
        UpdatedExtraPower?.Invoke();
    }
}
