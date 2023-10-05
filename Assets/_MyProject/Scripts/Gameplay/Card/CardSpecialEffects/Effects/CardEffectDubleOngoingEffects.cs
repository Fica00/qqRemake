using UnityEngine;

public class CardEffectDubleOngoingEffects :  CardEffectBase
{
    [SerializeField] private HaloRingEffect haloRingEffect;
    
    public override void Subscribe()
    {
        LaneDisplay _currentLane = GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation];
        _currentLane.LaneSpecifics.AmountOfOngoingEffects += 2;
        Instantiate(haloRingEffect, cardObject.transform);
    }
}