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
        float _sizeRatio = Screen.width / 1179f; // Calculate the ratio based on the original width
        float _spacingRatio = Screen.width / 2556f; // Calculate the ratio based on the original height
        int _size = 0;
        int _spacing = 0;

        if (_amountOfCardsInHand >= GameplayManager.Instance.MaxAmountOfCardsInHand)
        {
            _size = Mathf.RoundToInt(154 * _sizeRatio);
            _spacing = Mathf.RoundToInt(8 * _spacingRatio);
        }
        else
        {
            _size = Mathf.RoundToInt(174 * _sizeRatio);
            _spacing = Mathf.RoundToInt(12 * _spacingRatio);
        }

        foreach (var _card in cardsInHand)
        {
            RectTransform _rectTransform = _card.GetComponent<RectTransform>();
            _rectTransform.sizeDelta = new Vector2(_size, _size);
            _card.transform.localScale = Vector3.one;
        }

        if (horizontalLayoutGroup != null)
        {
            horizontalLayoutGroup.spacing = _spacing;
        }
    }
}