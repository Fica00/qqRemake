using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class CardInHandDisplay : MonoBehaviour
{
    CardObject cardObject;

    [SerializeField] TMP_FontAsset valueIncreasedFont;
    [SerializeField] TMP_FontAsset valueDecreasedFont;
    [SerializeField] TMP_FontAsset normalFont;

    [Space()]
    [SerializeField] TextMeshProUGUI manaDisplay;
    [SerializeField] TextMeshProUGUI powerDisplay;
    [SerializeField] Image qommonDisplay;
    [SerializeField] GameObject manaHolder;
    [SerializeField] GameObject powerHolder;

    public void Setup(CardObject _cardObject)
    {
        cardObject = _cardObject;
        qommonDisplay.sprite = cardObject.Details.Sprite;
        manaDisplay.font = normalFont;
        powerDisplay.font = normalFont;

        cardObject.Stats.UpdatedMana += ShowMana;
        cardObject.Stats.UpdatedPower += ShowPower;
    }

    public void Show()
    {
        ShowMana(ChangeStatus.Same);
        ShowPower(ChangeStatus.Same);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        cardObject.Stats.UpdatedMana -= ShowMana;
        cardObject.Stats.UpdatedPower -= ShowPower;
    }

    void ShowMana(ChangeStatus _status)
    {
        manaDisplay.text = cardObject.Stats.Mana.ToString();
        switch (_status)
        {
            case ChangeStatus.Same:
                break;
            case ChangeStatus.Increased:
                manaDisplay.font = valueIncreasedFont;
                break;
            case ChangeStatus.Decreased:
                manaDisplay.font = valueDecreasedFont;
                break;
            default:
                throw new System.Exception("Don't know how to resolve state: " + _status);
        }
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

    public void ShowDrawnAnimation()
    {
        manaHolder.SetActive(false);
        powerHolder.SetActive(false);

        transform.position = new Vector3(
            Screen.width + 1000,
            transform.position.y,
            transform.position.z
            );

        transform.DOLocalMove(Vector3.zero, 1f).OnComplete(() =>
         {
             manaHolder.SetActive(true);
             powerHolder.SetActive(true);
         });
    }
}
