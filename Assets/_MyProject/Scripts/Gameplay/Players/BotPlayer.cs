using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotPlayer : GameplayPlayer
{
    private Coroutine playCoroutine;
    private bool hasPlayedThisRound = false;

    public override void Setup()
    {
        List<int> _cardsInDeck = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
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
        GameplayManager.UpdatedGameState += ManageGameState;
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
            default:
                break;
        }
    }

    private IEnumerator PlayCards()
    {
        //todoo uncomment me
        //int _waitRandomTime = UnityEngine.Random.Range(0, GameplayManager.Instance.DurationOfRound - 2);
        int _waitRandomTime = UnityEngine.Random.Range(10, 20);

        yield return new WaitForSeconds(_waitRandomTime);

        int[] _playerPower = GameplayManager.Instance.TableHandler.GetAllPower(true).ToArray();
        int[] _botPower = GameplayManager.Instance.TableHandler.GetAllPower(false).ToArray();

        bool[] _canPlaceCard = new bool[3];

        for (int i = 0; i < 3; i++)
        {
            //i==0 first time when going through try to place card that would change power scale in bots favor
            //i==1 try to equalise somewhere
            //i==2 just place card anywhere
            _canPlaceCard[0] = GameplayManager.Instance.Lanes[0].GetPlaceLocation(false);
            _canPlaceCard[1] = GameplayManager.Instance.Lanes[1].GetPlaceLocation(false);
            _canPlaceCard[2] = GameplayManager.Instance.Lanes[2].GetPlaceLocation(false);

            _canPlaceCard = _canPlaceCard.OrderBy(element => Guid.NewGuid()).ToArray();
            for (int j = 0; j < _canPlaceCard.Length; j++)
            {
                if (!_canPlaceCard[j])
                {
                    continue;
                }
                if (i == 0)
                {
                    foreach (var _card in cardsInHand.ToList())
                    {
                        if (_playerPower[j] > _botPower[j] && _playerPower[j] < _botPower[j] + _card.Stats.Power)
                        {
                            PlaceCard(_card, _botPower, j);
                        }
                    }
                }
                else if (i == 1)
                {
                    foreach (var _card in cardsInHand.ToList())
                    {
                        if (_playerPower[j] == _botPower[j])
                        {
                            PlaceCard(_card, _botPower, j);
                        }
                    }
                }
                else if (i == 2)
                {
                    foreach (var _card in cardsInHand.ToList())
                    {
                        PlaceCard(_card, _botPower, j);
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
        if (_place == null)
        {
            return;
        }

        if (_card.TryToPlace(_place))
        {
            _card.Display.HideCardOnTable();
            _power[_index] += _card.Stats.Power;
        }
    }
}
