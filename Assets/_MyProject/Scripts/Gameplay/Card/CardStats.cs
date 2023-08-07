using System;
using UnityEngine;

public class CardStats
{
    private int power;
    private int mana;
    private int chagePowerDueToLocation;

    //if bool is false means that the power decreased,if it is true, power increased
    public Action<ChangeStatus> UpdatedPower;
    public Action<ChangeStatus> UpdatedMana;
    private int startingPower;
    private int startingMana;
    
    public void Setup()
    {
        startingPower = power;
        startingMana = mana;
    }

    public int Power
    {
        get
        {
            return power;
        }
        set
        {
            power = value;

            if (power == startingPower)
            {
                UpdatedPower?.Invoke(ChangeStatus.Same);
            }
            else if (power > startingPower)
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
            mana = value;

            if (mana == startingMana)
            {
                UpdatedMana?.Invoke(ChangeStatus.Same);
            }
            else if (mana > startingMana)
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
            int _oldChangePower = chagePowerDueToLocation;
            chagePowerDueToLocation = value;
            if (_oldChangePower== chagePowerDueToLocation)
            {
                UpdatedPower?.Invoke(ChangeStatus.Same);
            }
            else if (chagePowerDueToLocation >_oldChangePower)
            {
                UpdatedPower?.Invoke(ChangeStatus.Increased);
            }
            else
            {
                UpdatedPower?.Invoke(ChangeStatus.Decreased);
            }
        }
    }
}
