using UnityEngine;

public class LaneAbilityDrawXQommons : LaneAbilityBase
{
    [SerializeField] int amountOfCardsToDraw;

    public override void Subscribe()
    {
        for (int i = 0; i < amountOfCardsToDraw; i++)
        {
            GameplayManager.Instance.DrawCard();
        }
    }
}
