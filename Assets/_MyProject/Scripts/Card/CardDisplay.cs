using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] CardInHandDisplay cardInHandDisplay;
    [SerializeField] CardOnTableDisplay cardOnTableDisplay;

    public void Setup(CardObject _cardObject)
    {
        cardInHandDisplay.Setup(_cardObject);
        cardOnTableDisplay.Setup(_cardObject);
    }

    public void ShowDrawnAnimation()
    {
        ShowCardInHand();
        cardInHandDisplay.ShowDrawnAnimation();
    }

    public void ShowCardInHand()
    {
        cardInHandDisplay.Show();
        cardOnTableDisplay.Hide();
    }

    public void ShowCardOnTable()
    {
        cardOnTableDisplay.Show();
        cardInHandDisplay.Hide();
    }

    public void HideCardOnTable()
    {
        cardOnTableDisplay.Hide();
    }
}
