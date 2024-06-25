using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardEffectAddPowerToNQommonsOnYourField : CardEffectBase
{
    [SerializeField] private int powerToAdd;
    [SerializeField] private int amountOfQommons;

    public override void Subscribe()
    {
        for (int i = 0; i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; i++)
        {
            AddPower();
        }
    }

    void AddPower()
    {
        if (GameplayManager.IsPvpGame&& !cardObject.IsMy)
        {
            return;
        }

        List<CardObject> _availableCards = new List<CardObject>();
        
        CheckForAvailableCards(GameplayManager.Instance.TableHandler.GetCards(cardObject.IsMy, LaneLocation.Top));
        CheckForAvailableCards(GameplayManager.Instance.TableHandler.GetCards(cardObject.IsMy, LaneLocation.Mid));
        CheckForAvailableCards(GameplayManager.Instance.TableHandler.GetCards(cardObject.IsMy, LaneLocation.Bot));

        _availableCards = _availableCards.OrderBy(_element => Guid.NewGuid()).ToList();
        List<CardObject> _effectedCards = new List<CardObject>();

        for (int _i = 0; _i < amountOfQommons; _i++)
        {
            if (_i>=_availableCards.Count)
            {
                break;
            }
            if (_effectedCards.Count>=amountOfQommons)
            {
                break;
            }
            
            _effectedCards.Add(_availableCards[_i]);
            _availableCards[_i].Stats.Power += powerToAdd;
            LanePlaceIdentifier _identifier = _availableCards[_i].GetComponentInParent<LanePlaceIdentifier>();
            GameplayManager.Instance.FlashLocation(_identifier.Id,Color.white, 3);
        }
        
        if (GameplayManager.IsPvpGame)
        {
            (GameplayManagerPvp.Instance as GameplayManagerPvp).TellOpponentToAddPowerToQommons(_effectedCards,powerToAdd);
        }


        void CheckForAvailableCards(List<CardObject> _cards)
        {
            foreach (var _card in _cards)
            {
                if (_card==cardObject)
                {
                    continue;
                }

                _availableCards.Add(_card);
            }
        }
    }
}
