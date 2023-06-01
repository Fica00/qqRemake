using System;
using System.Collections.Generic;

public class LaneSpecifics
{
    public static Action UpdatedExtraPower;

    public List<int> CantPlaceCommonsThatCost = new List<int>();
    public List<int> CantPlaceCommonsOnRound = new List<int>();
    int[] extraPower = new int[2]; //0 for my player, 1 for opponent

    public int[] ExtraPower => extraPower;

    public void ChangeExtraPower(int _index, int _amount)
    {
        extraPower[_index] += _amount;
        UpdatedExtraPower?.Invoke();
    }
}
