using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class CardInHandDisplay : MonoBehaviour
{
    private CardObject cardObject;

    [SerializeField] private TMP_FontAsset positiveChange;
    [SerializeField] private TMP_FontAsset negativeChange;
    [SerializeField] private TMP_FontAsset normalFont;

    [Space()]
    [SerializeField]
    private TextMeshProUGUI manaDisplay;
    [SerializeField] private TextMeshProUGUI powerDisplay;
    [SerializeField] private Image qommonDisplay;
    [SerializeField] private GameObject manaHolder;
    [SerializeField] private GameObject powerHolder;

    public Image QommonDisplay => qommonDisplay;

    public void Setup(CardObject _cardObject)
    {
        cardObject = _cardObject;
        if (cardObject.Details.SpriteInHand==null)
        {
            cardObject.Details.SpriteInHand = cardObject.Details.Sprite;
        }
        qommonDisplay.sprite = cardObject.Details.SpriteInHand;
        manaDisplay.font = normalFont;
        powerDisplay.font = normalFont;

        cardObject.Stats.UpdatedMana += ShowMana;
        cardObject.Stats.UpdatedPower += ShowPower;
    }

    private void OnEnable()
    {
        GameplayManager.Instance.MyPlayer.UpdatedEnergy += ShowIfPlayerHasEnaughtEnergy;
        cardObject.Stats.UpdatedMana += ShowIfPlayerHasEnaughtEnergy;
        GameplayManager.Instance.MyPlayer.FinishedTurn += ShowAllQommonsAsAvailable;
        GameplayManager.UpdatedGameState += CheckState;
    }

    private void OnDisable()
    {
        cardObject.Stats.UpdatedMana -= ShowMana;
        cardObject.Stats.UpdatedPower -= ShowPower;
        GameplayManager.Instance.MyPlayer.UpdatedEnergy -= ShowIfPlayerHasEnaughtEnergy;
        cardObject.Stats.UpdatedMana -= ShowIfPlayerHasEnaughtEnergy;
        GameplayManager.Instance.MyPlayer.FinishedTurn -= ShowAllQommonsAsAvailable;
        GameplayManager.UpdatedGameState -= CheckState;
    }

    private void CheckState()
    {
        if (GameplayManager.Instance.GameplayState == GameplayState.Playing)
        {
            ShowIfPlayerHasEnaughtEnergy();
        }
    }

    private void ShowIfPlayerHasEnaughtEnergy(ChangeStatus status)
    {
        ShowIfPlayerHasEnaughtEnergy();
    }

    public void Show()
    {
        ShowMana(ChangeStatus.Same);
        ShowPower(ChangeStatus.Same);
        ShowIfPlayerHasEnaughtEnergy();
        gameObject.SetActive(true);
    }

    private void ShowIfPlayerHasEnaughtEnergy()
    {
        Color _color = qommonDisplay.color;
        _color.a = GameplayManager.Instance.MyPlayer.Energy < cardObject.Stats.Energy ? 0.3f : 1f;

        qommonDisplay.color = _color;
    }

    private void ShowAllQommonsAsAvailable()
    {
        Color _color = qommonDisplay.color;
        _color.a = 1f;
        qommonDisplay.color = _color;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowMana(ChangeStatus _)
    {
        int _cardMana = cardObject.Stats.Energy;
        manaDisplay.text = _cardMana.ToString();

        ChangeStatus _status;
        if (_cardMana > cardObject.Details.Mana)
        {
            _status = ChangeStatus.Increased;
        }
        else if (_cardMana<cardObject.Details.Mana)
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
                manaDisplay.font = normalFont;
                break;
            case ChangeStatus.Increased:
                manaDisplay.font = negativeChange;
                break;
            case ChangeStatus.Decreased:
                manaDisplay.font = positiveChange;
                break;
            default:
                throw new System.Exception("Don't know how to resolve state: " + _status);
        }
    }

    private void ShowPower(ChangeStatus _status)
    {
        powerDisplay.text = cardObject.Stats.Power.ToString();
        switch (_status)
        {
            case ChangeStatus.Same:
                break;
            case ChangeStatus.Increased:
                powerDisplay.font = normalFont;
                break;
            case ChangeStatus.Decreased:
                powerDisplay.font = normalFont;
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
