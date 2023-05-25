using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public PlayerData PlayerData { get; private set; }

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

    public void Init(Action _callback)
    {
        PlayerData = new PlayerData();
        PlayerData.Init();
        _callback?.Invoke();
    }
}
