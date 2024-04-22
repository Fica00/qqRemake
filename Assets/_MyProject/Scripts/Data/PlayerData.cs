using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private List<ClaimedLevelReward> claimedLevelProgressRewards = new ();
    private int weeklyLoginAmount = 1;
    private DateTime lastDayConnected;
    private int daysConnectedInRow;
    private int rankPoints;
    private int amountOfRankGamesPlayed;
    private List<int> claimedRankRewards = new ();
    private List<int> claimedLoginRewards = new ();
    private List<MissionProgress> missionProgresses = new();
    private DateTime nextDailyChallenges;
    private int isDemoPlayer;
    private int hasFinishedTutorial;
    private bool playBackgroundMusic = true;
    private bool playSoundEffects = true;
    

    public DateTime DateCreatedAccount;

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
    public static Action UpdatedWeeklyLoginAmount;
    public static Action UpdatedLastDayConnected;
    public static Action UpdatedDaysConnectedInRow;
    public static Action UpdatedRankPoints;
    public static Action UpdatedAmountOfRankGamesPlayed;
    public static Action UpdatedClaimedRankRewards;
    public static Action UpdatedNextDailyChallenges;
    public static Action UpdatedLoginRewards;
    public static Action UpdatedIsDemoPlayer;
    public static Action UpdatedHasFinishedTutorial;
    public static Action UpdatedBackgroundMusic;
    public static Action UpdatedPlaySoundEffects;
    

    public void CreateNewPlayer()
    {
        name = "Player" + UnityEngine.Random.Range(0, 10000);
        selectedDeck = 0;
        DateCreatedAccount = DateTime.UtcNow.Date;
        lastDayConnected = DateCreatedAccount;
        daysConnectedInRow=1;
        
        if (JavaScriptManager.Instance.IsDemo)
        {
            SetupDemo();
        }
        else
        {
            Setup();
        }
    }

    private void SetupDemo()
    {
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
        ownedQoomons.Add(1);
        ownedQoomons.Add(0);
        ownedQoomons.Add(11);
        ownedQoomons.Add(3);
        ownedQoomons.Add(4);
        ownedQoomons.Add(21);
        ownedQoomons.Add(9);
        ownedQoomons.Add(5);
    }

    private void Setup()
    {
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

        DeckData _discardAndDestroy = new DeckData() { Id = 1, Name="Discard & Destroy", CardsInDeck = new List<int>
        {
            7,1,11,4,14,16,30,17,31,27,18,19
        } };
        decks.Add(_discardAndDestroy);
        
        DeckData _summon = new DeckData() { Id = 2, Name="Summon", CardsInDeck = new List<int>
        {
            8,1,0,3,9,10,12,15,24,32,35,13
        } };
        decks.Add(_summon);    
        
        DeckData _summonSmall = new DeckData() { Id = 3, Name="Summon Small", CardsInDeck = new List<int>
        {
            28,10,20,47,26,42,33,25,36,2,45,7
        } };
        decks.Add(_summonSmall);   
        
        DeckData _ongoing = new DeckData() { Id = 4, Name="Ongoing", CardsInDeck = new List<int>
        {
            4,21,6,20,22,42,38,39,40,41,43,44
        } };
        decks.Add(_ongoing);

        for (int _i = 0; _i < 47; _i++)
        {
            if (_i is 37 or 34 or 23)
            {
                continue;
            }

            ownedQoomons.Add(_i);
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
        decks.Add(new DeckData { Id = decks.Count, CardsInDeck = new() });
        UpdatedDecks?.Invoke();
    }
    
    public void DeleteSelectedDeck()
    {
        if (decks.Count==1)
        {
            DialogsManager.Instance.OkDialog.Setup("You need to have latest 1 deck");
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

        if (string.IsNullOrEmpty(_name))
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

    public List<ClaimedLevelReward> ClaimedLevelProgressRewards => claimedLevelProgressRewards;

    public void AddQoomon(int _qoomonId)
    {
        if (DataManager.Instance.PlayerData.OwnedQoomons.Contains(_qoomonId))
        {
            return;
        }
        
        ownedQoomons.Add(_qoomonId);
        UpdatedOwnedQoomons?.Invoke();
    }

    public void ClaimedLevelReward(ClaimedLevelReward _reward)
    {
        DataManager.Instance.PlayerData.ClaimedLevelProgressRewards.Add(_reward);
        UpdatedClaimedLevelRewards?.Invoke();
    }
    
    public int WeeklyLoginAmount
    {
        get => weeklyLoginAmount;
        set
        {
            weeklyLoginAmount = value;
            if (weeklyLoginAmount>7)
            {
                weeklyLoginAmount = 7;
            }
            UpdatedWeeklyLoginAmount?.Invoke();
        }
    }

    public DateTime LastDayConnected
    {
        get => lastDayConnected;
        set
        {
            lastDayConnected = value;
            UpdatedLastDayConnected?.Invoke();
        }
    }

    public int DaysConnectedInRow
    {
        get => daysConnectedInRow;
        set
        {
            daysConnectedInRow = value;
            UpdatedDaysConnectedInRow?.Invoke();
        }
    }

    public int RankPoints
    {
        get => rankPoints;
        set
        {
            rankPoints = value;
            if (rankPoints<0)
            {
                rankPoints = 0;
            }
            UpdatedRankPoints?.Invoke();
        }
    }

    public int AmountOfRankGamesPlayed
    {
        get => amountOfRankGamesPlayed;
        set
        {
            amountOfRankGamesPlayed = value;
            UpdatedAmountOfRankGamesPlayed?.Invoke();
        }
    }

    public List<int> ClaimedRankRewards
    {
        get => claimedRankRewards;
        set => claimedRankRewards = value;
    }

    public void ClaimRankReward(int _rewardNumber)
    {
        claimedRankRewards.Add(_rewardNumber);
        UpdatedClaimedRankRewards?.Invoke();
    }
    
    public void ClaimReward(ItemType _type, int _value)
    {
        if (_type == ItemType.Qoomon)
        {
            AddQoomon(_value);
            return;
        }

        switch (_type)
        {
            case ItemType.None:
                break;
            case ItemType.Exp:
                Exp += _value;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_type), _type, null);
        }
    }

    public List<MissionProgress> MissionsProgress
    {
        get => missionProgresses;
        set => missionProgresses = value;
    }

    public DateTime NextDailyChallenges
    {
        get => nextDailyChallenges;
        set
        {
            nextDailyChallenges = value;
            UpdatedNextDailyChallenges?.Invoke();
        }
    }

    public List<int> ClaimedLoginRewards
    {
        get => claimedLoginRewards;
        set => claimedLoginRewards = value;
    }

    public void AddClaimedLoginReward(int _number)
    {
        claimedLoginRewards.Add(_number);
        UpdatedLoginRewards?.Invoke();
    }

    public int IsDemoPlayer
    {
        get => isDemoPlayer;
        set
        {
            isDemoPlayer = value;
            UpdatedIsDemoPlayer?.Invoke();
        }
    }

    public int HasFinishedTutorial
    {
        get => hasFinishedTutorial;
        set
        {
            hasFinishedTutorial = value;
            UpdatedHasFinishedTutorial?.Invoke();
        }
    }

    public bool PlayBackgroundMusic
    {
        get => playBackgroundMusic;
        set
        {
            playBackgroundMusic = value;
            UpdatedBackgroundMusic?.Invoke();
        }
    }

    public bool PlaySoundEffects
    {
        get => playSoundEffects;
        set
        {
            playSoundEffects = value;
            UpdatedPlaySoundEffects?.Invoke();
        }
    }

    public int GetQoomonFromPool()
    {
        List<int> _possibleQoomons = new () { 2, 6, 10, 12, 13, 15, 20, 22, 24, 25, 26, 27, 31, 32, 33, 35, 36, 38, 39, 40, 41, 41, 42, 43, 43, 44, 46};
        foreach (var _qoomon in _possibleQoomons.OrderBy(_ => Guid.NewGuid()))
        {
            if (ownedQoomons.Contains(_qoomon))
            {
                continue;
            }

            return _qoomon;
        }
        return -1;
    }

    public bool HasClaimedLevelReward(int _level)
    {
        return GetClaimedLevelReward(_level) != null;
    }

    public ClaimedLevelReward GetClaimedLevelReward(int _level)
    {
        foreach (ClaimedLevelReward _claimedLevelReward in claimedLevelProgressRewards)
        {
            if (_claimedLevelReward.Level==_level)
            {
                return _claimedLevelReward;
            }
        }

        return null;
    }

}