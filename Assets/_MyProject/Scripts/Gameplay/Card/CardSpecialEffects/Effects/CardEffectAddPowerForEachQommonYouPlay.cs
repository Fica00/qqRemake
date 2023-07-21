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
      LanePlaceIdentifier _placeIdentifier = cardObject.GetComponentInParent<LanePlaceIdentifier>();
      if (_card.LaneLocation!=cardObject.LaneLocation)
      {
         return;
      }

      if (_card==cardObject)
      {
         return;
      }

      for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfOngoingEffects; _i++)
      {
         _card.Stats.Power += powerToAdd;
      }

      if (GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfOngoingEffects>0)
      {
         GameplayManager.Instance.FlashLocation(_placeIdentifier.Id,Color.white, 3);
      }
   }
}
