using UnityEngine;

public class LaneAbilityDrawXQommons : LaneAbilityBase
{
    [SerializeField] private int amountOfCardsToDraw;
    
    public override void Subscribe()
    {
        isSubscribed = true;
        for (int i = 0; i < amountOfCardsToDraw; i++)
        {
            GameplayManager.Instance.DrawCard();
        }
    }
}
