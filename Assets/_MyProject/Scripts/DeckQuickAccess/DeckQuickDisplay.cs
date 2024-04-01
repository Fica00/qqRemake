using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckQuickDisplay : MonoBehaviour
{
    public static Action<int> OnSelectDeck;
    public static Action<int> OnEditDeck;
    [SerializeField] private TextMeshProUGUI labelDisplay;
    [SerializeField] private Button selectDeck;
    [SerializeField] private Button editDeck;
    [SerializeField] private GameObject selectedDisplay;
    private int number;
    
    public void Setup(DeckData _deckData)
    {
        number = _deckData.Id;
        labelDisplay.text = _deckData.Name;
        selectedDisplay.SetActive(DataManager.Instance.PlayerData.SelectedDeck==number);
    }

    private void OnEnable()
    {
        selectDeck.onClick.AddListener(SelectDeck);
        editDeck.onClick.AddListener(EditDeck);
    }

    private void OnDisable()
    {
        selectDeck.onClick.RemoveListener(SelectDeck);
        editDeck.onClick.RemoveListener(EditDeck);
    }

    private void SelectDeck()
    {
        OnSelectDeck?.Invoke(number);
    }

    private void EditDeck()
    {
        OnEditDeck?.Invoke(number);
    }
}
