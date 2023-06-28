using UnityEngine;
using UnityEngine.UI;

public class GeishaKoYellowCircle : MonoBehaviour
{
    private Image image;
    private CardObject cardObject;

    private void Awake()
    {
        image = GetComponent<Image>();
        cardObject = GetComponentInParent<CardObject>();
    }

    private void OnEnable()
    {
        TableHandler.OnRevealdCard += CheckRevealedCard;
    }

    private void OnDisable()
    {
        TableHandler.OnRevealdCard -= CheckRevealedCard;
    }

    private void CheckRevealedCard(CardObject _cardObject)
    {
        if (_cardObject==cardObject)
        {
            Color _color = image.color;
            _color.a = 1;
            image.color = _color;
        }
    }
}
