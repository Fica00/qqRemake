using UnityEngine;
using UnityEngine.UI;

public class GeishaKoYellowCircle : MonoBehaviour
{
    Image image;
    CardObject cardObject;

    private void Awake()
    {
        image = GetComponent<Image>();
        cardObject = GetComponent<CardObject>();
    }

    private void OnEnable()
    {
        TableHandler.OnRevealdCard += CheckRevealedCard;
    }

    private void OnDisable()
    {
        TableHandler.OnRevealdCard -= CheckRevealedCard;
    }

    void CheckRevealedCard(CardObject _cardObject)
    {
        if (_cardObject==cardObject)
        {
            Color _color = image.color;
            _color.a = 1;
            image.color = _color;
        }
    }
}
