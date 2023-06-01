using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardOnTableDisplay : MonoBehaviour
{
    CardObject cardObject;

    [SerializeField] TMP_FontAsset valueIncreasedFont;
    [SerializeField] TMP_FontAsset valueDecreasedFont;
    [SerializeField] TMP_FontAsset normalFont;

    [Space()]
    [SerializeField] TextMeshProUGUI powerDisplay;
    [SerializeField] Image qommonDisplay;

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

    void ShowPower(ChangeStatus _status)
    {
        powerDisplay.text = cardObject.Stats.Power.ToString();
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
