using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardReveal : MonoBehaviour
{
    [SerializeField] private GameObject shadowObject;
    [SerializeField] private GameObject revealObject;
    [SerializeField] private Image revealImage;
    private CardObject cardObject;
    public bool IsReveled;

    public bool IsRevealing => shadowObject.activeSelf||revealObject.activeSelf;

    public void Setup(CardObject _cardObject)
    {
        cardObject = _cardObject;
        revealImage.sprite = cardObject.Details.Sprite;
        float _rotationY = _cardObject.IsMy ? 0 : 180;
        transform.eulerAngles = new Vector3(0, _rotationY, 0);
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
