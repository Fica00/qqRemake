using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class PlayerData
{
    private string name;
    private List<DeckData> decks = new();
    private int selectedDeck;
    private List<int> ownedQommons = new();

    [JsonIgnore] public Action UpdatedName;
    [JsonIgnore] public Action UpdatedSelectedDeck;
    [JsonIgnore] public Action UpdatedCardsInDeck;
    [JsonIgnore] public Action BoughtNewDeck;
    [JsonIgnore] public Action UpdatedDeckName;

    public void CreateNewPlayer()
    {
        name = "Player" + UnityEngine.Random.Range(0, 10000);
        selectedDeck = 0;
        DeckData _startingDeck = new DeckData
        {
            Id = 0,
            CardsInDeck = new List<int>
            {
                0,
                1,
                2,
                3,
                4,
                5,
                6,
                7,
                8,
                9,
                10,
                11
            }
        };
        decks.Add(_startingDeck);

        for (int _i = 0; _i < 47; _i++)
        {
            if (_i is 37 or 34 or 23)
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
        set => decks = value;
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
        decks.Add(new DeckData { Id = decks.Count, CardsInDeck = new () });
        BoughtNewDeck?.Invoke();
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

    public void UpdateDeckName(string _name)
    {
        DeckData _deck = decks.Find(_deck => _deck.Id == selectedDeck);
        _deck.Name = _name;
        UpdatedDeckName?.Invoke();
    }

    public DeckData GetDeck(int _id)
    {
        DeckData _deck = decks.Find(_deck => _deck.Id == selectedDeck);
        return _deck;
    }


}