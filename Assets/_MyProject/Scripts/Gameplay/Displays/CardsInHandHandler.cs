using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsInHandHandler : MonoBehaviour
{
    List<CardObject> cardsInHand = new List<CardObject>();
    GameplayPlayer player;
    HorizontalLayoutGroup horizontalLayoutGroup;

    private void Awake()
    {
        horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
    }

    public void Setup(GameplayPlayer _player)
    {
        player = _player;

        player.AddedCardToHand += AddCardToHand;
        player.RemovedCardFromHand += RemoveCardFromHand;
    }

    private void OnDisable()
    {
        player.AddedCardToHand -= AddCardToHand;
        player.RemovedCardFromHand -= RemoveCardFromHand;
    }

    void AddCardToHand(CardObject _card)
    {
        cardsInHand.Add(_card);
        _card.transform.SetParent(transform);
        _card.Display.ShowDrawnAnimation();
        CheckForCardSizeChange();
    }

    void RemoveCardFromHand(CardObject _card)
    {
        cardsInHand.Remove(_card);
        CheckForCardSizeChange();
    }

    void CheckForCardSizeChange()
    {
        int _amountOfCardsInHand = cardsInHand.Count;

        // these percentages should be adjusted based on your design
        float maxCardSizePercent = 0.08f; // 8% of the screen width
        float minCardSizePercent = 0.06f; // 6% of the screen width
        float maxSpacingPercent = 0.01f; // 1% of the screen width
        float minSpacingPercent = 0.005f; // 0.5% of the screen width

        // calculate the maximum size and spacing based on screen width
        float maxSize = Screen.width * maxCardSizePercent;
        float maxSpacing = Screen.width * maxSpacingPercent;

        // calculate the total width of the cards and spacing if at maximum size
        float totalWidth = (_amountOfCardsInHand * maxSize) + ((_amountOfCardsInHand - 1) * maxSpacing);

        int _size = 0;
        int _spacing = 0;

        // if the total width is less than or equal to the screen width
        if (totalWidth <= Screen.width)
        {
            // use maximum size and spacing
            _size = Mathf.FloorToInt(maxSize);
            _spacing = Mathf.FloorToInt(maxSpacing);
        }
        else
        {
            // otherwise, calculate a smaller size and spacing
            float minSize = Screen.width * minCardSizePercent;
            float minSpacing = Screen.width * minSpacingPercent;

            // calculate the size and spacing based on the screen width and the number of cards
            float sizeF = (Screen.width - (_amountOfCardsInHand - 1) * minSpacing) / _amountOfCardsInHand;
            _size = Mathf.FloorToInt(sizeF);
            _spacing = Mathf.FloorToInt(minSpacing);
        }

        foreach (var _card in cardsInHand)
        {
            RectTransform _rectTransform = _card.GetComponent<RectTransform>();
            _rectTransform.sizeDelta = new Vector2(_size, _size);
        }

        if (horizontalLayoutGroup != null)
        {
            horizontalLayoutGroup.spacing = _spacing;
        }
    }
}
