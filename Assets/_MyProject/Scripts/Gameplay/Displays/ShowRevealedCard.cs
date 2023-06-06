using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShowRevealedCard : MonoBehaviour
{
    [SerializeField] Image qommonDisplay;
    [SerializeField] GameObject detailsHolder;

    Vector2 startingRect;

    Sequence sequence;

    private void Awake()
    {
        RectTransform _qommonRect = qommonDisplay.GetComponent<RectTransform>();
        startingRect = new Vector2(_qommonRect.rect.width, _qommonRect.rect.height);
    }

    private void OnEnable()
    {
        CardReveal.ShowRevealCard += ShowCardDetails;
    }

    private void OnDisable()
    {
        CardReveal.ShowRevealCard -= ShowCardDetails;
    }

    void ShowCardDetails(CardObject _cardObject)
    {
        Vector3 _rotation = new Vector3(0, 0, 0);
        _rotation.y = _cardObject.IsMy ? 180 : 0;
        qommonDisplay.transform.eulerAngles = _rotation;
        RectTransform _rectTransform = qommonDisplay.GetComponent<RectTransform>();
        RectTransform _cardRect = _cardObject.GetComponent<RectTransform>();
        float _animationDuration = 0.5f;
        CardDetails _cardDetails = _cardObject.Details;

        qommonDisplay.sprite = _cardDetails.Sprite;

        _rectTransform.sizeDelta = new Vector2(_cardRect.rect.width, _cardRect.rect.height);
        _rectTransform.position = _cardRect.position;

        sequence = DOTween.Sequence();
        sequence.Append(_rectTransform.DOSizeDelta(startingRect, _animationDuration));
        sequence.Join(_rectTransform.DOLocalMove(Vector3.zero, _animationDuration));
        sequence.Append(_rectTransform.DOSizeDelta(_rectTransform.sizeDelta, _animationDuration));
        sequence.Join(_rectTransform.DOLocalMove(_rectTransform.anchoredPosition, _animationDuration));
        sequence.OnComplete(() =>
        {
            Close();
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
