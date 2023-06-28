using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayPlayer : MonoBehaviour
{
    public static Action<PlaceCommand> AddedCardToTable;
    public static Action<PlaceCommand> RemovedCardFromTable;
    public static Action<CardObject> DiscardedCard;
    public static Action<CardObject> DestroyedCardFromTable;
    public Action<CardObject> AddedCardToHand;
    public Action<CardObject> RemovedCardFromHand;
    public Action UpdatedEnergy;

    [field: SerializeField] public bool IsMy { get; private set; }

    [SerializeField] private CardsInHandHandler cardsInHandHandler = null;
    [SerializeField] private EnergyDisplayHandler energyDisplayHandler = null;

    protected List<CardObject> cardsInDeck;
    protected List<CardObject> cardsInHand;
    protected List<CardObject> cardsInDiscardPile;
    protected int energy;

    public List<CardObject> CardsOnTop;
    public List<CardObject> CardsOnMid;
    public List<CardObject> CardsOnBot;

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

    public virtual void Setup()
    {
        List<int> _cardsInDeck = new List<int>(DataManager.Instance.PlayerData.CardIdsIndeck);
        cardsInDeck = new List<CardObject>();
        foreach (var _cardInDeck in _cardsInDeck)
        {
            CardObject _cardObject = CardsManager.Instance.CreateCard(_cardInDeck, IsMy);
            _cardObject.transform.SetParent(transform);
            cardsInDeck.Add(_cardObject);
        }
        ShuffleDeck();
        cardsInHand = new List<CardObject>();
        cardsInDiscardPile = new List<CardObject>();
        CardsOnTop = new List<CardObject>();
        CardsOnMid = new List<CardObject>();
        CardsOnBot = new List<CardObject>();
        cardsInHandHandler.Setup(this);
        energyDisplayHandler.Setup(this);
    }

    protected void ShuffleDeck()
    {
        cardsInDeck = cardsInDeck.OrderBy(element => Guid.NewGuid()).ToList();

        //check for cards that should get in hand in certain runds and send them on bottom of the deck
        for (int i = cardsInDeck.Count - 1; i >= 0; i--)
        {
            var _specialEffects = CardsManager.Instance.GetCardEffects(cardsInDeck[i].Details.Id);
            foreach (var _specialEffect in _specialEffects)
            {
                if (_specialEffect is CardEffectAddCardToHandOnRound)
                {
                    var _card = cardsInDeck[i];
                    cardsInDeck.RemoveAt(i);
                    cardsInDeck.Add(_card);
                }
            }
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
        if (cardsInDeck.Count==0)
        {
            return null;
        }
        CardObject _card = cardsInDeck[0];
        return DrawCard(_card, true);
    }

    public CardObject DrawCard(CardObject _card, bool _updateDeck)
    {
        if (_updateDeck)
        {
            if (cardsInDeck.Contains(_card))
            {
                cardsInDeck.Remove(_card);
            }
        }

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
            List<CardObject> _cardsThatShouldStartInHand = new List<CardObject>();
            foreach (var _cardId in cardsInDeck)
            {
                var _specialEffects = CardsManager.Instance.GetCardEffects(_cardId.Details.Id);
                foreach (var _specialEffect in _specialEffects)
                {
                    if (!(_specialEffect is CardEffectAddCardToHandOnRound))
                    {
                        continue;
                    }

                    CardEffectAddCardToHandOnRound _addCardEffect = _specialEffect as CardEffectAddCardToHandOnRound;
                    if (_addCardEffect.Round == GameplayManager.Instance.CurrentRound)
                    {
                        _cardsThatShouldStartInHand.Add(_cardId);
                    }
                }
            }

            foreach (var _card in _cardsThatShouldStartInHand)
            {
                CardObject _drawnCard = DrawCard(_card, true);
                AddCardToHand(_drawnCard);
                yield return new WaitForSeconds(0.3f);
            }

            _callback?.Invoke();
        }
    }

    public void AddCardToTable(PlaceCommand _command)
    {
        CardObject _card = _command.Card;
        switch (_command.Location)
        {
            case LaneLocation.Top:
                CardsOnTop.Add(_card);
                break;
            case LaneLocation.Mid:
                CardsOnMid.Add(_card);
                break;
            case LaneLocation.Bot:
                CardsOnBot.Add(_card);
                break;
            default:
                throw new Exception("Cant handle Location: " + _command.Location);
        }
        _card.SetCardLocation(CardLocation.Table);
        AddedCardToTable?.Invoke(_command);
    }

    public void RemoveCardFromTable(PlaceCommand _command)
    {
        CardObject _card = _command.Card;
        RemoveCardFromTable(_card);
    }

    public void RemoveCardFromTable(CardObject _card)
    {
        switch (_card.LaneLocation)
        {
            case LaneLocation.Top:
                CardsOnTop.Remove(_card);
                break;
            case LaneLocation.Mid:
                CardsOnMid.Remove(_card);
                break;
            case LaneLocation.Bot:
                CardsOnBot.Remove(_card);
                break;
            default:
                throw new Exception("Cant handle Location: " + _card.LaneLocation);
        }
    }

    public void CancelCommand(CardObject _cardObject)
    {
        PlaceCommand _command = GameplayManager.Instance.CommandsHandler.GetCommand(_cardObject);
        CancelCommand(_command);
    }

    public void CancelAllCommands()
    {
        List<PlaceCommand> _commands = GameplayManager.Instance.CommandsHandler.MyCommands;
        foreach (var _command in _commands.ToList())
        {
            CancelCommand(_command);
        }
    }

    private void CancelCommand(PlaceCommand _command)
    {
        if (_command.Card.IsForcedToBePlaced)
        {
            return;
        }
        Energy += _command.Card.Stats.Energy;
        AddCardToHand(_command.Card);
        RemoveCardFromTable(_command);
        RemovedCardFromTable?.Invoke(_command);
        _command.Card.GetComponent<CardInteractions>().CanDrag = true;
    }

    public void UpdateQommonCost(int _amount)
    {
        foreach (var _card in cardsInHand)
        {
            _card.Stats.Energy += _amount;
        }

        foreach (var _card in cardsInDeck)
        {
            _card.Stats.Energy += _amount;
        }
    }

    public CardObject GetQommonFromHand()
    {
        if (cardsInHand.Count > 0)
        {
            return cardsInHand[UnityEngine.Random.Range(0, cardsInHand.Count)];
        }

        return null;
    }
    
    public CardObject GetHigestCostQommon()
    {
        if (cardsInHand.Count == 0)
        {
            return null;
        }
        
        CardObject _highestCostQommon = cardsInHand[0];

        foreach (var _qommonInHand in cardsInHand)
        {
            if (_qommonInHand.Stats.Energy>_highestCostQommon.Stats.Energy)
            {
                _highestCostQommon = _qommonInHand;
            }
        }

        return _highestCostQommon;
    }

    public void ReturnCardsToDeck()
    {
        foreach (var _cardInHand in cardsInHand.ToList())
        {
            RemoveCardFromHand(_cardInHand);
            cardsInDeck.Add(_cardInHand);
            _cardInHand.transform.SetParent(transform);
        }
    }

    public void DiscardCardFromHand(CardObject _card)
    {
        _card.transform.SetParent(null);
        RemoveCardFromHand(_card);
        cardsInDiscardPile.Add(_card);
        _card.SetCardLocation(CardLocation.Discarded);
        DiscardedCard?.Invoke(_card);
    }

    public void DestroyCardFromTable(CardObject _card)
    {
        RemoveCardFromTable(_card);
        DestroyedCardFromTable?.Invoke(_card);
        Destroy(_card.gameObject);
    }
}
