using UnityEngine;

public class CardEffectAddPowerForEachQommonYouPlay : CardEffectBase
{
   [SerializeField] private int powerToAdd;
   
   public override void Subscribe()
   {
      TableHandler.OnRevealdCard += CheckCard;
      isSubscribed = true;
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
      LanePlaceIdentifier _placeIdentifier = cardObject.GetComponentInParent<LanePlaceIdentifier>();
      if (_card.IsMy!=cardObject.IsMy)
      {
         return;
      }

      if (_card==cardObject)
      {
         return;
      }

      int _powerToAdd = 0;
      for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfOngoingEffects; _i++)
      {
         _powerToAdd += powerToAdd;
      }
      cardObject.Stats.Power += _powerToAdd;
      
      GameplayManager.Instance.FlashLocation(_placeIdentifier.Id,Color.white, 3);
   }
}
