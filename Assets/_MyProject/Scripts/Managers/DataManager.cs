using Newtonsoft.Json;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public PlayerData PlayerData { get; private set; }
    public GameData GameData { get; private set; }

    public int[] locationsPicked = {-1, -1, -1};

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
    }
}
