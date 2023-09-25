using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableHandler : MonoBehaviour
{
    public static Action<LaneLocation> UpdatedPower;
    public static Action<CardObject> OnRevealdCard;
    private int[] myPower = new int[3];//top,mid,bot
    private int[] opponentPower = new int[3];//top,mid,bot

    private List<CardObject>[] myCardsOnTable = new List<CardObject>[3]; //top,mid,bot
    private List<CardObject>[] opponentCardsOnTable = new List<CardObject>[3]; //top,mid,bot


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

        LaneSpecifics.UpdatedExtraPower += CalculatePower;
        GameplayPlayer.DestroyedCardFromTable += RemoveCard;
    }

    private void OnDestroy()
    {
        LaneSpecifics.UpdatedExtraPower -= CalculatePower;
        GameplayPlayer.DestroyedCardFromTable -= RemoveCard;
    }

    void RemoveCard(CardObject _cardObject)
    {
        var _cardsOnLane = _cardObject.IsMy ? myCardsOnTable[(int)_cardObject.LaneLocation]: opponentCardsOnTable[(int)_cardObject.LaneLocation] ;
        if (_cardsOnLane.Contains(_cardObject))
        {
            _cardsOnLane.Remove(_cardObject);
            CalculatePower(ChangeStatus.Decreased);
        }
    }
    
    public int WhichCardsToRevealFrist() //-1 mine, 1 opponents, show mine if I am winning show opponents if he is winnig,show random if it is draw
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
        else if (_myAmountOfWinningLocations < _opponentAmountOfWinningLocations)
        {
            return 1;
        }
        else
        {
            if (PhotonNetwork.CurrentRoom == null)
            {
                return -1;
            }
            else
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
        }
    }

    public IEnumerator RevealCards(List<PlaceCommand> _commands)
    {
        foreach (var _command in _commands.ToList())
        {
            yield return new WaitUntil(() => !CardEffectWhenThisIsDiscardedAddXPowerAndAddItBackToHand.IsActive);
            // if (SkipRevealAnimation(_command.Card))
            // {
            //     _command.Card.Reveal.PreReveal();
            //     _command.Card.Reveal.Finish();
            //     _command.Card.Subscribe();
            // }
            // else
            // {
            //     yield return StartCoroutine(_command.Card.RevealCard());
            // }
            yield return StartCoroutine(_command.Card.RevealCard());
            List<CardObject> _cardsOnLane = null;
            switch (_command.Location)
            {
                case LaneLocation.Top:
                    _cardsOnLane = _command.IsMyPlayer ? myCardsOnTable[0] : opponentCardsOnTable[0];
                    break;
                case LaneLocation.Mid:
                    _cardsOnLane = _command.IsMyPlayer ? myCardsOnTable[1] : opponentCardsOnTable[1];
                    break;
                case LaneLocation.Bot:
                    _cardsOnLane = _command.IsMyPlayer ? myCardsOnTable[2] : opponentCardsOnTable[2];
                    break;
            }
            
            AddCardOnLane(_command.Card, _cardsOnLane);
            OnRevealdCard?.Invoke(_command.Card);
            _commands.Remove(_command);
        }
    }

    private bool SkipRevealAnimation(CardObject _card)
    {
        int _laneIndex = (int)_card.LaneLocation;
        var _laneAbility = GameplayManager.Instance.LaneAbilities.ContainsKey(GameplayManager.Instance.Lanes[_laneIndex])?
            GameplayManager.Instance.LaneAbilities[GameplayManager.Instance.Lanes[_laneIndex]]:
            null;
        if (_laneAbility!=null)
        {
            foreach (var _laneEffect in _laneAbility.Abilities)
            {
                if (_laneEffect is LaneAbilityDisableOnRevealEffects)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void AddCardOnLane(CardObject _card, List<CardObject> _cardsOnLane)
    {
        _cardsOnLane.Add(_card);
        _card.Stats.UpdatedPower += CalculatePower;
        CalculatePower();
    }

    private void CalculatePower(ChangeStatus _status)
    {
        CalculatePower();
    }

    private void CalculatePower()
    {
        CalculatePower(true);
        CalculatePower(false);


        void CalculatePower(bool _isMy)
        {
            List<CardObject>[] _cardsOnTable = _isMy ? myCardsOnTable : opponentCardsOnTable;
            int[] _powerHolder = _isMy ? myPower : opponentPower;
            int _playerIndex = _isMy ? 0 : 1;
            for (int i = 0; i < _cardsOnTable.Length; i++)
            {
                int _power = 0;
                LaneLocation _location = LaneLocation.Bot;
                LaneDisplay _laneDisplay = null;

                foreach (var _cardOnLane in _cardsOnTable[i])
                {
                    _power += _cardOnLane.Stats.Power;
                }

                switch (i)
                {
                    case 0:
                        _location = LaneLocation.Top;
                        _laneDisplay = GameplayManager.Instance.Lanes[0];
                        break;
                    case 1:
                        _location = LaneLocation.Mid;
                        _laneDisplay = GameplayManager.Instance.Lanes[1];
                        break;
                    case 2:
                        _location = LaneLocation.Bot;
                        _laneDisplay = GameplayManager.Instance.Lanes[2];
                        break;
                }

                int _extraPower = 0;
                foreach (var _card in GetCards(_isMy, _location))
                {
                    _extraPower += _card.Stats.ChagePowerDueToLocation;
                    foreach (var _specialEffect in _card.SpecialEffects)
                    {
                        if (_specialEffect is CardEffectDoublePowerOnCurrentLane)
                        {
                            int _geishasPower =0;
                            for (int j = 0; j <= GameplayManager.Instance.Lanes[(int)_location].LaneSpecifics.AmountOfOngoingEffects; j++)
                            {
                                if (j==0)
                                {
                                    _geishasPower = _power;
                                    continue;
                                }
                                _geishasPower *= 2;
                            }
                            
                            _power = _geishasPower;
                        }
                    }
                }

                //add extra power
                _power += _extraPower;

                _power += _laneDisplay.LaneSpecifics.ExtraPower[_playerIndex];

                _powerHolder[i] = _power;

                UpdatedPower?.Invoke(_location);
            }
        }
    }

    public int GetPower(bool _my, LaneLocation _location)
    {
        int[] _powers = _my ? myPower : opponentPower;
        int _power;
        switch (_location)
        {
            case LaneLocation.Top:
                _power = _powers[0];
                break;
            case LaneLocation.Mid:
                _power = _powers[1];
                break;
            case LaneLocation.Bot:
                _power = _powers[2];
                break;
            default:
                throw new Exception("Cant handle lane: " + _location);
        }

        return _power;
    }

    public GameResult CalculateWinner()
    {
        int _myAmountOfWinningLocations = 0;
        int _opponentAmountOfWinningLocations = 0;
        for (int i = 0; i < myPower.Length; i++)
        {
            int _myPowerOnLane = GetPower(true, (LaneLocation)(i));
            int _opponentPowerOnLane = GetPower(false, (LaneLocation)(i));
            if (_myPowerOnLane > _opponentPowerOnLane)
            {
                _myAmountOfWinningLocations++;
            }
            else if (_myPowerOnLane < _opponentPowerOnLane)
            {
                _opponentAmountOfWinningLocations++;
            }
        }

        if (_myAmountOfWinningLocations > _opponentAmountOfWinningLocations)
        {
            return GameResult.IWon;
        }
        else if (_myAmountOfWinningLocations == _opponentAmountOfWinningLocations)
        {
            return GameResult.Draw;
        }
        else
        {
            return GameResult.ILost;
        }
    }

    public int[] GetAllPower(bool _getPlayers)
    {
        if (_getPlayers)
        {
            return myPower;
        }
        else
        {
            return opponentPower;
        }
    }

    public List<CardObject> GetCards(bool _getForPlayer, LaneLocation _location)
    {
        switch (_location)
        {
            case LaneLocation.Top:
                return _getForPlayer ? myCardsOnTable[0] : opponentCardsOnTable[0];
            case LaneLocation.Mid:
                return _getForPlayer ? myCardsOnTable[1] : opponentCardsOnTable[1];
            case LaneLocation.Bot:
                return _getForPlayer ? myCardsOnTable[2] : opponentCardsOnTable[2];
            default:
                throw new Exception("Dont know how to handle location: " + _location);
        }
    }

    public List<CardObject> GetCards(GameplayPlayer _player)
    {
        List<CardObject> _validCards = new List<CardObject>();

        if (_player.IsMy)
        {
            AddCardsToListOfCards(myCardsOnTable[0], _validCards);
            AddCardsToListOfCards(myCardsOnTable[1], _validCards);
            AddCardsToListOfCards(myCardsOnTable[2], _validCards);
        }
        else
        {
            AddCardsToListOfCards(opponentCardsOnTable[0], _validCards);
            AddCardsToListOfCards(opponentCardsOnTable[1], _validCards);
            AddCardsToListOfCards(opponentCardsOnTable[2], _validCards);
        }

        return _validCards;

        void AddCardsToListOfCards(List<CardObject> _cards, List<CardObject> _listToAddTo)
        {
            foreach (var _card in _cards)
            {
                _listToAddTo.Add(_card);
            }
        }
    }
}
