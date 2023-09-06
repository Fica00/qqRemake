using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuildQommonDetails : MonoBehaviour
{
    public static Action<int> OnAddCardToDeck;
    [SerializeField] private Image qommonDisplay;
    [SerializeField] private TextMeshProUGUI powerDisplay;
    [SerializeField] private TextMeshProUGUI manaDisplay;
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private TextMeshProUGUI descDisplay;
    [SerializeField] private Button addToDeckButton;
    [SerializeField] private RectTransform collectionHolder;
    private RectTransform rect;


    private int cardId;
    
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        addToDeckButton.onClick.AddListener(AddCardToDeck);
        
        var _sizeDelta = collectionHolder.sizeDelta;
        _sizeDelta = new Vector2(_sizeDelta.x, _sizeDelta.y - rect.sizeDelta.y);
        collectionHolder.sizeDelta = _sizeDelta;
    }

    private void OnDisable()
    {
        addToDeckButton.onClick.RemoveListener(AddCardToDeck);
        var _sizeDelta = collectionHolder.sizeDelta;
        _sizeDelta = new Vector2(_sizeDelta.x, _sizeDelta.y + rect.sizeDelta.y);
        collectionHolder.sizeDelta = _sizeDelta;
    }

    private void AddCardToDeck()
    {
        OnAddCardToDeck?.Invoke(cardId);
    }

    public void Setup(int _cardId)
    {
        cardId = _cardId;
        CardObject _card = CardsManager.Instance.GetCardObject(_cardId);
        qommonDisplay.sprite = _card.Details.Sprite;
        powerDisplay.text = _card.Details.Power.ToString();
        manaDisplay.text = _card.Details.Mana.ToString();
        nameDisplay.text = _card.Details.Name;
        descDisplay.text = _card.Details.Description.Replace("\\n", "\n");
        addToDeckButton.interactable = !DataManager.Instance.PlayerData.CardIdsInDeck.Contains(_cardId);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
