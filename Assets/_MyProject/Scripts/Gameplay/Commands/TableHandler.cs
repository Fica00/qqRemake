using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableHandler : MonoBehaviour
{
    public static Action<LaneLocation> UpdatedPower;
    int[] myPower = new int[3];//top,mid,bot
    int[] opponentPower = new int[3];//top,mid,bot

    List<CardObject>[] myCardsOnTable = new List<CardObject>[3]; //top,mid,bot
    List<CardObject>[] opponentCardsOnTable = new List<CardObject>[3]; //top,mid,bot


    public void Setup()
    {
        for (int i = 0; i < myPower.Length; i++)
        {
            myPower[i] = 0;
            opponentPower[i] = 0;
        }

        for (int i = 0; i < myCardsOnTable.Length; i++)
        {
            myCardsOnTable[i] = new List<CardObject>();
            opponentCardsOnTable[i] = new List<CardObject>();
        }
    }

    public int WhichCardsToRevealFrist() //-1 mine, 1 opponents, show mine if I am winning show opponents if it is draw or he is winning
    {
        int _myAmountOfWinningLocations = 0;
        int _opponentAmountOfWinningLocations = 0;
        for (int i = 0; i < myPower.Length; i++)
        {
            if (myPower[i] > opponentPower[i])
            {
                _myAmountOfWinningLocations++;
            }
            else if (myPower[i] < opponentPower[i])
            {
                _opponentAmountOfWinningLocations++;
            }
        }

        if (_myAmountOfWinningLocations > _opponentAmountOfWinningLocations)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }

    public IEnumerator RevealCards(List<PlaceCommand> _commands)
    {
        foreach (var _command in _commands)
        {
            yield return _command.Card.RevealCard();
            List<CardObject> _cardsOnLane = null;
            switch (_command.Location)
            {
                case LaneLocation.Top:
                    _cardsOnLane = _command.Player.IsMy ? myCardsOnTable[0] : opponentCardsOnTable[0];
                    break;
                case LaneLocation.Mid:
                    _cardsOnLane = _command.Player.IsMy ? myCardsOnTable[1] : opponentCardsOnTable[1];
                    break;
                case LaneLocation.Bot:
                    _cardsOnLane = _command.Player.IsMy ? myCardsOnTable[2] : opponentCardsOnTable[2];
                    break;
                default:
                    break;
            }
            AddCardOnLane(_command.Card, _cardsOnLane);
        }

        _commands.Clear();
    }

    void AddCardOnLane(CardObject _card, List<CardObject> _cardsOnLane)
    {
        _cardsOnLane.Add(_card);
        _card.Stats.UpdatedPower += CalculatePower;
        CalculatePower();
    }

    void CalculatePower(ChangeStatus _status)
    {
        CalculatePower();
    }

    void CalculatePower()
    {
        CalculatePower(myCardsOnTable, myPower);
        CalculatePower(opponentCardsOnTable, opponentPower);

        void CalculatePower(List<CardObject>[] _cardsOnTable, int[] _powerHolder)
        {
            for (int i = 0; i < _cardsOnTable.Length; i++)
            {
                int _power = 0;
                bool _shouldUpdatePower = false;
                LaneLocation _location = LaneLocation.Bot;

                foreach (var _cardOnLane in _cardsOnTable[i])
                {
                    _power += _cardOnLane.Stats.Power;
                }

                if (_powerHolder[i] != _power)
                {
                    switch (i)
                    {
                        case 0:
                            _location = LaneLocation.Top;
                            break;
                        case 1:
                            _location = LaneLocation.Mid;
                            break;
                        case 2:
                            _location = LaneLocation.Bot;
                            break;
                    }
                    _shouldUpdatePower = true;
                }
                _powerHolder[i] = _power;

                if (_shouldUpdatePower)
                {
                    Debug.Log("Gonna update power for location: " + _location);
                    UpdatedPower?.Invoke(_location);
                }
            }
        }
    }

    public int GetPower(bool _my, LaneLocation _location)
    {
        int[] _powers = _my ? myPower : opponentPower;
        switch (_location)
        {
            case LaneLocation.Top:
                return _powers[0];
            case LaneLocation.Mid:
                return _powers[1];
            case LaneLocation.Bot:
                return _powers[2];
            default:
                throw new Exception("Cant handle lane: " + _location);
        }
    }
}
