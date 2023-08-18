using System;
using System.Collections.Generic;

public class PlayerData
{
    private string name;
    private List<int> cardsInDeck;

    public Action UpdatedName;

    public PlayerData()
    {
        name = "Player" + UnityEngine.Random.Range(0, 10000);
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

    public List<int> CardIdsInDeck
    {
        get => cardsInDeck;
        set => cardsInDeck=value;
    }
}
