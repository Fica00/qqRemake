using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardEffectAddNPowerToXCards : CardEffectBase
{
   [SerializeField] private int powerToAdd;
   [SerializeField] private int amountOfCards;
   
   public override void Subscribe()
   {
      if (GameplayManager.IsPvpGame&&!cardObject.IsMy)
      {
         return;
      }
      
      GameplayPlayer _player = cardObject.IsMy ? GameplayManager.Instance.MyPlayer : GameplayManager.Instance.OpponentPlayer;
      List<CardObject> _myCards = GameplayManager.Instance.TableHandler.GetCards(_player).ToList().OrderBy(_element=>Guid.NewGuid()).ToList();
      List<CardObject> _chosenCards = new List<CardObject>();
      for (int _i = 0; _i < amountOfCards; _i++)
      {
         if (_myCards.Count==0)
         {
            break;
         }
         _chosenCards.Add(_myCards[0]);
         _myCards.RemoveAt(0);
      }

      foreach (var _card in _chosenCards)
      {
         for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfOngoingEffects; _i++)
         {
            _card.Stats.Power += powerToAdd;
         }
      }

      if (GameplayManager.IsPvpGame)
      {
         (GameplayManager.Instance as GameplayManagerPVP)?.TellOpponentToAddPowerToQommons(_chosenCards,powerToAdd*GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfOngoingEffects);
      }
   }
}
