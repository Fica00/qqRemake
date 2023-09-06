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
    
    public void Setup(int _number)
    {
        number = _number;
        labelDisplay.text = "Lineup " + number;
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
