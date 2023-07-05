using System.Collections.Generic;

public class PlayerData
{
    public List<int> CardIdsInDeck;

    public void Init()
    {
        CardIdsInDeck = new List<int>() { 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
    }
}
