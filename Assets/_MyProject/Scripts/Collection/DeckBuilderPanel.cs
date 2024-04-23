using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuilderPanel : BasePanel
{
    [SerializeField] private InputField nameInput;
    [SerializeField] private Button backButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private GameObject holder;
    [SerializeField] private CollectionQommonDisplay[] qommonDisplays;
    [SerializeField] private CollectionQommonDisplay qommonPrefab;
    [SerializeField] private Transform collectionHolder;
    [SerializeField] private DeckBuildQommonDetails qommonDetails;
    [SerializeField] private CollectionPanel collectionPanel;

    private List<GameObject> showedQommonsInCollection = new();

    private void OnEnable()
    {
        FilterHandler.OnUpdatedFilter += UpdateQoomons;
        backButton.onClick.AddListener(Close);
        CollectionQommonDisplay.OnClicked += ShowDetails;
        CollectionQommonDisplay.OnHold += ChangeDeck;
        DeckBuildQommonDetails.OnChangeDeck += ChangeDeck;
        deleteButton.onClick.AddListener(DeleteDeck);
    }

    private void OnDisable()
    {
        FilterHandler.OnUpdatedFilter -= UpdateQoomons;
        backButton.onClick.RemoveListener(Close);
        CollectionQommonDisplay.OnClicked -= ShowDetails;
        CollectionQommonDisplay.OnHold -= ChangeDeck;
        DeckBuildQommonDetails.OnChangeDeck -= ChangeDeck;
        DataManager.Instance.PlayerData.UpdateDeckName(nameInput.text);
        deleteButton.onClick.RemoveListener(DeleteDeck);
    }

    private void UpdateQoomons()
    {
        ShowQommonsInCollection();
    }

    private void ShowDetails(int _cardId)
    {
        qommonDetails.Setup(_cardId);
    }

    private void ChangeDeck(int _cardId)
    {
        if (DataManager.Instance.PlayerData.CardIdsInDeck.Contains(_cardId))
        {
            RemoveQommon(_cardId);
        }
        else
        {
            AddQommon(_cardId);
        }
    }
    
    private void RemoveQommon(int _cardId)
    {
        DataManager.Instance.PlayerData.RemoveCardFromSelectedDeck(_cardId);
        Show(DataManager.Instance.PlayerData.SelectedDeck);
    }

    private void AddQommon(int _cardId)
    {
        if (DataManager.Instance.PlayerData.CardIdsInDeck.Count>=12)
        {
            DialogsManager.Instance.OkDialog.Setup("Your lineup is full!");
            return;
        }

        DataManager.Instance.PlayerData.AddCardToSelectedDeck(_cardId);
        Show(DataManager.Instance.PlayerData.SelectedDeck);
    }

    public void Show(int _deckId)
    {
        DataManager.Instance.PlayerData.SelectedDeck = _deckId;
        nameInput.text = DataManager.Instance.PlayerData.GetSelectedDeck().Name;
        ShowQommonsInDeck();
        ShowQommonsInCollection();
        qommonDetails.Close();
        holder.SetActive(true);
    }

    private void ShowQommonsInDeck()
    {
        ClearQommonsInDeck();

        int _counter = 0;
        foreach (var _cardInDeck in Utils.OrderQommons(DataManager.Instance.PlayerData.CardIdsInDeck))
        {
            qommonDisplays[_counter].Setup(_cardInDeck.Details.Id);
            _counter++;
        }
    }

    private void ClearQommonsInDeck()
    {
        foreach (var _qommonDisplay in qommonDisplays)
        {
            _qommonDisplay.SetupEmpty();
        }
    }
    
    private void ShowQommonsInCollection()
    {
        ClearQommonsInCollection();
        
        foreach (var _qommon in Utils.OrderQommons(DataManager.Instance.PlayerData.OwnedQoomons))
        {
            CollectionQommonDisplay _qommonDisplay = Instantiate(qommonPrefab, collectionHolder);
            _qommonDisplay.Setup(_qommon.Details.Id,true);
            showedQommonsInCollection.Add(_qommonDisplay.gameObject);
        }   
    }
    
    private void ClearQommonsInCollection()
    {
        foreach (var _shownQommon in showedQommonsInCollection)
        {
            Destroy(_shownQommon);
        }
        
        showedQommonsInCollection?.Clear();
    }

    public override void Close()
    {
        DataManager.Instance.PlayerData.UpdateDeckName(nameInput.text);
        holder.SetActive(false);
        collectionPanel.SubscribeForQommonDetails();
    }
    
    private void DeleteDeck()
    {
        DataManager.Instance.PlayerData.DeleteSelectedDeck();
        Close();
    }
}
