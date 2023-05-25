using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CardDetailsPanel : MonoBehaviour
{
    [SerializeField] Button closeButton;
    [SerializeField] GameObject detailsHolder;
    [SerializeField] Image qommonDisplay;
    [SerializeField] GameObject manaHolder;
    [SerializeField] GameObject powerHolder;
    [SerializeField] TextMeshProUGUI powerDisplay;
    [SerializeField] TextMeshProUGUI manaDisplay;
    [SerializeField] TextMeshProUGUI nameDispaly;
    [SerializeField] TextMeshProUGUI descDisplay;
    Vector2 startingRect;

    Sequence sequence;

    private void Awake()
    {
        RectTransform _qommonRect = qommonDisplay.GetComponent<RectTransform>();
        startingRect = new Vector2(_qommonRect.rect.width, _qommonRect.rect.height);
    }

    private void OnEnable()
    {
        CardInputInteractions.OnClicked += ShowCardDetails;
        closeButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        CardInputInteractions.OnClicked -= ShowCardDetails;
        closeButton.onClick.RemoveListener(Close);
    }

    void ShowCardDetails(CardObject _cardObject)
    {
        RectTransform _rectTransform = qommonDisplay.GetComponent<RectTransform>();
        RectTransform _cardRect = _cardObject.GetComponent<RectTransform>();
        float _animationDuration = 1f;
        CardDetails _cardDetails = _cardObject.Details;

        qommonDisplay.sprite = _cardDetails.Sprite;
        manaHolder.SetActive(false);
        powerHolder.SetActive(false);
        nameDispaly.text = string.Empty;
        descDisplay.text = string.Empty;

        _rectTransform.sizeDelta = new Vector2(_cardRect.rect.width, _cardRect.rect.height);
        _rectTransform.position = _cardRect.position;

        sequence = DOTween.Sequence();
        sequence.Append(_rectTransform.DOSizeDelta(startingRect, _animationDuration));
        sequence.Join(_rectTransform.DOLocalMove(Vector3.zero, _animationDuration));
        sequence.OnComplete(() =>
        {
            manaHolder.SetActive(true);
            powerHolder.SetActive(true);
            nameDispaly.text = _cardDetails.Name;
            descDisplay.text = _cardDetails.Description;
            manaDisplay.text = _cardDetails.Mana.ToString();
            powerDisplay.text = _cardDetails.Power.ToString();
        });

        detailsHolder.SetActive(true);
        sequence.Play();
    }

    void Close()
    {
        if (sequence.IsActive() && sequence.IsPlaying())
        {
            sequence.Kill();
        }

        detailsHolder.SetActive(false);
    }
}
