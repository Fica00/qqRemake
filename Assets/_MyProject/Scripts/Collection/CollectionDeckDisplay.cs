using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionDeckDisplay : MonoBehaviour
{
    public static Action<int> OnShowDeck;
    [SerializeField] private TextMeshProUGUI labelDisplay;
    [SerializeField] private Button button;
    private int number;
    
    public void Setup(DeckData _deckData)
    {
        number = _deckData.Id;
        labelDisplay.text = _deckData.Name;
    }

    private void OnEnable()
    {
        button.onClick.AddListener(ShowDeck);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(ShowDeck);
    }

    private void ShowDeck()
    {
        OnShowDeck?.Invoke(number);
    }
}
