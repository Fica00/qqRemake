using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardOnTableDisplay : MonoBehaviour
{
    private CardObject cardObject;

    [SerializeField] private TMP_FontAsset valueIncreasedFont;
    [SerializeField] private TMP_FontAsset valueDecreasedFont;
    [SerializeField] private TMP_FontAsset normalFont;

    [Space()]
    [SerializeField]
    private TextMeshProUGUI powerDisplay;
    [SerializeField] private Image qommonDisplay;


    public Image QommonDisplay => qommonDisplay;
    
    public void Setup(CardObject _cardObject)
    {
        cardObject = _cardObject;
        qommonDisplay.sprite = cardObject.Details.Sprite;
        powerDisplay.font = normalFont;
        cardObject.Stats.UpdatedPower += ShowPower;
    }

    public void Show()
    {
        ShowPower(ChangeStatus.Same);
        gameObject.SetActive(true);
        float _newScale = cardObject.Stats.Energy switch
        {
            6 => 1.1f,
            1 => 0.9f,
            _ => 1
        };
        if (cardObject.Animator.HasAnimations && cardObject.Animator.IsRevealAnimationDone)
        {
            qommonDisplay.gameObject.SetActive(false);
        }

        transform.localScale = new Vector3(_newScale, _newScale, _newScale);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        cardObject.Stats.UpdatedPower -= ShowPower;
    }

    private void ShowPower(ChangeStatus _)
    {
        int _cardPower = cardObject.Stats.Power + cardObject.Stats.ChagePowerDueToLocation;
        powerDisplay.text = _cardPower.ToString();
        ChangeStatus _status;
        if (_cardPower > cardObject.Details.Power)
        {
            _status = ChangeStatus.Increased;
        }
        else if (_cardPower<cardObject.Details.Power)
        {
            _status = ChangeStatus.Decreased;
        }
        else
        {
            _status = ChangeStatus.Same;
        }
        switch (_status)
        {
            case ChangeStatus.Same:
                powerDisplay.font = normalFont;
                break;
            case ChangeStatus.Increased:
                powerDisplay.font = valueIncreasedFont;
                break;
            case ChangeStatus.Decreased:
                powerDisplay.font = valueDecreasedFont;
                break;
            default:
                throw new System.Exception("Don't know how to resolve state: " + _status);
        }
    }
    
    public void EnlargedPowerAnimation()
    {
        
        float _currentSize = powerDisplay.fontSize;
        float _startSize = _currentSize;
        DOTween.To(() => _currentSize, x => _currentSize = x, _currentSize + 25, 1f)
            .OnUpdate(() =>
            {
                powerDisplay.fontSize = _currentSize;
            })
            .OnComplete(() =>
            {
                cardObject.Stats.Power += 0;
                powerDisplay.fontSize = _startSize;
            });
    }
}
