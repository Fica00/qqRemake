public class CardEffectDisableAddingAndRemovingCards : CardEffectBase
{
    public override void Subscribe()
    {
        LaneDisplay _currentLane = GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation];
        for (int i = 0; i < 8; i++)
        {
            _currentLane.LaneSpecifics.CantPlaceCommonsOnRound.Add(i);
        }

        _currentLane.LaneSpecifics.CanRemoveCards = false;
        GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].Visualizator.ShowWholeLanePurple();
    }
}
