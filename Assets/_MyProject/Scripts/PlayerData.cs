using System.Collections.Generic;

public class PlayerData
{
    public List<int> CardIdsIndeck { get; private set; }

    public void Init()
    {
        CardIdsIndeck = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        //CardIdsIndeck = new List<int>() { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
    }
}
