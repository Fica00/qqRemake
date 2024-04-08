using System;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static EventsManager Instance;
    public static Action DrawCard;
    public static Action PlayCard;
    public static Action WinALocationWithPowerLess100;
    public static Action WinALocationWithPowerMore200;
    public static Action WinALocationWith1Card;
    public static Action WinALocationWith4Card;
    public static Action WinMatch;
    public static Action PlayCardCost1;
    public static Action PlayCardCost2;
    public static Action PlayCardCost3;
    public static Action PlayCardCost4;
    public static Action PlayCardCost5;
    public static Action PlayCardCost6;
    public static Action<int> PlayCardsOfPowerWorth;
    public static Action WinMatchWithADouble;
    

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
