using System.Collections.Generic;
using UnityEngine;

public class CardEffectDestroyEnemysNRandomXCostQommonsHere : CardEffectBase
{
   [SerializeField] private int amountOfQommons;
   [SerializeField] private List<int> qommonsCost;

   public override void Subscribe()
   {
      if (!GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].CanRemoveCards())
      {
         return;
      }
      for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; _i++)
      {
         DestroyQommon();
      }
   }

   void DestroyQommon()
   {
      if (GameplayManager.IsPvpGame&& !cardObject.IsMy)
      {
         return;
      }

      List<CardObject> _cardsToBeDestroyed = new List<CardObject>();
      List<CardObject> _oppoentsCardsOnLine = GameplayManager.Instance.TableHandler.GetCards(!cardObject.IsMy, cardObject.LaneLocation);
      for (int _i = 0; _i < amountOfQommons; _i++)
      {
         if (_oppoentsCardsOnLine.Count == 0)
         {
            break;
         }

         CardObject _card=null;
         foreach (var _cardOnLane in _oppoentsCardsOnLine)
         {
            if (qommonsCost.Contains(_cardOnLane.Stats.Energy))
            {
               _card = _cardOnLane;
               break;
            }
         }

         if (_card==null)
         {
            break;
         }
         _oppoentsCardsOnLine.Remove(_card);
         _cardsToBeDestroyed.Add(_card);
      }

      if (GameplayManager.IsPvpGame)
      {
         ((GameplayManagerPvp)GameplayManager.Instance).TellOpponentToDestroyCardsOnTable(_cardsToBeDestroyed,false);
      }
      
      GameplayPlayer _opponentPlayer = cardObject.IsMy ? GameplayManager.Instance.OpponentPlayer : GameplayManager.Instance.MyPlayer;
      foreach (var _card in _cardsToBeDestroyed)
      {
         _opponentPlayer.DestroyCardFromTable(_card);
      }
   }
}
