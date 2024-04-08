using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotPlayer : GameplayPlayer
{
    private Coroutine playCoroutine;
    private bool hasPlayedThisRound;
    private BotType botType;

    public static List<int> CardsInDeck = new()
    {
        28,
        8,
        7,
        29,
        1,
        0,
        11,
        3,
        4,
        21,
        9,
        5
    };
    
    public override void Setup()
    {
        botType = BotTypeSelection.BotType;
        List<int> _cardsInDeck = CardsInDeck;
        base.CardsInDeck = new List<CardObject>();
        foreach (var _cardInDeck in _cardsInDeck)
        {
            CardObject _cardObject = CardsManager.Instance.CreateCard(_cardInDeck, IsMy);
            _cardObject.transform.SetParent(transform);
            base.CardsInDeck.Add(_cardObject);
        }
        ShuffleDeck();
        CardsInHand = new List<CardObject>();
        CardsInDiscardPile = new List<CardObject>();
        GameplayManager.UpdatedGameState += ManageGameState;
        playerDisplay.Setup(this);
    }

    private void OnDisable()
    {
        GameplayManager.UpdatedGameState -= ManageGameState;
    }

    private void ManageGameState()
    {
        switch (GameplayManager.Instance.GameplayState)
        {
            case GameplayState.ResolvingBeginingOfRound:
                break;
            case GameplayState.Playing:
                if (playCoroutine != null)
                {
                    StopCoroutine(playCoroutine);
                }
                if (!hasPlayedThisRound)
                {
                    playCoroutine = StartCoroutine(PlayCards());
                }
                break;
            case GameplayState.Waiting:
                break;
            case GameplayState.ResolvingEndOfRound:
                hasPlayedThisRound = false;
                break;
        }
    }

    private IEnumerator PlayCards()
    {
        if (botType is BotType.Version2 or BotType.Version3)
        {
            yield return new WaitUntil(() => GameplayManager.Instance.IFinished);
            int _randomNumber = Random.Range(0, 10);
            if (_randomNumber > 5)
            {
                if (_randomNumber < 7)
                {
                    yield return new WaitForSeconds(2);
                }
                else if (_randomNumber < 9)
                {
                    yield return new WaitForSeconds(4);
                }
                else
                {
                    yield return new WaitForSeconds(6);
                }
            }
        }

        int[] _playerPower = GameplayManager.Instance.TableHandler.GetAllPower(true).ToArray();
        int[] _botPower = GameplayManager.Instance.TableHandler.GetAllPower(false).ToArray();

        bool[] _canPlaceCard = new bool[3];

        for (int _i = 0; _i < 3; _i++)
        {
            //i==0 first time when going through try to place card that would change power scale in bots favor
            //i==1 try to equalise somewhere
            //i==2 just place card anywhere
            if (botType == BotType.Version3)
            {
                if (GameplayManager.Instance.CurrentRound is 4 or 6)
                {
                    _i = 2;
                }
            }
            _canPlaceCard[0] = GameplayManager.Instance.Lanes[0].GetPlaceLocation(false);
            _canPlaceCard[1] = GameplayManager.Instance.Lanes[1].GetPlaceLocation(false);
            _canPlaceCard[2] = GameplayManager.Instance.Lanes[2].GetPlaceLocation(false);

            _canPlaceCard = _canPlaceCard.OrderBy(element => Guid.NewGuid()).ToArray();
            for (int _j = 0; _j < _canPlaceCard.Length; _j++)
            {
                if (!_canPlaceCard[_j])
                {
                    continue;
                }
                if (_i == 0)
                {
                    foreach (var _card in CardsInHand.ToList())
                    {
                        if (_playerPower[_j] > _botPower[_j] && _playerPower[_j] < _botPower[_j] + _card.Stats.Power)
                        {
                            PlaceCard(_card, _botPower, _j);
                        }
                    }
                }
                else if (_i == 1)
                {
                    foreach (var _card in CardsInHand.ToList())
                    {
                        if (_playerPower[_j] == _botPower[_j])
                        {
                            PlaceCard(_card, _botPower, _j);
                        }
                    }
                }
                else if (_i == 2)
                {
                    foreach (var _card in CardsInHand.ToList())
                    {
                        PlaceCard(_card, _botPower, _j);
                    }
                }
            }
        }

        hasPlayedThisRound = true;
        GameplayManager.Instance.OpponentFinished();
    }

    private void PlaceCard(CardObject _card, int[] _power, int _index)
    {
        if (Energy < _card.Stats.Energy)
        {
            return;
        }

        LanePlaceIdentifier _place = GameplayManager.Instance.Lanes[_index].GetPlaceLocation(false);
        LaneAbility _laneAbility = null;
        
        if (GameplayManager.Instance.LaneAbilities.ContainsKey(GameplayManager.Instance.Lanes[_index]))
        {
            _laneAbility = GameplayManager.Instance.LaneAbilities[GameplayManager.Instance.Lanes[_index]];
        }
        if (_place == null)
        {
            return;
        }

        if (_laneAbility!=null)
        {
            foreach (var _ability in _laneAbility.Abilities)
            {
                if (_ability is LaneAbilityChangePowerToQommonsHere)
                {
                    LaneAbilityChangePowerToQommonsHere _lowerPowerAbility =
                        (_ability as LaneAbilityChangePowerToQommonsHere);
                    if (_lowerPowerAbility.PowerAmount<0&& Math.Abs(_lowerPowerAbility.PowerAmount)>_card.Details.Power)
                    {
                        return;
                    }
                }
            }
           
        }

        if (_card.TryToPlace(_place))
        {
            _card.Display.HideCardOnTable();
            _power[_index] += _card.Stats.Power;
        }
    }
}
