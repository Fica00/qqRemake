using UnityEngine;

public class CardEffectAddCardToHandOnRound : CardEffectBase
{
    [SerializeField] private int round;
    public int Round => round;

    public override void Subscribe()
    {
        //nothing to do here :)
    }
}
