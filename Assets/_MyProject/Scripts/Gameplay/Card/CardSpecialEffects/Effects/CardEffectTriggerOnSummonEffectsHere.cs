using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectTriggerOnSummonEffectsHere : CardEffectBase
{
    [SerializeField] private HaloRingEffect haloRingEffect;
    
    public override void Subscribe()
    {
        for (int i = 0; i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; i++)
        {
            Trigger();
        }
    }

    void Trigger()
    {
        List<CardObject> _myCardsOnLane = GameplayManager.Instance.TableHandler.GetCards(cardObject.IsMy, cardObject.LaneLocation);
        Instantiate(haloRingEffect, cardObject.transform);

        StartCoroutine(TriggerRoutine());
        IEnumerator TriggerRoutine()
        {
            foreach (var _card in _myCardsOnLane)
            {
                CardEffectBase _cardEffect= _card.GetComponentInChildren<CardEffectBase>();
                if (_cardEffect==null)
                {
                    continue;
                }

                if (_card.Details.Description.ToLower().Contains("On Summon".ToLower()))
                {
                    _cardEffect.Subscribe();
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
