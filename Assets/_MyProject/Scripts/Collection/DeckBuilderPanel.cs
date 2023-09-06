using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuilderPanel : BasePanel
{
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private Button backButton;
    [SerializeField] private CollectionQommonDisplay[] qommonDisplays;
    [SerializeField] private CollectionQommonDisplay qommonPrefab;
    [SerializeField] private Transform collectionHolder;
    [SerializeField] private DeckBuildQommonDetails qommonDetails;

    private List<GameObject> showedQommonsInCollection = new();

    private void OnEnable()
    {
        backButton.onClick.AddListener(Close);
        CollectionQommonDisplay.OnClicked += ShowDetails;
        DeckBuildQommonDetails.OnAddCardToDeck += AddCardToDeck;
    }

    private void OnDisable()
    {
        backButton.onClick.RemoveListener(Close);
        CollectionQommonDisplay.OnClicked -= ShowDetails;
        DeckBuildQommonDetails.OnAddCardToDeck -= AddCardToDeck;
    }

    private void ShowDetails(int _cardId)
    {
        if (DataManager.Instance.PlayerData.CardIdsInDeck.Contains(_cardId))
        {
            UIManager.Instance.YesNoDialog.OnNoPressed.AddListener(ShowQommon);
            UIManager.Instance.YesNoDialog.OnYesPressed.AddListener(RemoveQommon);
            UIManager.Instance.YesNoDialog.Setup("Remove qommon from the deck?");
        }
        else
        {
            ShowQommon();
        }


        void ShowQommon()
        {
            qommonDetails.Setup(_cardId);
        }
        
        void RemoveQommon()
        {
            if (DataManager.Instance.PlayerData.CardIdsInDeck.Contains(_cardId))
            {
                DataManager.Instance.PlayerData.RemoveCardFromSelectedDeck(_cardId);
            }

            Show(DataManager.Instance.PlayerData.SelectedDeck);
        }
    }

    private void AddCardToDeck(int _cardId)
    {
        if (DataManager.Instance.PlayerData.CardIdsInDeck.Count>=12)
        {
            UIManager.Instance.OkDialog.Setup("Your lineup is full!");
            return;
        }

        DataManager.Instance.PlayerData.AddCardToSelectedDeck(_cardId);
        Show(DataManager.Instance.PlayerData.SelectedDeck);
    }

    public void Show(int _deckId)
    {
        DataManager.Instance.PlayerData.SelectedDeck = _deckId;
        nameDisplay.text = "Lineup " + _deckId;
        ShowQommonsInDeck();
        ShowQommonsInCollection();
        qommonDetails.Close();
        gameObject.SetActive(true);
    }

    private void ShowQommonsInDeck()
    {
        ClearQommonsInDeck();

        int _counter = 0;
        foreach (var _cardInDeck in Helpers.OrderQommons(DataManager.Instance.PlayerData.CardIdsInDeck))
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
        
        foreach (var _qommon in Helpers.OrderQommons(DataManager.Instance.PlayerData.OwnedQommons))
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
        gameObject.SetActive(false);
    }
}
