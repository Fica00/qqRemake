using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static Action NewGameDay;
    public static DataManager Instance;
    public PlayerData PlayerData { get; private set; }
    public GameData GameData { get; private set; }

    public int[] locationsPicked = {-1, -1, -1};

    private bool isSubscribed;

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
        PlayerData = JsonConvert.DeserializeObject<PlayerData>(_data);
        StartCoroutine(CheckForNewGameDay());
    }

    IEnumerator CheckForNewGameDay()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            DateTime _nextDate = PlayerData.LastDayConnected.AddDays(1);
            DateTime _currentDate = DateTime.UtcNow.Date;
            if (_nextDate<_currentDate)
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
            NewGameDay?.Invoke();
        }
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
        PlayerData.UpdatedWeeklyMissionCount += SaveWeeklyCount;
        PlayerData.UpdatedLastDayConnected += SaveLastDayConnected;
        PlayerData.UpdatedDaysConnectedInRow += SaveDaysConnectedInARow;
        PlayerData.UpdatedRankPoints += SaveRankPoints;
        PlayerData.UpdatedAmountOfRankGamesPlayed += SaveAmountOfRankGamesPlayed;
        PlayerData.UpdatedClaimedRankRewards += SaveClaimedRankRewards;
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
        PlayerData.UpdatedWeeklyMissionCount -= SaveWeeklyCount;
        PlayerData.UpdatedLastDayConnected -= SaveLastDayConnected;
        PlayerData.UpdatedDaysConnectedInRow -= SaveDaysConnectedInARow;
        PlayerData.UpdatedRankPoints -= SaveRankPoints;
        PlayerData.UpdatedAmountOfRankGamesPlayed -= SaveAmountOfRankGamesPlayed;
        PlayerData.UpdatedClaimedRankRewards -= SaveClaimedRankRewards;
    }

    private void SaveSelectedDeck()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.SelectedDeck),PlayerData.SelectedDeck);
    }

    private void SaveName()
    {
        FirebaseManager.Instance.SaveString(nameof(PlayerData.Name),PlayerData.Name);
    }

    private void SaveOwnedDecks()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.Decks),JsonConvert.SerializeObject(PlayerData.Decks));
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
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.ClaimedLevelRewards),JsonConvert.SerializeObject(PlayerData.ClaimedLevelRewards));
    }
    
    private void SaveWeeklyCount()
    {
        FirebaseManager.Instance.SaveValue(nameof(PlayerData.WeeklyMissionCount),JsonConvert.SerializeObject(PlayerData.WeeklyMissionCount));
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
}
