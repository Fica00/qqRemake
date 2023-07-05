using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UIDeckPicking : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown chosenDeckInput;

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
        switch (_deckId)
        {
            case 0:
                DataManager.Instance.PlayerData.CardIdsInDeck = new List<int>()
                    { 28,8,7,29,5,1,0,11,3,4,21,9};
                break;
            case 1:
                DataManager.Instance.PlayerData.CardIdsInDeck = new List<int>()
                    { 7,1,11,4,14,16,30,17,18,19,27,31};
                break;
        }
    }
}
