using UnityEngine;

public class LaneAbilityOnTurnXAllCardsMustBePlayedHere : LaneAbilityBase
{
    [SerializeField] private int round;

    public override void Subscribe()
    {
        isSubscribed = true;
        GameplayManager.UpdatedRound += CheckRound;
    }

    private void OnDisable()
    {
        if (!isSubscribed)
        {
            return;
        }
        GameplayManager.UpdatedRound -= CheckRound;
    }

    private void CheckRound()
    {
        if (GameplayManager.Instance.CurrentRound==round)
        {
            laneDisplay.AbilityShowAsActive();
            ChangePlacingRules(true);
        }
        else if(GameplayManager.Instance.CurrentRound>round)
        {
            laneDisplay.AbilityShowAsInactive();
            ChangePlacingRules(false);
            isSubscribed = false;
            GameplayManager.UpdatedRound -= CheckRound;
        }
    }

    private void ChangePlacingRules(bool _add)
    {
        LaneDisplay _lane1 = null;
        LaneDisplay _lane2 = null;

        switch (laneDisplay.Location)
        {
            case LaneLocation.Top:
                _lane1 = GameplayManager.Instance.Lanes[1];
                _lane2 = GameplayManager.Instance.Lanes[2];
                break;
            case LaneLocation.Mid:
                _lane1 = GameplayManager.Instance.Lanes[0];
                _lane2 = GameplayManager.Instance.Lanes[2];
                break;
            case LaneLocation.Bot:
                _lane1 = GameplayManager.Instance.Lanes[0];
                _lane2 = GameplayManager.Instance.Lanes[1];
                break;
        }

        if (_add)
        {
            _lane1.LaneSpecifics.CantPlaceCommonsOnRound.Add(round);
            _lane2.LaneSpecifics.CantPlaceCommonsOnRound.Add(round);
        }
        else
        {
            _lane1.LaneSpecifics.CantPlaceCommonsOnRound.Remove(round);
            _lane2.LaneSpecifics.CantPlaceCommonsOnRound.Remove(round);
        }

       
    }
}
