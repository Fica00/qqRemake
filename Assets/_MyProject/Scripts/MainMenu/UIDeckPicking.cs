using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using TMPro;

public class UIDeckPicking : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown chosenDeckInput;
    [SerializeField] private bool pickingForBot;

    private void OnEnable()
    {
        chosenDeckInput.onValueChanged.AddListener(ChoseDeck);
    }

    private void OnDisable()
    {
        chosenDeckInput.onValueChanged.AddListener(ChoseDeck);
    }

    private void Start()
    {
        ChoseDeck(0);
    }

    private void ChoseDeck(int _deckId)
    {
        var _changingDeck= new List<int>();
        switch (_deckId)
        {
            case 0:
                _changingDeck = new List<int>()
                    { 28,8,7,29,5,1,0,11,3,4,21,9};
                break;
            case 1:
                _changingDeck = new List<int>()
                    { 7,1,11,4,14,16,30,17,18,19,31,27};
                break;
            case 2:
                _changingDeck = new List<int>()
                    { 8,1,0,3,9,10,12,15,24,32,35,13};
                break;
            case 3:
                _changingDeck = new List<int>()
                    { 28,10,20,37,46,42,33,25,36,2,45,27};
                break;
            case 4:
                _changingDeck = new List<int>()
                    { 4,21,6,20,22,42,38,39,40,41,43,44};
                break;
        }

        if (pickingForBot)
        {
            BotPlayer.CardsInDeck = _changingDeck;
        }
        else
        {
            DataManager.Instance.PlayerData.CardIdsInDeck = _changingDeck;
        }
    }
}
