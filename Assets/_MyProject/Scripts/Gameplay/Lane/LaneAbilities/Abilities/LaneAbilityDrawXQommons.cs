using System.Collections;
using UnityEngine;

public class LaneAbilityDrawXQommons : LaneAbilityBase
{
    [SerializeField] int amountOfCardsToDraw;

    public override void Subscribe()
    {
        laneDisplay.AbilityShowAsActive();
        for (int i = 0; i < amountOfCardsToDraw; i++)
        {
            GameplayManager.Instance.DrawCard();
        }
        StartCoroutine(DisableAbility());


        IEnumerator DisableAbility()
        {
            yield return new WaitForSeconds(2);
            laneDisplay.AbilityShowAsInactive();
        }
    }
}
