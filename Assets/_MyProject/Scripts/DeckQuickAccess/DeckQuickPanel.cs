using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckQuickPanel : MonoBehaviour
{
    [SerializeField] private Transform holder;
    [SerializeField] private DeckQuickDisplay quickDisplay;
    [SerializeField] private Button close;
    [SerializeField] private CollectionPanel collectionPanel;
    private List<GameObject> showObjects = new();

    public void Setup()
    {
        foreach (var _showObject in showObjects)
        {
            Destroy(_showObject);
        }
        showObjects.Clear();
        
        foreach (var _ownedDeck in DataManager.Instance.PlayerData.Decks)
        {
            DeckQuickDisplay _deck = Instantiate(quickDisplay, holder);
            _deck.Setup(_ownedDeck);
            showObjects.Add(_deck.gameObject);
        }
        
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        close.onClick.AddListener(Close);
        DeckQuickDisplay.OnSelectDeck += SelectDeck;
        DeckQuickDisplay.OnEditDeck += EditDeck;
    }

    private void OnDisable()
    {
        close.onClick.RemoveListener(Close);
        DeckQuickDisplay.OnSelectDeck -= SelectDeck;
        DeckQuickDisplay.OnEditDeck -= EditDeck;
    }

    private void SelectDeck(int _deckId)
    {
        DataManager.Instance.PlayerData.SelectedDeck = _deckId;
        Close();
    }
    
    private void EditDeck(int _deckId)
    {
        collectionPanel.EditDeckShortcut(_deckId);
        Close();
    }
    
    private void Close()
    {
        gameObject.SetActive(false);
    }
}
