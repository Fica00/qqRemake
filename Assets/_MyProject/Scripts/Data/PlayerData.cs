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
    private List<ClaimedLevelReward> claimedLevelProgressRewards = new();
    private int weeklyLoginAmount = 1;
    private DateTime lastDayConnected;
    private int daysConnectedInRow;
    private int rankPoints;
    private int amountOfRankGamesPlayed;
    private List<int> claimedRankRewards = new();
    private List<int> claimedLoginRewards = new();
    private List<MissionProgress> missionProgresses = new();
    private DateTime nextDailyChallenges;
    private int isDemoPlayer;
    private int hasFinishedTutorial;
    private bool playBackgroundMusic = true;
    private bool playSoundEffects = true;
    private string version;
    private List<DeviceData> devices = new();
    private string userWalletAddress;
    private bool didRequestUserWallet;
    private string agency;
    private bool isGuest;
    private bool isNewAccount;
    private bool hasPlayedFirstGame;
    private bool hasFinishedFirstGame;
    private bool hasPickedUpPwaReward;
    private PlayerStatistics statistics = new ();
    private int selectedAvatar;
    private List<int> ownedAvatars = new List<int>(){0,1,2,3};

    private bool beforeFirstGameOverlayShown;
    private bool afterFirstGameOverlayShown;
    private bool guestOverlayShown;
    private PwaOverlay settingsFirstTimeShown;

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
    public static Action UpdatedVersion;
    public static Action UpdatedPlayerDevices;
    public static Action UpdatedUserWalletAddress;
    public static Action UpdatedDidRequestUserWallet;
    public static Action UpdatedAgency;
    public static Action UpdatedIsGuest;
    public static Action UpdatedBeforeFirstGameOverlayShown;
    public static Action UpdatedAfterFirstGameOverlayShown;
    public static Action UpdatedGuestOverlayShown;
    public static Action UpdatedIsNewAccount;
    public static Action UpdatedHasPlayedFirstGame;
    public static Action UpdatedHasPickedUpPwaReward;
    public static Action UpdatedSettingsFirstTimeShown;
    public static Action UpdatedStatistics;
    public static Action UpdatedSelectedAvatar;
    public static Action UpdatedOwnedAvatars;
    
    public void CreateNewPlayer()
    {
        name = "Player" + UnityEngine.Random.Range(0, 10000);
        selectedDeck = 0;
        DateCreatedAccount = DateTime.UtcNow.Date;
        lastDayConnected = DateCreatedAccount;
        daysConnectedInRow = 1;

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
        DeckData starterDeck = DeckInitializer.InitializeDecks().First(deck => deck.Id == 0);
        decks.Add(starterDeck);
        ownedQoomons.AddRange(starterDeck.CardsInDeck);
    }

    private void Setup()
    {
        decks = DeckInitializer.InitializeDecks();

        foreach (var _card in CardsManager.Instance.GetAllPlayableCards())
        {
            ownedQoomons.Add(_card.Details.Id);
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
        statistics.NoteFirstDeckUpdate("Added deck");
        UpdatedDecks?.Invoke();
    }

    public void DeleteSelectedDeck()
    {
        if (decks.Count == 1)
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
        statistics.NoteFirstDeckUpdate("Added card");
        UpdatedCardsInDeck?.Invoke();
    }

    public void RemoveCardFromSelectedDeck(int _cardId)
    {
        DeckData _deck = decks.Find(_deck => _deck.Id == selectedDeck);
        _deck.CardsInDeck.Remove(_cardId);
        statistics.NoteFirstDeckUpdate("Removed card");
        UpdatedCardsInDeck?.Invoke();
    }

    public void UpdateDeckName(string _name)
    {
        DeckData _deck = GetSelectedDeck();
        if (_deck == null)
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
        statistics.NoteFirstDeckUpdate("Changed name");
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

    public bool HasPickedUpPwaReward
    {
        get => hasPickedUpPwaReward;
        set
        {
            hasPickedUpPwaReward = value;
            UpdatedHasPickedUpPwaReward?.Invoke();
        }
    }

    public bool IsGuest
    {
        get => isGuest;
        set
        {
            isGuest = value;
            UpdatedIsGuest?.Invoke();
        }
    }
    
    public bool HasPlayedFirstGame
    {
        get => hasPlayedFirstGame;
        set
        {
            hasPlayedFirstGame = value;
            UpdatedHasPlayedFirstGame?.Invoke();
        }
    }
    
    public PwaOverlay SettingsFirstTimeShown
    {
        get => settingsFirstTimeShown;
        set
        {
            settingsFirstTimeShown = value;
            UpdatedSettingsFirstTimeShown?.Invoke();
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


    public int Exp
    {
        get => exp;
        set
        {
            exp = value;
            if (exp>DataManager.Instance.GameData.ExpBorders[^1])
            {
                exp = DataManager.Instance.GameData.ExpBorders[^1];
            }
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
                if (exp < DataManager.Instance.GameData.ExpBorders[_level])
                {
                    return _level;
                }

                _level++;
                if (_level >= DataManager.Instance.GameData.ExpBorders.Count)
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
            if (IsMaxLevel)
            {
                return 1.0f;
            }

            int _currentLevelXp = Level == 0 ? exp : Exp - GetXpForLevel(Level - 1);
            int _nextLevelXp = GetXpForNextLevel();
            return (float)_currentLevelXp / _nextLevelXp;
        }
    }

    [JsonIgnore]
    public int CurrentExpOnLevel
    {
        get
        {
            if (IsMaxLevel)
            {
                return 1;
            }

            if (Level == 0)
            {
                return exp;
            }

            return exp - GetXpForLevel(Level - 1);
        }
    }

    public int GetXpForNextLevel()
    {
        if (Level == 0)
        {
            return DataManager.Instance.GameData.ExpBorders[Level];
        }

        if (IsMaxLevel)
        {
            return 1;
        }
        
        return DataManager.Instance.GameData.ExpBorders[Level] - DataManager.Instance.GameData.ExpBorders[Level - 1];
    }

    public bool IsMaxLevel => Level > DataManager.Instance.GameData.ExpBorders.Count - 1;

    public int GetXpForLevel(int _level)
    {
        if (IsMaxLevel)
        {
            return 1;
        }
        return DataManager.Instance.GameData.ExpBorders[_level];
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
            if (weeklyLoginAmount > 7)
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
            if (rankPoints < 0)
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

    public string Agency
    {
        get => agency;
        set
        {
            agency = value;
            UpdatedAgency?.Invoke();
        }
    }
    
    public bool IsNewAccount
    {
        get => isNewAccount;
        set
        {
            isNewAccount = value;
            UpdatedIsNewAccount?.Invoke();
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
    
    public bool HasFinishedFirstGame
    {
        get => hasFinishedFirstGame;
        set => hasFinishedFirstGame = value;
    }
    
    public bool BeforeFirstGameOverlayShown
    {
        get => beforeFirstGameOverlayShown;
        set
        {
            beforeFirstGameOverlayShown = value;
            UpdatedBeforeFirstGameOverlayShown?.Invoke();
        }
    }
    public bool AfterFirstGameOverlayShown
    {
        get => afterFirstGameOverlayShown;
        set
        {
            afterFirstGameOverlayShown = value;
            UpdatedAfterFirstGameOverlayShown?.Invoke();
        }
    }

    public bool GuestOverlayShown
    {
        get => guestOverlayShown;
        set
        {
            guestOverlayShown = value;
            UpdatedGuestOverlayShown?.Invoke();
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
        List<int> _possibleQoomons = new()
        {
            2,
            6,
            10,
            12,
            13,
            15,
            20,
            22,
            24,
            25,
            26,
            27,
            31,
            32,
            33,
            35,
            36,
            38,
            39,
            40,
            41,
            41,
            42,
            43,
            43,
            44,
            46
        };
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
            if (_claimedLevelReward.Level == _level)
            {
                return _claimedLevelReward;
            }
        }

        return null;
    }

    public string Version
    {
        get => version;
        set
        {
            version = value;
            UpdatedVersion?.Invoke();
        }
    }

    public List<DeviceData> Devices
    {
        get => devices;
        set => devices = value;
    }

    public void AddDeviceData()
    {
        DeviceData _data = DeviceData.Get();

        if (devices.Any(_device => _device.UniqueIdentifier == _data.UniqueIdentifier))
        {
            return;
        }

        Devices.Add(_data);
        UpdatedPlayerDevices?.Invoke();
    }

    public string UserWalletAddress
    {
        get => userWalletAddress;
        set
        {
            userWalletAddress = value;
            UpdatedUserWalletAddress?.Invoke();
        }
    }

    public bool DidRequestUserWallet
    {
        get => didRequestUserWallet;
        set
        {
            didRequestUserWallet = value;
            UpdatedDidRequestUserWallet?.Invoke();
        }
    }

    public PlayerStatistics Statistics
    {
        get => statistics;
        set
        {
            statistics = value;
            UpdatedStatistics?.Invoke();
        }
    }

    public int SelectedAvatar
    {
        get => selectedAvatar;
        set
        {
            selectedAvatar = value;
            UpdatedSelectedAvatar?.Invoke();
        }
    }

    public List<int> OwnedAvatars
    {
        get => ownedAvatars;
        set => ownedAvatars = value;
    }

    public void AddOwnedAvatars(int _avatar)
    {
        if (ownedAvatars.Contains(_avatar))
        {
            return;
        }

        ownedAvatars.Add(_avatar);
        UpdatedOwnedAvatars?.Invoke();
    }
}