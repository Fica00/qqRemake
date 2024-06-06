using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static Action NewGameDay;
    public static DataManager Instance;
    public PlayerData PlayerData { get; private set; } = null;
    public GameData GameData { get; private set; }

    public int[] locationsPicked = {-1, -1, -1};

    private bool isSubscribed;
    public bool IsGuestOverlayShown;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreatePlayerDataEmpty()
    {
        PlayerData = new PlayerData();
        PlayerData.CreateNewPlayer();
    }

    public void SetGameData(string _data)
    {
        GameData = JsonConvert.DeserializeObject<GameData>(_data);
    }

    public void SetPlayerData(string _data)
    {
        if (PlayerData!=null)
        {
            return;
        }
        
        PlayerData = JsonConvert.DeserializeObject<PlayerData>(_data);
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    public void Subscribe()
    {
        if (isSubscribed)
        {
            return;
        }

        isSubscribed = true;
        PlayerData.UpdatedSelectedDeck += SaveSelectedDeck;
        PlayerData.UpdatedCardsInDeck += SaveOwnedDecks;
        PlayerData.UpdatedName += SaveName;
        PlayerData.UpdatedDecks += SaveOwnedDecks;
        PlayerData.UpdatedDeckName += SaveDeckName;
        PlayerData.UpdatedGamePasses += SaveGamePasses;
        PlayerData.UpdatedCoins += SaveCoins;
        PlayerData.UpdatedUSDC += SaveUSDC;
        PlayerData.UpdatedExp += SaveExp;
        PlayerData.UpdatedOwnedQoomons += SaveOwnedQommons;
        PlayerData.UpdatedClaimedLevelRewards += SaveClaimedRewards;
        PlayerData.UpdatedWeeklyLoginAmount += SaveWeeklyCount;
        PlayerData.UpdatedLastDayConnected += SaveLastDayConnected;
        PlayerData.UpdatedDaysConnectedInRow += SaveDaysConnectedInARow;
        PlayerData.UpdatedRankPoints += SaveRankPoints;
        PlayerData.UpdatedAmountOfRankGamesPlayed += SaveAmountOfRankGamesPlayed;
        PlayerData.UpdatedClaimedRankRewards += SaveClaimedRankRewards;
        PlayerData.UpdatedNextDailyChallenges += SaveNextDailyChallenges;
        MissionManager.OnClaimed += SaveMissions;
        MissionManager.OnGeneratedNewChallenges += SaveMissions;
        MissionProgress.UpdatedProgress += SaveProgress;
        PlayerData.UpdatedLoginRewards += SaveLoginRewards;
        PlayerData.UpdatedIsDemoPlayer += SaveIsDemoPlayer;
        PlayerData.UpdatedBackgroundMusic += SaveBackgroundMusic;
        PlayerData.UpdatedPlaySoundEffects += SaveSoundEffects;
        PlayerData.UpdatedHasFinishedTutorial += SaveTutorial;
        PlayerData.UpdatedVersion += SaveVersion;
        PlayerData.UpdatedPlayerDevices += SaveDevices;
        PlayerData.UpdatedDidRequestUserWallet += SaveWalletRequest;
        PlayerData.UpdatedAgency += SaveAgency;
        PlayerData.UpdatedIsGuest += SaveIsGuest;
        PlayerData.UpdatedBeforeFirstGameOverlayShown += SaveBeforeFirstGameOverlayShown;
        PlayerData.UpdatedAfterFirstGameOverlayShown += SaveAfterFirstGameOverlayShown;
        PlayerData.UpdatedGuestOverlayShown += SaveGuestOverlayShown;
        PlayerData.UpdatedIsNewAccount += SaveIsNewAccount;
        PlayerData.UpdatedHasPlayedFirstGame += SaveHasPlayedFirstGame;
        PlayerData.UpdatedStatistics += SaveStatistics;

        StartCoroutine(CheckForNewGameDay());
        PlayerData.IsDemoPlayer = JavaScriptManager.Instance.IsDemo ? 1 : 0;
        PlayerData.Version = JavaScriptManager.Instance.Version;
        PlayerData.AddDeviceData();
    }
    
    IEnumerator CheckForNewGameDay()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            DateTime _nextDate = PlayerData.LastDayConnected.AddDays(1);
            DateTime _currentDate = DateTime.UtcNow.Date;
            
            if (_nextDate>_currentDate)
            {
                continue;
            }

            if ((_nextDate-_currentDate).TotalDays<1)
            {
                PlayerData.DaysConnectedInRow++;
            }
            else
            {
                PlayerData.DaysConnectedInRow = 1;
            }
        
            PlayerData.LastDayConnected = DateTime.UtcNow.Date;
            if (PlayerData.LastDayConnected.DayOfWeek == DayOfWeek.Monday)
            {
                PlayerData.WeeklyLoginAmount = 1;
                PlayerData.ClaimedLoginRewards.Clear();
                PlayerData.UpdatedLoginRewards?.Invoke();
            }
            PlayerData.WeeklyLoginAmount++;
            NewGameDay?.Invoke();
        }
    }

    private void Unsubscribe()
    {
        if (!isSubscribed)
        {
            return;
        }
        
        isSubscribed = false;
        PlayerData.UpdatedSelectedDeck -= SaveSelectedDeck;
        PlayerData.UpdatedCardsInDeck -= SaveOwnedDecks;
        PlayerData.UpdatedName -= SaveName;
        PlayerData.UpdatedDecks -= SaveOwnedDecks;
        PlayerData.UpdatedDeckName -= SaveDeckName;
        PlayerData.UpdatedGamePasses -= SaveGamePasses;
        PlayerData.UpdatedCoins -= SaveCoins;
        PlayerData.UpdatedUSDC -= SaveUSDC;
        PlayerData.UpdatedExp -= SaveExp;
        PlayerData.UpdatedOwnedQoomons -= SaveOwnedQommons;
        PlayerData.UpdatedClaimedLevelRewards -= SaveClaimedRewards;
        PlayerData.UpdatedWeeklyLoginAmount -= SaveWeeklyCount;
        PlayerData.UpdatedLastDayConnected -= SaveLastDayConnected;
        PlayerData.UpdatedDaysConnectedInRow -= SaveDaysConnectedInARow;
        PlayerData.UpdatedRankPoints -= SaveRankPoints;
        PlayerData.UpdatedAmountOfRankGamesPlayed -= SaveAmountOfRankGamesPlayed;
        PlayerData.UpdatedClaimedRankRewards -= SaveClaimedRankRewards;
        PlayerData.UpdatedNextDailyChallenges -= SaveNextDailyChallenges;
        MissionManager.OnClaimed -= SaveMissions;
        MissionManager.OnGeneratedNewChallenges -= SaveMissions;
        MissionProgress.UpdatedProgress -= SaveProgress;
        PlayerData.UpdatedLoginRewards -= SaveLoginRewards;
        PlayerData.UpdatedIsDemoPlayer -= SaveIsDemoPlayer;
        PlayerData.UpdatedBackgroundMusic -= SaveBackgroundMusic;
        PlayerData.UpdatedPlaySoundEffects -= SaveSoundEffects;
        PlayerData.UpdatedHasFinishedTutorial -= SaveTutorial;
        PlayerData.UpdatedVersion -= SaveVersion;
        PlayerData.UpdatedPlayerDevices -= SaveDevices;
        PlayerData.UpdatedDidRequestUserWallet -= SaveWalletRequest;
        PlayerData.UpdatedAgency -= SaveAgency;
        PlayerData.UpdatedIsGuest -= SaveIsGuest;
        PlayerData.UpdatedBeforeFirstGameOverlayShown -= SaveBeforeFirstGameOverlayShown;
        PlayerData.UpdatedAfterFirstGameOverlayShown -= SaveAfterFirstGameOverlayShown;
        PlayerData.UpdatedGuestOverlayShown -=  SaveGuestOverlayShown;
        PlayerData.UpdatedIsNewAccount -= SaveIsNewAccount;
        PlayerData.UpdatedHasPlayedFirstGame -= SaveHasPlayedFirstGame;
        PlayerData.UpdatedStatistics -= SaveStatistics;
    }

    private void SaveTutorial()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.HasFinishedTutorial),PlayerData.HasFinishedTutorial);
    }

    private void SaveSelectedDeck()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.SelectedDeck),PlayerData.SelectedDeck);
    }

    private void SaveName()
    {
        FirebaseManager.Instance.SaveString(nameof(PlayerData.Name),PlayerData.Name);
    }    
    
    private void SaveVersion()
    {
        FirebaseManager.Instance.SaveString(nameof(PlayerData.Version),PlayerData.Version);
    }

    private void SaveOwnedDecks()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.Decks),JsonConvert.SerializeObject(PlayerData.Decks));
    }

    private void SaveDevices()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.Devices),JsonConvert.SerializeObject(PlayerData.Devices));
    }

    private void SaveDeckName()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.Decks),JsonConvert.SerializeObject(PlayerData.Decks));
    }

    private void SaveGamePasses()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.GamePasses),JsonConvert.SerializeObject(PlayerData.GamePasses));
    }

    private void SaveCoins()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.Coins),JsonConvert.SerializeObject(PlayerData.Coins));
    }

    private void SaveUSDC()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.USDC),JsonConvert.SerializeObject(PlayerData.USDC));
    }    
    
    private void SaveExp()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.Exp),JsonConvert.SerializeObject(PlayerData.Exp));
    }
    
    private void SaveOwnedQommons()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.OwnedQoomons),JsonConvert.SerializeObject(PlayerData.OwnedQoomons));
    }
    
    private void SaveClaimedRewards()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.ClaimedLevelProgressRewards),JsonConvert.SerializeObject(PlayerData.ClaimedLevelProgressRewards));
    }
    
    private void SaveWeeklyCount()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.WeeklyLoginAmount),PlayerData.WeeklyLoginAmount);
    }
    
    private void SaveLastDayConnected()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.LastDayConnected),JsonConvert.SerializeObject(PlayerData.LastDayConnected));
    }

    private void SaveDaysConnectedInARow()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.DaysConnectedInRow),JsonConvert.SerializeObject(PlayerData.DaysConnectedInRow));
    }

    private void SaveRankPoints()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.RankPoints),JsonConvert.SerializeObject(PlayerData.RankPoints));
    }
    
    private void SaveAmountOfRankGamesPlayed()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.AmountOfRankGamesPlayed),PlayerData.AmountOfRankGamesPlayed);
    }
    
    private void SaveClaimedRankRewards()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.ClaimedRankRewards),JsonConvert.SerializeObject(PlayerData.ClaimedRankRewards));
    }
    
    private void SaveNextDailyChallenges()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.NextDailyChallenges),JsonConvert.SerializeObject(PlayerData.NextDailyChallenges));
    }
    
    private void SaveMissions(MissionProgress _)
    {
        SaveMissions();
    }

    private void SaveMissions()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.MissionsProgress),JsonConvert.SerializeObject(PlayerData.MissionsProgress));
    }
    
    private void SaveIsDemoPlayer()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.IsDemoPlayer),PlayerData.IsDemoPlayer);
    }
    
    private void SaveProgress(int _missionId)
    {
        MissionProgress _missionProgress = PlayerData.MissionsProgress.Find(_mission => _mission.Id == _missionId);
        int _missionIndex = PlayerData.MissionsProgress.IndexOf(_missionProgress);
        
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.MissionsProgress)+"/"+_missionIndex,JsonConvert.SerializeObject(_missionProgress));
    }
    
    private void SaveLoginRewards()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.ClaimedLoginRewards),JsonConvert.SerializeObject(PlayerData.ClaimedLoginRewards));
    }
    
    private void SaveBackgroundMusic()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.PlayBackgroundMusic),JsonConvert.SerializeObject(PlayerData.PlayBackgroundMusic));
    } 
    
    private void SaveWalletRequest()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.DidRequestUserWallet),JsonConvert.SerializeObject(PlayerData.DidRequestUserWallet));
    }

    private void SaveSoundEffects()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.PlaySoundEffects),JsonConvert.SerializeObject(PlayerData.PlaySoundEffects));
    }
    
    private void SaveAgency()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.Agency),JsonConvert.SerializeObject(PlayerData.Agency));
    }
    
    public void SaveIsGuest()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.IsGuest),JsonConvert.SerializeObject(PlayerData.IsGuest));
    }
    
    private void SaveIsNewAccount()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.IsNewAccount),JsonConvert.SerializeObject(PlayerData.IsNewAccount));
    }
    
    private void SaveBeforeFirstGameOverlayShown()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.BeforeFirstGameOverlayShown),JsonConvert.SerializeObject(PlayerData.BeforeFirstGameOverlayShown));
    }
    
    private void SaveGuestOverlayShown()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.GuestOverlayShown),JsonConvert.SerializeObject(PlayerData.GuestOverlayShown));
    }
    private void SaveAfterFirstGameOverlayShown()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.AfterFirstGameOverlayShown),JsonConvert.SerializeObject(PlayerData.AfterFirstGameOverlayShown));
    }
    
    private void SaveHasPlayedFirstGame()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.HasPlayedFirstGame),JsonConvert.SerializeObject(PlayerData.HasPlayedFirstGame));
    }
    
    private void SaveStatistics()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.Statistics),JsonConvert.SerializeObject(PlayerData.Statistics));
    }
}
