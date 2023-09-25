using UnityEngine;

public class CardEffectDrawXCardsFromOpponentsDeck : CardEffectBase
{
   [SerializeField] private int amountOfCards;
   public override void Subscribe()
   {
      if (GameplayManager.IsPvpGame && !cardObject.IsMy)
      {
         return;
      }
      
      for (int i = 0; i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; i++)
      {
         for (int j = 0; j < amountOfCards; j++)
         {
            GameplayManager.Instance.DrawCardFromOpponentsDeck(cardObject.IsMy);
         }
      }
   }
}
