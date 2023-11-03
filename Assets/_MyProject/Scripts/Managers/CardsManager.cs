using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    public static CardsManager Instance;

    private List<CardObject> allCards;

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
        CardObject _desiredCard = GetCardObject(_cardId);
        CardObject _cardObject = Instantiate(_desiredCard);
        _cardObject.Setup(_isMy);
        return _cardObject;
    }

    public List<CardEffectBase> GetCardEffects(int _cardId)
    {
        CardObject _desiredCard = GetCardObject(_cardId);
        return _desiredCard.SpecialEffects;
    }

    public CardObject GetCardObject(int _cardId)
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

        return _desiredCard;
    }

    public Sprite GetCardSprite(int _cardId)
    {
        return allCards.Find(_elemet => _elemet.Details.Id == _cardId).Details.Sprite;
    }
    
    public Sprite GetCardSpriteGamePass(int _cardId)
    {
        return allCards.Find(_element => _element.Details.Id == _cardId).Details.SpriteGamePass;
    }
}
