using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayPlayer : MonoBehaviour
{
    public static Action<PlaceCommand> AddedCardToTable;
    public static Action<PlaceCommand> RemovedCardFromTable;
    public Action<CardObject> AddedCardToHand;
    public Action<CardObject> RemovedCardFromHand;
    public Action UpdatedEnergy;

    [field: SerializeField] public bool IsMy { get; private set; }

    [SerializeField] CardsInHandHandler cardsInHandHandler = null;
    [SerializeField] EnergyDisplayHandler energyDisplayHandler = null;

    protected List<int> cardsInDeck;
    protected List<CardObject> cardsInHand;
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
        cardsInDeck = new List<int>(DataManager.Instance.PlayerData.CardIdsIndeck);
        cardsInHand = new List<CardObject>();
        CardsOnTop = new List<CardObject>();
        CardsOnMid = new List<CardObject>();
        CardsOnBot = new List<CardObject>();
        cardsInHandHandler.Setup(this);
        energyDisplayHandler.Setup(this);
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
        switch (_command.Location)
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
                throw new Exception("Cant handle Location: " + _command.Location);
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

    void CancelCommand(PlaceCommand _command)
    {
        Energy += _command.Card.Stats.Energy;
        AddCardToHand(_command.Card);
        RemoveCardFromTable(_command);
        RemovedCardFromTable?.Invoke(_command);
    }
}
