using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectAddPowerForEachCardHere : CardEffectBase
{
    [SerializeField] private int powerToAdd;
    public override void Subscribe()
    {
        for (int i = 0; i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; i++)
        {
            AddPower();
        }
    }

    void AddPower()
    {
        
    }
}
