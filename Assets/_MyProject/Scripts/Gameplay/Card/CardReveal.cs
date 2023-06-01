using System;
using System.Collections;
using UnityEngine;

public class CardReveal : MonoBehaviour
{
    public static Action<CardObject> ShowRevealCard;
    [SerializeField] GameObject shadowObject;
    [SerializeField] GameObject revealObject;
    CardObject cardObject;

    public void Setup(CardObject _cardObject)
    {
        cardObject = _cardObject;
    }


    public void PrepareForReveal()
    {
        shadowObject.SetActive(true);
    }

    public void CancelReveal()
    {
        shadowObject.SetActive(false);
    }

    public IEnumerator Reveal()
    {
        cardObject.CanChangePlace = false;
        CancelReveal();
        revealObject.SetActive(true);
        yield return new WaitForSeconds(2f);//duration of reveal animation
        revealObject.SetActive(false);
        cardObject.Display.ShowCardOnTable();
        ShowRevealCard?.Invoke(cardObject);
        yield return new WaitForSeconds(2.5f);//duration of showing reveal card
    }
}
