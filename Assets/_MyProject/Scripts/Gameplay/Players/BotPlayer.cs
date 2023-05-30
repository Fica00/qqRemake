using System;
using System.Collections;
using System.Collections.Generic;
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
        Debug.Log("Finished waiting,gonna play my cards");
        //todo play cards

        hasPlayedThisRound = true;
        GameplayManager.Instance.OpponentFinished();
    }

    List<int> GenerateDeck()
    {
        //todo generate deck
        return new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
    }
}
