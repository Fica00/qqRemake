public class LaneAbilityAddPowerToOtherLocations : LaneAbilityBase
{
    private LaneDisplay lane1;
    private LaneDisplay lane2;
    private int[] powerAdded = new int[2];

    public override void Subscribe()
    {
        isSubscribed = true;
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
        if (!isSubscribed)
        {
            return;
        }
        TableHandler.OnRevealdCard -= CheckCard;
        GameplayManager.UpdatedRound -= CalculatePower;
    }

    private void CheckCard(CardObject _cardObject)
    {
        CalculatePower();
    }

    private void CalculatePower()
    {
        CalculatePower(true);
        CalculatePower(false);
    }

    private void CalculatePower(bool _isMy)
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
