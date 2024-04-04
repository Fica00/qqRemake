using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayPlayer : MonoBehaviour
{
    public static Action<CardObject> DrewCard;
    public static Action<PlaceCommand> AddedCardToTable;
    public static Action<PlaceCommand> RemovedCardFromTable;
    public static Action<CardObject> DiscardedCard;
    public static Action<CardObject> DestroyedCardFromTable;
    public Action<CardObject,bool> AddedCardToHand;
    public Action<CardObject> RemovedCardFromHand;
    public Action UpdatedEnergy;
    public Action FinishedTurn;

    [field: SerializeField] public bool IsMy { get; private set; }

    [SerializeField] private CardsInHandHandler cardsInHandHandler;
    [SerializeField] private EnergyDisplayHandler energyDisplayHandler;
    [SerializeField] protected PlayerDisplay playerDisplay;

    private List<CardObject> cardsInHand;
    private List<CardObject> cardsInDiscardPile;

    protected List<CardObject> CardsInDeck;

    protected List<CardObject> CardsInHand
    {
        get => cardsInHand;
        set
        {
            cardsInHand = value;
            PhotonManager.Instance.TryUpdateCustomProperty(PhotonManager.AMOUNT_OF_CARDS_IN_HAND,cardsInHand.Count.ToString());
        }
    }

    protected List<CardObject> CardsInDiscardPile
    {
        get => cardsInDiscardPile;
        set
        {
            cardsInDiscardPile = value;
            PhotonManager.Instance.TryUpdateCustomProperty(PhotonManager.AMOUNT_OF_DISCARDED_CARDS,cardsInDiscardPile.Count.ToString());
        }
    }
    protected int energy;

    public List<CardObject> CardsOnTop;
    public List<CardObject> CardsOnMid;
    public List<CardObject> CardsOnBot;

    private int amountOfDestroyedCards;
    
    public int AmountOfCardsInHand => CardsInHand.Count;
    public int AmountOfDiscardedCards => CardsInDiscardPile.Count;

    public int AmountOfDestroyedCards
    {
        get => amountOfDestroyedCards;
        set
        {
            amountOfDestroyedCards = value;
            PhotonManager.Instance.TryUpdateCustomProperty(PhotonManager.AMOUNT_OF_DESTROYED_CARDS,amountOfDestroyedCards.ToString());
        }
    }
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
        List<int> _cardsInDeck = new List<int>(DataManager.Instance.PlayerData.CardIdsInDeck);
        CardsInDeck = new List<CardObject>();
        foreach (var _cardInDeck in _cardsInDeck)
        {
            CardObject _cardObject = CardsManager.Instance.CreateCard(_cardInDeck, IsMy);
            _cardObject.transform.SetParent(transform);
            CardsInDeck.Add(_cardObject);
        }
        ShuffleDeck();
        CardsInHand = new List<CardObject>();
        CardsInDiscardPile = new List<CardObject>();
        CardsOnTop = new List<CardObject>();
        CardsOnMid = new List<CardObject>();
        CardsOnBot = new List<CardObject>();
        cardsInHandHandler.Setup(this);
        energyDisplayHandler.Setup(this);
        playerDisplay.Setup(this);
    }

    protected void ShuffleDeck()
    {
        CardsInDeck = CardsInDeck.OrderBy(element => Guid.NewGuid()).ToList();

        //check for cards that should get in hand in certain runds and send them on bottom of the deck
        for (int i = CardsInDeck.Count - 1; i >= 0; i--)
        {
            var _specialEffects = CardsManager.Instance.GetCardEffects(CardsInDeck[i].Details.Id);
            foreach (var _specialEffect in _specialEffects)
            {
                if (_specialEffect is CardEffectAddCardToHandOnRound)
                {
                    var _card = CardsInDeck[i];
                    CardsInDeck.RemoveAt(i);
                    CardsInDeck.Add(_card);
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
        if (CardsInDeck.Count==0)
        {
            return null;
        }
        CardObject _card = CardsInDeck[0];
        return DrawCard(_card, true);
    }

    public CardObject DrawCard(CardObject _card, bool _updateDeck)
    {
        if (_updateDeck)
        {
            if (CardsInDeck.Contains(_card))
            {
                CardsInDeck.Remove(_card);
            }
        }
        
        DrewCard?.Invoke(_card);
        return _card;
    }

    public void AddCardToHand(CardObject _cardObject, bool _showAnimation=true)
    {
        if (IsMy)
        {
            EventsManager.DrawCard?.Invoke();
        }
        CardsInHand.Add(_cardObject);
        _cardObject.SetCardLocation(CardLocation.Hand);
        AddedCardToHand?.Invoke(_cardObject,_showAnimation);
    }
    
    public void RemoveCardFromHand(CardObject _cardObject)
    {
        if (CardsInHand.Contains(_cardObject))
        {
            CardsInHand.Remove(_cardObject);
        }
        RemovedCardFromHand?.Invoke(_cardObject);
    }

    public void CheckForCardsThatShouldMoveToHand(Action _callback)
    {
        StartCoroutine(CheckForCardsThatShouldMoveToHandRoutine());

        IEnumerator CheckForCardsThatShouldMoveToHandRoutine()
        {
            List<CardObject> _cardsThatShouldStartInHand = new List<CardObject>();
            foreach (var _cardId in CardsInDeck)
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
                if (IsMy)
                {
                    GameplayManager.DrewCardDirectlyToHand = true;
                }
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
        if (_card.LaneLocation==LaneLocation.None)
        {
            return;
        }
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
        foreach (var _card in CardsInHand)
        {
            _card.Stats.Energy += _amount;
        }

        foreach (var _card in CardsInDeck)
        {
            _card.Stats.Energy += _amount;
        }
    }

    public CardObject GetQommonFromHand()
    {
        if (CardsInHand.Count > 0)
        {
            return CardsInHand[UnityEngine.Random.Range(0, CardsInHand.Count)];
        }

        return null;
    }
    
    public CardObject GetHigestCostQommon()
    {
        if (CardsInHand.Count == 0)
        {
            return null;
        }
        
        CardObject _highestCostQommon = CardsInHand[0];

        foreach (var _qommonInHand in CardsInHand)
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
        foreach (var _cardInHand in CardsInHand.ToList())
        {
            RemoveCardFromHand(_cardInHand);
            CardsInDeck.Add(_cardInHand);
            _cardInHand.transform.SetParent(transform);
        }
    }

    public void DiscardCardFromHand(CardObject _card)
    {
        CardsInHand.Remove(_card);
        GameplayManager.Instance.TellOpponentThatIDiscardedACard(_card);
        AnimateRoutine();

        void AnimateRoutine()
        {
            _card.Display.DiscardInHandAnimation(FinishDiscard);

            void FinishDiscard()
            {
                RemoveCardFromHand(_card);
                CardsInDiscardPile.Add(_card);
                _card.SetCardLocation(CardLocation.Discarded);
                _card.transform.SetParent(null);
                DiscardedCard?.Invoke(_card);
            }
        }
    }

    public void DestroyCardFromTable(CardObject _card)
    {
        RemoveCardFromTable(_card);
        DestroyedCardFromTable?.Invoke(_card);
        
        _card.Display.ShowDestroyEffect(FinishDestroy);

        void FinishDestroy()
        {
            AmountOfDestroyedCards++;
            Destroy(_card.gameObject);
        }
    }
}
