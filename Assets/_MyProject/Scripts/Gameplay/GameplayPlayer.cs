using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayPlayer : MonoBehaviour
{

    public Action<CardObject> AddedCardToHand;
    public Action<CardObject> RemovedCardFromHand;
    public Action UpdatedEnergy;
    public bool IsMy { get; private set; }

    [SerializeField] CardsInHandHandler cardsInHandHandler = null;
    [SerializeField] EnergyDisplayHandler energyDisplayHandler = null;

    List<int> cardsInDeck;
    List<CardObject> cardsInHand;
    int energy;

    public int AmountOfCardsInHand => cardsInHand.Count;
    public int Energy
    {
        get
        {
            return energy;
        }
        set
        {
            energy = value;
            UpdatedEnergy?.Invoke();
        }
    }

    public void Setup()
    {
        cardsInDeck = new List<int>(DataManager.Instance.PlayerData.CardIdsIndeck);
        cardsInHand = new List<CardObject>();
        if (cardsInHandHandler != null)
        {
            cardsInHandHandler.Setup(this);
        }
        if (energyDisplayHandler != null)
        {
            energyDisplayHandler.Setup(this);
        }
    }

    private void OnEnable()
    {
        GameplayManager.UpdatedRound += SetEnergy;
    }

    private void OnDisable()
    {
        GameplayManager.UpdatedRound -= SetEnergy;
    }

    private void SetEnergy()
    {
        Energy = GameplayManager.Instance.CurrentRound;
    }

    public CardObject DrawCard()
    {
        int _randomIndex = UnityEngine.Random.Range(0, cardsInDeck.Count);
        int _cardId = cardsInDeck[_randomIndex];
        return DrawCard(_cardId, true);
    }

    public CardObject DrawCard(int _id, bool _updateDeck)
    {
        if (_updateDeck)
        {
            if (cardsInDeck.Contains(_id))
            {
                cardsInDeck.Remove(_id);
            }
        }

        CardObject _card = CardsManager.Instance.CreateCard(_id, IsMy);
        return _card;
    }

    public void AddCardToHand(CardObject _cardObject)
    {
        cardsInHand.Add(_cardObject);
        _cardObject.SetCardLocation(CardLocation.Hand);
        AddedCardToHand?.Invoke(_cardObject);
    }

    public void RemoveCardFromHand(CardObject _cardObject)
    {
        cardsInHand.Remove(_cardObject);
        RemovedCardFromHand?.Invoke(_cardObject);
    }

    public void CheckForCardsThatShouldMoveToHand(Action _callback)
    {
        StartCoroutine(CheckForCardsThatShouldMoveToHandRoutine());

        IEnumerator CheckForCardsThatShouldMoveToHandRoutine()
        {
            List<int> _idsOfCardsThatShouldBeAddedToHand = new List<int>();
            foreach (var _card in cardsInDeck)
            {
                //go thue special effects and
                //check if card should be added this round to the hand;
            }

            foreach (var _id in _idsOfCardsThatShouldBeAddedToHand)
            {
                DrawCard(_id, true);
                yield return new WaitForSeconds(0.3f);
            }

            _callback?.Invoke();
        }
    }
}
