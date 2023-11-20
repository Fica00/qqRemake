using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionPanel : BasePanel
{
    public static Action OnClosed;
    [SerializeField] private DeckBuilderPanel deckBuilderPanel;
    [SerializeField] private CollectionDeckDisplay deckPrefab;
    [SerializeField] private CollectionQommonDisplay qommonPrefab;
    [SerializeField] private Transform deckHolder;
    [SerializeField] private Transform qommonsHolder;
    [SerializeField] private Button buyMoreDecks;
    [SerializeField] private Button showNextDeck;
    [SerializeField] private Button closeButton;
    [SerializeField] private CollectionQommonDisplayFullScreen qommonDisplay;

    private List<GameObject> shownDecks = new ();
    private List<GameObject> shownQommons = new ();
    private int moveAmount = 50;

    private void OnEnable()
    {
        buyMoreDecks.onClick.AddListener(BuyAnotherDeck);
        showNextDeck.onClick.AddListener(MoveLayout);
        closeButton.onClick.AddListener(ClosePanel);

        CollectionDeckDisplay.OnShowDeck += ShowDeck;
        SubscribeForQommonDetails();
    }

    public void SubscribeForQommonDetails()
    {
        CollectionQommonDisplay.OnClicked += ShowDetails;
        ClearShownDecks();
        ShowDecks();
    }

    private void OnDisable()
    {
        buyMoreDecks.onClick.RemoveListener(BuyAnotherDeck);
        showNextDeck.onClick.RemoveListener(MoveLayout);
        closeButton.onClick.RemoveListener(ClosePanel);
        
        CollectionDeckDisplay.OnShowDeck -= ShowDeck;
        CollectionQommonDisplay.OnClicked -= ShowDetails;
    }

    private void ClosePanel()
    {
        Close();
        BotHudHandler.Instance.ShowMain();
    }

    private void BuyAnotherDeck()
    {
        if (DataManager.Instance.PlayerData.Decks.Count>= DataManager.Instance.GameData.AmountOfDecksPerPlayer)
        {
            UIManager.Instance.OkDialog.Setup("You already own max amount of lineups");
            return;
        }

        DataManager.Instance.PlayerData.AddNewDeck();
        Show();
    }

    private void MoveLayout()
    {
        var _deckHolderTransform = deckHolder.transform;
        Vector3 _itemsPosition = _deckHolderTransform.position;
        _itemsPosition.x -= moveAmount;
        _deckHolderTransform.position = _itemsPosition;
    }

    public override void Show()
    {
        ClearShownDecks();
        ClearOwnedQommons();
        ShowDecks();
        ShowQommons();
        gameObject.SetActive(true);
    }

    private void ClearShownDecks()
    {
        foreach (var _shownDeck in shownDecks)
        {
            Destroy(_shownDeck);
        }
        
        shownDecks?.Clear();
    }

    private void ClearOwnedQommons()
    {
        foreach (var _shownQommon in shownQommons)
        {
            Destroy(_shownQommon);
        }
        
        shownQommons?.Clear();
    }

    private void ShowDecks()
    {
        foreach (var _ownedDeck in DataManager.Instance.PlayerData.Decks)
        {
            CollectionDeckDisplay _deck = Instantiate(deckPrefab, deckHolder);
            _deck.Setup(_ownedDeck);
            shownDecks.Add(_deck.gameObject);
        }
    }

    private void ShowQommons()
    {
        foreach (var _qommon in Helpers.OrderQommons(DataManager.Instance.PlayerData.OwnedQommons))
        {
            CollectionQommonDisplay _qommonDisplay = Instantiate(qommonPrefab, qommonsHolder);
            _qommonDisplay.Setup(_qommon.Details.Id);
            _qommonDisplay.name = _qommon.name;
            shownQommons.Add(_qommonDisplay.gameObject);
        }   
    }

    private void ShowDeck(int _deckId)
    {
        deckBuilderPanel.Show(_deckId);
        CollectionQommonDisplay.OnClicked -= ShowDetails;
    }

    private void ShowDetails(int _qommonId)
    {
        qommonDisplay.Setup(_qommonId);
    }

    public override void Close()
    {
        OnClosed?.Invoke();
        gameObject.SetActive(false);
    }
}
