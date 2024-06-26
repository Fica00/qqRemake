using UnityEngine;

public class LaneAbilityAddXPowerHereToQoomonsWithNoAbility : LaneAbilityBase
{
    [SerializeField] private int powerToAdd;

    public override void Subscribe()
    {
        isSubscribed = true;
        TableHandler.OnRevealdCard += CheckCard;
    }

    private void OnDisable()
    {
        if (!isSubscribed)
        {
            return;
        }
        TableHandler.OnRevealdCard -= CheckCard;
    }

    private void CheckCard(CardObject _card)
    {
        if (_card.LaneLocation != laneDisplay.Location)
        {
            return;
        }

        if (_card.Details.Description.Length == 0)
            _card.Stats.Power += powerToAdd;
    }
}
