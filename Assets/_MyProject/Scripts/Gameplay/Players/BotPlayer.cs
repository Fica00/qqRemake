using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotPlayer : GameplayPlayer
{
    private Coroutine playCoroutine;
    bool hasPlayedThisRound = false;

    public override void Setup()
    {
        cardsInDeck = GenerateDeck();
        cardsInHand = new List<CardObject>();
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

    IEnumerator PlayCards()
    {
        int _waitRandomTime = UnityEngine.Random.Range(0, GameplayManager.Instance.DurationOfRound - 2);
        yield return new WaitForSeconds(_waitRandomTime);

        int[] _playerPower = GameplayManager.Instance.TableHandler.GetAllPower(true);
        int[] _botPower = GameplayManager.Instance.TableHandler.GetAllPower(false);

        bool[] _canPlaceCard = new bool[3];
        _canPlaceCard[0] = GameplayManager.Instance.Lanes[0].GetPlaceLocation(false);
        _canPlaceCard[1] = GameplayManager.Instance.Lanes[1].GetPlaceLocation(false);
        _canPlaceCard[2] = GameplayManager.Instance.Lanes[2].GetPlaceLocation(false);

        for (int i = 0; i < 3; i++)
        {
            //i==0 first time when going through try to place card that would change power scale in bots favor
            //i==1 try to equalise somewhere
            //i==2 just place card anywhere
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
                            if (Energy < _card.Stats.Energy)
                            {
                                continue;
                            }
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
                            if (Energy < _card.Stats.Energy)
                            {
                                continue;
                            }
                            Debug.Log("Tie but can start winning on this location: " + GameplayManager.Instance.Lanes[j].Location);
                            PlaceCard(_card, _botPower, j);
                        }
                    }
                }
                else if (i == 2)
                {
                    foreach (var _card in cardsInHand.ToList())
                    {
                        if (Energy < _card.Stats.Energy)
                        {
                            continue;
                        }
                        Debug.Log("Just place anywhere: " + GameplayManager.Instance.Lanes[j].Location);
                        PlaceCard(_card, _botPower, j);
                    }
                }
            }
        }

        hasPlayedThisRound = true;
        GameplayManager.Instance.OpponentFinished();
    }

    void PlaceCard(CardObject _card, int[] _power, int _index)
    {
        _card.TryToPlace(GameplayManager.Instance.Lanes[_index].GetPlaceLocation(false));
        _card.Display.HideCardOnTable();
        _power[_index] += _card.Stats.Power;
    }

    List<int> GenerateDeck()
    {
        //todo generate deck
        return new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
    }
}
