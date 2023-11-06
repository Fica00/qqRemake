using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class DataManager : MonoBehaviour
{
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
        GameData = JsonUtilities.ConvertJsonToGameData(_data);
    }

    public void SetPlayerData(string _data)
    {
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
        PlayerData.BoughtNewDeck += SaveOwnedDecks;
        PlayerData.UpdatedDeckName += SaveDeckName;
        PlayerData.UpdatedGamePasses += SaveGamePasses;
        PlayerData.UpdatedCoins += SaveCoins;
        PlayerData.UpdatedUSDC += SaveUSDC;
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
        PlayerData.BoughtNewDeck -= SaveOwnedDecks;
        PlayerData.UpdatedDeckName -= SaveDeckName;
        PlayerData.UpdatedGamePasses -= SaveGamePasses;
        PlayerData.UpdatedCoins -= SaveCoins;
        PlayerData.UpdatedUSDC -= SaveUSDC;
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
}
