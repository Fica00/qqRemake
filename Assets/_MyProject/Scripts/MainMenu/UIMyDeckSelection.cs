using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMyDeckSelection : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown chosenDeckInput;

    private void OnEnable()
    {
        chosenDeckInput.onValueChanged.AddListener(ChoseDeck);
        CollectionPanel.OnClosed += ChangeDropdownOptions;
    }

    private void OnDisable()
    {
        chosenDeckInput.onValueChanged.AddListener(ChoseDeck);
        CollectionPanel.OnClosed -= ChangeDropdownOptions;
    }

    private void Start()
    {
        ChangeDropdownOptions();
        ChoseDeck(0);
    }
    
    private void ChangeDropdownOptions()
    {
        chosenDeckInput.options.Clear();

        foreach (var _deck in DataManager.Instance.PlayerData.Decks)
        {
            chosenDeckInput.options.Add(new TMP_Dropdown.OptionData(_deck.Name));
        }

        chosenDeckInput.RefreshShownValue();
    }

    private void ChoseDeck(int _deckId)
    {
        DataManager.Instance.PlayerData.SelectedDeck = _deckId;
    }
}
