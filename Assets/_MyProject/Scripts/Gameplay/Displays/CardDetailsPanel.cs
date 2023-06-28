using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CardDetailsPanel : MonoBehaviour
{
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
        showHandPanel.SetActive(false);
        hideHandPanel.SetActive(false);

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

        detailsHolder.SetActive(false);
    }
}
