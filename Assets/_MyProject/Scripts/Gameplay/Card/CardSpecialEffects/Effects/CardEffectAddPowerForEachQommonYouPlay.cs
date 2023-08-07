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
      LanePlaceIdentifier _placeIdentifier = _card.GetComponentInParent<LanePlaceIdentifier>();
      if (_card.IsMy!=cardObject.IsMy)
      {
         return;
      }

      if (_card==cardObject)
      {
         return;
      }

      cardObject.Stats.Power += powerToAdd;
      GameplayManager.Instance.FlashLocation(_placeIdentifier.Id,Color.white, 3);
   }
}
