using System;
using UnityEngine;

public class CardStats
{
    int power;
    int mana;
    int chagePowerDueToLocation;

    //if bool is false means that the power decreased,if it is true, power increased
    public Action<ChangeStatus> UpdatedPower;
    public Action<ChangeStatus> UpdatedMana;

    public int Power
    {
        get
        {
            return power;
        }
        set
        {
            int _oldPower = power;
            power = value;

            if (power == _oldPower)
            {
                UpdatedPower?.Invoke(ChangeStatus.Same);
            }
            else if (power > _oldPower)
            {
                UpdatedPower?.Invoke(ChangeStatus.Increased);
            }
            else
            {
                UpdatedPower?.Invoke(ChangeStatus.Decreased);
            }
        }
    }

    public int Energy
    {
        get
        {
            return mana;
        }
        set
        {
            int _oldMana = mana;
            mana = value;

            if (mana == _oldMana)
            {
                UpdatedMana?.Invoke(ChangeStatus.Same);
            }
            else if (mana > _oldMana)
            {
                UpdatedMana?.Invoke(ChangeStatus.Increased);
            }
            else
            {
                UpdatedMana?.Invoke(ChangeStatus.Decreased);
            }
        }
    }

   [HideInInspector] public int ChagePowerDueToLocation
    {
        get
        {
            return chagePowerDueToLocation;
        }
        set
        {
            chagePowerDueToLocation = value;
        }
    }
}
