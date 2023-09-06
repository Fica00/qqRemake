using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class PlayerData
{
    private string name;
    private List<DeckData> decks = new ();
    private int selectedDeck;
    private List<int> ownedQommons = new ();

    public Action UpdatedName;
    public Action UpdatedSelectedDeck;
    public Action UpdatedCardsInDeck;
    public Action UpdatedAmountOfOwnedDecks;

    public void CreateNewPlayer()
    {
        name = "Player" + UnityEngine.Random.Range(0, 10000);
        selectedDeck = 0;
        DeckData _startingDeck = new DeckData { Id = 0, CardsInDeck = new List<int> {0,1,2,3,4,5,6,7,8,9,10,11} };
        decks.Add(_startingDeck);

        for (int _i = 0; _i < 48; _i++)
        {
            if (_i is 15 or 47)
            {
                continue;
            }
            ownedQommons.Add(_i);
        }
    }
    public string Name
    {
        get => name;
        set
        {
            name = value;
            UpdatedName?.Invoke();
        }
    }
    public List<DeckData> Decks
    {
        get => decks;
        set => decks=value;
    }
    public int SelectedDeck
    {
        get => selectedDeck;
        set
        {
            selectedDeck = value;
            UpdatedSelectedDeck?.Invoke();
        }
    }

    [JsonIgnore]
    public List<int> CardIdsInDeck
    {
        get => decks.Find(_deck => _deck.Id == SelectedDeck).CardsInDeck;
    }

    public void SetCardsInSelectedDeck(List<int> _newCardsInDeck)
    {
        decks.Find(_deck => _deck.Id == selectedDeck).CardsInDeck = _newCardsInDeck;
        UpdatedCardsInDeck?.Invoke();
    }

    public void AddNewDeck()
    {
        decks.Add(new DeckData { Id = decks.Count, CardsInDeck = {0} });
        UpdatedAmountOfOwnedDecks?.Invoke();
    }

    public List<int> OwnedQommons
    {
        get => ownedQommons;
        set => ownedQommons = value;
    }
    
    public void AddCardToSelectedDeck(int _cardId)
    {
        DeckData _deck = decks.Find(_deck => _deck.Id == selectedDeck);
        _deck.CardsInDeck.Add(_cardId);
        UpdatedCardsInDeck?.Invoke();
    }

    public void RemoveCardFromSelectedDeck(int _cardId)
    {
        DeckData _deck = decks.Find(_deck => _deck.Id == selectedDeck);
        _deck.CardsInDeck.Remove(_cardId);
        UpdatedCardsInDeck?.Invoke();
    }

}
