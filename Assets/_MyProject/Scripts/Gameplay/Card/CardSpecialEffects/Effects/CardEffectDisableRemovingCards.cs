public class CardEffectDisableRemovingCards : CardEffectBase
{
   public override void Subscribe()
   {
      LaneDisplay _currentLane = GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation];

      _currentLane.LaneSpecifics.CanRemoveCards = false;
      GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].Visualizator.ShowWholeLaneBlue();
   }
}
