using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    public static CardsManager Instance;

    List<CardObject> allCards;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            allCards = Resources.LoadAll<CardObject>("Cards/").ToList();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public CardObject CreateCard(int _cardId, bool _isMy)
    {
        CardObject _desiredCard = null;
        foreach (var _card in allCards)
        {
            if (_card.Details.Id == _cardId)
            {
                _desiredCard = _card;
                break;
            }
        }

        if (_desiredCard == null)
        {
            throw new System.Exception("Cant find prefab for card with id: " + _cardId);
        }

        CardObject _cardObject = Instantiate(_desiredCard);
        _cardObject.Setup(_isMy);
        return _cardObject;
    }
}
