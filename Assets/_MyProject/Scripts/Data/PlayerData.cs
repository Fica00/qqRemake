using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class PlayerData
{
    private string name;
    private int exp;
    private List<DeckData> decks = new();
    private int selectedDeck;
    private List<int> ownedQoomons = new();
    private List<GamePass> gamePasses = new();
    private double coins;
    private double usdc;
    private List<int> claimedLevelRewards = new ();


    public static Action UpdatedName;
    public static Action UpdatedSelectedDeck;
    public static Action UpdatedCardsInDeck;
    public static Action UpdatedDecks;
    public static Action UpdatedDeckName;
    public static Action UpdatedGamePasses;
    public static Action UpdatedCoins;
    public static Action UpdatedUSDC;
    public static Action UpdatedExp;
    public static Action UpdatedOwnedQoomons;
    public static Action UpdatedClaimedLevelRewards;

    public void CreateNewPlayer()
    {
        name = "Player" + UnityEngine.Random.Range(0, 10000);
        selectedDeck = 0;
        DeckData _starterDeck = new DeckData
        {
            Id = 0,
            Name = "Starter",
            CardsInDeck = new List<int>
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
            }
        };
        decks.Add(_starterDeck);

        ownedQoomons.Add(28);
        ownedQoomons.Add(8);
        ownedQoomons.Add(7);
        ownedQoomons.Add(29);
        ownedQoomons.Add(5);
        ownedQoomons.Add(1);
        ownedQoomons.Add(0);
        ownedQoomons.Add(11);
        ownedQoomons.Add(3);
        ownedQoomons.Add(4);
        ownedQoomons.Add(21);
        ownedQoomons.Add(9);
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
        decks.Add(new DeckData { Id = decks.Count, CardsInDeck = new() });
        UpdatedDecks?.Invoke();
    }
    
    public void DeleteSelectedDeck()
    {
        if (decks.Count==1)
        {
            UIManager.Instance.OkDialog.Setup("You need to have latest 1 deck");
            return;
        }

        DataManager.Instance.GetComponent<MonoBehaviour>().StartCoroutine(DeleteDeck());
        IEnumerator DeleteDeck()
        {
            DeckData _selectedDeck = GetSelectedDeck();
            decks.Remove(_selectedDeck);
            yield return null;
            SelectedDeck = decks[0].Id;
            UpdatedDecks?.Invoke();
        }
    }

    public List<int> OwnedQoomons
    {
        get => ownedQoomons;
        set => ownedQoomons = value;
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
        DeckData _deck = GetSelectedDeck();
        if (_deck==null)
        {
            return;
        }

        if (_deck.Name == _name)
        {
            return;
        }
        
        _deck.Name = _name;
        UpdatedDeckName?.Invoke();
    }

    public DeckData GetSelectedDeck()
    {
        DeckData _deck = decks.Find(_deck => _deck.Id == selectedDeck);
        return _deck;
    }

    public List<GamePass> GamePasses
    {
        get => gamePasses;
        set
        {
            gamePasses = value;
            UpdatedGamePasses?.Invoke();
        }
    }

    public double Coins
    {
        get => coins;
        set
        {
            coins = value;
            UpdatedCoins?.Invoke();
        }
    }

    public double USDC
    {
        get => usdc;
        set
        {
            usdc = value;
            UpdatedUSDC?.Invoke();
        }
    }

    public void AddGamePass(GamePass _offer)
    {
        Coins += _offer.Coins;
        _offer.Coins = 0;
        gamePasses.Add(_offer);
        UpdatedGamePasses?.Invoke();
    }

    public void RemoveGamePass(GamePass _gamePass)
    {
        gamePasses.Remove(_gamePass);
        UpdatedGamePasses?.Invoke();
    }

    int[] expBorders = { 10,30,60,100,150,210,280,360,450,550,650,750};

    public int Exp
    {
        get => exp;
        set
        {
            exp = value;
            UpdatedExp?.Invoke();
        }
    }

    [JsonIgnore]
    public int Level
    {
        get
        {
            int _level = 0;
            while (true)
            {
                if (exp<expBorders[_level])
                {
                    return _level;
                }

                _level++;
                if (_level>=expBorders.Length)
                {
                    return _level;
                }
            }
        }
    }

    [JsonIgnore]
    public float LevelPercentage
    {
        get
        {
            if (Level >= expBorders.Length)
            {
                return 1.0f;
            }

            int _currentLevelXp = Level == 0 ? exp : Exp-GetXpForLevel(Level-1);
            int _nextLevelXp = GetXpForLevel(Level);
            return (float)_currentLevelXp / _nextLevelXp;
        }
    }

    [JsonIgnore]
    public int CurrentExpOnLevel
    {
        get
        {
            if (Level >= expBorders.Length)
            {
                return Exp - GetXpForLevel(expBorders.Length-1);
            }

            if (Level==0)
            {
                return exp;
            }
            
            return exp-GetXpForLevel(Level-1);
        }
    }

    public int GetXpForNextLevel()
    {
        if (Level==0)
        {
            return expBorders[Level];
        }

        return expBorders[Level] - expBorders[Level-1];
    }
    
    public int GetXpForLevel(int _level)
    {
        if (Level >= expBorders.Length)
        {
            return 0;
        }
        
        return expBorders[_level];
    }

    public List<int> ClaimedLevelRewards => claimedLevelRewards;

    public void AddQoomon(int _qoomonId)
    {
        ownedQoomons.Add(_qoomonId);
        UpdatedOwnedQoomons?.Invoke();
    }

    public void ClaimedLevelReward(int _level)
    {
        DataManager.Instance.PlayerData.ClaimedLevelRewards.Add(_level);
        UpdatedClaimedLevelRewards?.Invoke();
    }
}