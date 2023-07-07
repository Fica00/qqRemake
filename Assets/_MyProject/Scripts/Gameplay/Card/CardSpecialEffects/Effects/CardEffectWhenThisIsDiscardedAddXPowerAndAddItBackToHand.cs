using System;
using System.Collections;
using UnityEngine;

public class CardEffectWhenThisIsDiscardedAddXPowerAndAddItBackToHand : CardEffectBase
{
    [SerializeField] private int power;
    [SerializeField] private GameObject animation;

    public override void Subscribe()
    {
        //nothing to do here
    }

    private void OnEnable()
    {
        GameplayPlayer.DiscardedCard += CheckDiscardedCard;
    }

    private void OnDisable()
    {
        GameplayPlayer.DiscardedCard -= CheckDiscardedCard;
    }

    void CheckDiscardedCard(CardObject _card)
    {
        if (GameplayManager.IsPvpGame&&!_card.IsMy)
        {
            return;
        }
        
        if (_card==cardObject)
        {
            StartCoroutine(ApplyRoutine());
        }
    }

    IEnumerator ApplyRoutine()
    {
        yield return new WaitForSeconds(0.9f);
        cardObject.Display.HideCardInHand();
        GameplayPlayer _player =
            cardObject.IsMy ? GameplayManager.Instance.MyPlayer : GameplayManager.Instance.OpponentPlayer;
        
        _player.AddCardToHand(cardObject,false);
        cardObject.Display.HideCardInHand();
        animation.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        cardObject.Display.ShowCardInHand();
        cardObject.Display.ShowDrawnAnimation();
        yield return new WaitForSeconds(0.3f);
        cardObject.Stats.Power += power;
    }
    
    
}
