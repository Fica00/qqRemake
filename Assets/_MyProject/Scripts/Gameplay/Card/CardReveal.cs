using System.Collections;
using UnityEngine;

public class CardReveal : MonoBehaviour
{
    [SerializeField] private GameObject shadowObject;
    [SerializeField] private GameObject revealObject;
    private CardObject cardObject;
    public bool IsReveled;

    public bool IsRevealing => shadowObject.activeSelf||revealObject.activeSelf;

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

    public void PreReveal()
    {
        cardObject.CanChangePlace = false;
        CancelReveal();
    }

    public IEnumerator Reveal()
    {
        PreReveal();
        revealObject.SetActive(true);
        yield return new WaitForSeconds(1f);//duration of reveal animation
        revealObject.SetActive(false);
        Finish();
    }

    public void Finish()
    {
        IsReveled = true;
        cardObject.Display.ShowCardOnTable();
    }
}
