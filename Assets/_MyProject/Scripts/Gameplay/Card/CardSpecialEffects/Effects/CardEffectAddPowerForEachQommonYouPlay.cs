using UnityEngine;

public class CardEffectAddPowerForEachQommonYouPlay : CardEffectBase
{
   [SerializeField] private int powerToAdd;
   
   public override void Subscribe()
   {
      TableHandler.OnRevealdCard += CheckCard;
   }

   private void CheckCard(CardObject _card)
   {
      if (_card.LaneLocation!=cardObject.LaneLocation)
      {
         return;
      }

      for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfOngoingEffects; _i++)
      {
         _card.Stats.Power += powerToAdd;
      }
   }
}
