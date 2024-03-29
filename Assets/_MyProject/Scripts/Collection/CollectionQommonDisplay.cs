using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CollectionQommonDisplay : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static Action<int> OnClicked;
    public static Action<int> OnHold;

    [SerializeField] private Image qommonDisplay;
    [SerializeField] private TextMeshProUGUI manaDisplay;
    [SerializeField] private TextMeshProUGUI powerDisplay;
    [SerializeField] private Image border;

    [SerializeField] private Sprite emptySprite;
    [SerializeField] private GameObject manaHolder;
    [SerializeField] private GameObject powerHolder;
    [SerializeField] private GameObject alreadyInDeck;
    
    private int cardId;
    private bool isButtonHeld;
    private float holdStartTime;
    private float holdDuration = 0.2f;

    public void Setup(int _cardId, bool _checkIfInDeck=false)
    {
        border.color = new Color(1, 1, 1, 1);
        cardId = _cardId;
        CardObject _card = CardsManager.Instance.GetCardObject(_cardId);
        qommonDisplay.sprite = _card.Details.SpriteInHand;
        manaDisplay.text = _card.Details.Mana.ToString();
        powerDisplay.text = _card.Details.Power.ToString();
        manaHolder.SetActive(true);
        powerHolder.SetActive(true);
        alreadyInDeck.SetActive(_checkIfInDeck && DataManager.Instance.PlayerData.CardIdsInDeck.Contains(_cardId));
    }

    public void SetupEmpty()
    {
        cardId = -1;
        border.color = new Color(1, 1, 1, 0);
        qommonDisplay.sprite = emptySprite;
        manaHolder.SetActive(false);
        powerHolder.SetActive(false);
        alreadyInDeck.SetActive(false);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonHeld = true;
        holdStartTime = Time.time;
    }

    public void OnPointerUp(PointerEventData _eventData)
    {
        isButtonHeld = false;
        if (Time.time - holdStartTime < holdDuration)
        {
            CardClicked();
        }
    }

    private void Update()
    {
        if (isButtonHeld && Time.time - holdStartTime >= holdDuration)
        {
            if (cardId==-1)
            {
                return;
            }
            OnHold?.Invoke(cardId);
        }
    }

    private void CardClicked()
    {
        if (cardId == -1)
        {
            return;
        }
        OnClicked?.Invoke(cardId);
    }
}