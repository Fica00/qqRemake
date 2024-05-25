using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckInitializer : MonoBehaviour
{
   public static List<DeckData> InitializeDecks()
    {
        return new List<DeckData>()
        {
             new DeckData
            {
                Id = 0,
                Name = "Starter",
                CardsInDeck = new List<int> { 28, 8, 7, 29, 1, 0, 11, 3, 4, 21, 9, 5 }
            },
            new DeckData
            {
                Id = 1,
                Name = "Discard & Destroy",
                CardsInDeck = new List<int> { 7, 1, 11, 4, 14, 16, 30, 17, 31, 27, 18, 19 }
            },
            new DeckData
            {
                Id = 2,
                Name = "Summon",
                CardsInDeck = new List<int> { 8, 1, 0, 3, 9, 10, 12, 15, 24, 32, 35, 13 }
            },
            new DeckData
            {
                Id = 3,
                Name = "Summon Small",
                CardsInDeck = new List<int> { 28, 10, 20, 9, 26, 42, 33, 25, 36, 2, 45, 7 }
            },
            new DeckData
            {
                Id = 4,
                Name = "Ongoing",
                CardsInDeck = new List<int> { 4, 21, 6, 20, 22, 42, 38, 39, 40, 41, 43, 44 }
            }
        };

    }
}
