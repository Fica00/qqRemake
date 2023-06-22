public class LaneAbilityAddPowerToOtherLocations : LaneAbilityBase
{
    LaneDisplay lane1;
    LaneDisplay lane2;
    int[] powerAdded = new int[2];

    public override void Subscribe()
    {
        switch (laneDisplay.Location)
        {
            case LaneLocation.Top:
                lane1 = GameplayManager.Instance.Lanes[1];
                lane2 = GameplayManager.Instance.Lanes[2];
                break;
            case LaneLocation.Mid:
                lane1 = GameplayManager.Instance.Lanes[0];
                lane2 = GameplayManager.Instance.Lanes[2];
                break;
            case LaneLocation.Bot:
                lane1 = GameplayManager.Instance.Lanes[0];
                lane2 = GameplayManager.Instance.Lanes[1];
                break;
            default:
                throw new System.Exception("Don`t know what are the other lines for location: " + laneDisplay.Location);
        }
        laneDisplay.AbilityShowAsActive();
        for (int i = 0; i < 2; i++)
        {
            powerAdded[i] = default;
        }
        TableHandler.OnRevealdCard += CheckCard;
        GameplayManager.UpdatedRound += CalculatePower;
        CalculatePower();
    }

    private void OnDisable()
    {
        TableHandler.OnRevealdCard -= CheckCard;
        GameplayManager.UpdatedRound -= CalculatePower;
    }

    void CheckCard(CardObject _cardObject)
    {
        CalculatePower();
    }

    void CalculatePower()
    {
        CalculatePower(true);
        CalculatePower(false);
    }

    void CalculatePower(bool _isMy)
    {
        int _index = _isMy ? 0 : 1;
        if (powerAdded != default)
        {
            lane1.LaneSpecifics.ChangeExtraPower(_index, -powerAdded[_index]);
            lane2.LaneSpecifics.ChangeExtraPower(_index, -powerAdded[_index]);
        }

        powerAdded[_index] = GameplayManager.Instance.TableHandler.GetPower(_isMy, laneDisplay.Location);

        lane1.LaneSpecifics.ChangeExtraPower(_index, powerAdded[_index]);
        lane2.LaneSpecifics.ChangeExtraPower(_index, powerAdded[_index]);
    }
}
