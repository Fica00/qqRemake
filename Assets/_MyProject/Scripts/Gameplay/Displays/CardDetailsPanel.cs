using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;

public class CardDetailsPanel : MonoBehaviour
{
    public static Action OnClose;
    
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject detailsHolder;
    [SerializeField] private Image qommonDisplay;
    [SerializeField] private GameObject manaHolder;
    [SerializeField] private GameObject powerHolder;
    [SerializeField] private TextMeshProUGUI powerDisplay;
    [SerializeField] private TextMeshProUGUI manaDisplay;
    [SerializeField] private TextMeshProUGUI nameDispaly;
    [SerializeField] private TextMeshProUGUI descDisplay;
    [SerializeField] private GameObject showHandPanel;
    [SerializeField] private GameObject hideHandPanel;

    private Vector2 startingRect;
    private Vector3 originalPosition; // To store the original position of the qommon
    private Vector2 originalSize; // To store the original size of the qommon

    private Sequence sequence;

    private void Awake()
    {
        RectTransform _qommonRect = qommonDisplay.GetComponent<RectTransform>();
        startingRect = new Vector2(_qommonRect.rect.width, _qommonRect.rect.height);
    }

    private void OnEnable()
    {
        CardInteractions.OnClicked += ShowCardDetails;
        closeButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        CardInteractions.OnClicked -= ShowCardDetails;
        closeButton.onClick.RemoveListener(Close);
    }

    private void ShowCardDetails(CardObject _cardObject)
    {
        if (!_cardObject.IsMy)
        {
            if (!_cardObject.Reveal.IsReveled)
            {
                return;
            }
        }
        AudioManager.Instance.PlaySoundEffect(AudioManager.CARD_SOUND);
        showHandPanel.SetActive(false);
        hideHandPanel.SetActive(false);
        
        RectTransform _cardRectNew = _cardObject.GetComponent<RectTransform>();
        originalPosition = _cardRectNew.position;
        originalSize = new Vector2(_cardRectNew.rect.width, _cardRectNew.rect.height);

        if (_cardObject.CardLocation == CardLocation.Table)
        {
            hideHandPanel.SetActive(true);
        }
        else
        {
            showHandPanel.SetActive(true);
        }

        Vector3 _rotation = new Vector3(0, 0, 0);
        _rotation.y = _cardObject.IsMy ? 180 : 0;
        qommonDisplay.transform.eulerAngles = _rotation;
        RectTransform _rectTransform = qommonDisplay.GetComponent<RectTransform>();
        RectTransform _cardRect = _cardObject.GetComponent<RectTransform>();
        float _animationDuration = 0.4f;
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
            string _desc = _cardDetails.Description;
            _desc = _desc.Replace("\\n", "\n");
            descDisplay.text = _desc;
            manaDisplay.text = _cardDetails.Mana.ToString();
            powerDisplay.text = _cardDetails.Power.ToString();
        });

        detailsHolder.SetActive(true);
        sequence.Play();
    }

    private void Close()
    {
        if (sequence.IsActive() && sequence.IsPlaying())
        {
            sequence.Kill();
        }

        OnClose?.Invoke();
        manaHolder.SetActive(false);
        powerHolder.SetActive(false);
        nameDispaly.text = string.Empty;
        descDisplay.text = string.Empty;
        manaDisplay.text = string.Empty;
        powerDisplay.text = string.Empty;
        
        RectTransform _rectTransform = qommonDisplay.GetComponent<RectTransform>();
        Sequence _closeSequence = DOTween.Sequence();

        _closeSequence.Append(_rectTransform.DOSizeDelta(originalSize, 0.4f));
        _closeSequence.Join(_rectTransform.DOMove(originalPosition, 0.4f));

        _closeSequence.OnComplete(() => {
            detailsHolder.SetActive(false);
        });

        _closeSequence.Play();
    }
}
