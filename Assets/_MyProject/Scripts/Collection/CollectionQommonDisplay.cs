using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionQommonDisplay : MonoBehaviour
{
    public static Action<int> OnClicked;

    [SerializeField] private Button button;
    [SerializeField] private Image qommonDisplay;
    [SerializeField] private TextMeshProUGUI manaDisplay;
    [SerializeField] private TextMeshProUGUI powerDisplay;

    [SerializeField] private Sprite emptySprite;
    [SerializeField] private GameObject manaHolder;
    [SerializeField] private GameObject powerHolder;
    [SerializeField] private GameObject alreadyInDeck;
    
    private int cardId;

    public void Setup(int _cardId, bool _checkIfInDeck=false)
    {
        cardId = _cardId;
        CardObject _card = CardsManager.Instance.GetCardObject(_cardId);
        qommonDisplay.sprite = _card.Details.Sprite;
        manaDisplay.text = _card.Details.Mana.ToString();
        powerDisplay.text = _card.Details.Power.ToString();
        manaHolder.SetActive(true);
        powerHolder.SetActive(true);
        alreadyInDeck.SetActive(_checkIfInDeck && DataManager.Instance.PlayerData.CardIdsInDeck.Contains(_cardId));
    }

    public void SetupEmpty()
    {
        cardId = -1;
        qommonDisplay.sprite = emptySprite;
        manaHolder.SetActive(false);
        powerHolder.SetActive(false);
        alreadyInDeck.SetActive(false);
    }

    private void OnEnable()
    {
        button.onClick.AddListener(CardClicked);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(CardClicked);
    }

    private void CardClicked()
    {
        if (cardId==-1)
        {
            return;
        }
        OnClicked?.Invoke(cardId);
    }
}
