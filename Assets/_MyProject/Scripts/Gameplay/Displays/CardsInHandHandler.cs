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
        int _size = 0;
        int _spacing = 0;

        if (_amountOfCardsInHand >= GameplayManager.Instance.MaxAmountOfCardsInHand)
        {
            _size = 154;
            _spacing = 8;
        }
        else
        {
            _size = 174;
            _spacing = 12;
        }

        foreach (var _card in cardsInHand)
        {
            RectTransform _rectTransform = _card.GetComponent<RectTransform>();
            _rectTransform.localScale = Vector3.one;
            _rectTransform.sizeDelta = new Vector2(_size, _size);
        }

        if (horizontalLayoutGroup != null)
        {
            horizontalLayoutGroup.spacing = _spacing;
        }
    }
}
