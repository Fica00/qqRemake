public class CardEffectDisableOnSummon : CardEffectBase
{
    public override void Subscribe()
    {
        LaneDisplay _currentLane = GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation];
        _currentLane.LaneSpecifics.AmountOfRevealEffects = 0;
    }
}
