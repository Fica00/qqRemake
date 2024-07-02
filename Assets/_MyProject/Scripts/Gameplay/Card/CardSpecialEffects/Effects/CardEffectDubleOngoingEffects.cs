using UnityEngine;

public class CardEffectDubleOngoingEffects :  CardEffectBase
{
    [SerializeField] private HaloRingEffect haloRingEffect;
    
    public override void Subscribe()
    {
        LaneDisplay _currentLane = GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation];
        _currentLane.LaneSpecifics.AmountOfOngoingEffects *= 2;
        GameObject _effect = Instantiate(haloRingEffect, cardObject.transform).gameObject;
        _effect.transform.position += new Vector3(0, 100, 0);
    }
}