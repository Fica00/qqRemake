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
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        cardObject.Stats.UpdatedPower -= ShowPower;
    }

    private void ShowPower(ChangeStatus _status)
    {
        powerDisplay.text = (cardObject.Stats.Power+cardObject.Stats.ChagePowerDueToLocation).ToString();
        switch (_status)
        {
            case ChangeStatus.Same:
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
}
