public class CardEffectDisableOngoingEffects : CardEffectBase
{
    public override void Subscribe()
    {
        LaneDisplay _currentLane = GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation];
        _currentLane.LaneSpecifics.GlobalAmountOfOngoingEffects = 0;
    }
}
