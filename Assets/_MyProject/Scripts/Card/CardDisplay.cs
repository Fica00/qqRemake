using UnityEngine;
using TMPro;


public class CardDisplay : MonoBehaviour
{
    [SerializeField] CardInHandDisplay cardInHandDisplay;

    public void Setup(CardObject _cardObject)
    {
        cardInHandDisplay.Setup(_cardObject);
    }

    public void ShowDrawnAnimation()
    {
        ShowCardInHand();
        cardInHandDisplay.ShowDrawnAnimation();
    }

    public void ShowCardInHand()
    {
        cardInHandDisplay.Show();
    }

}
