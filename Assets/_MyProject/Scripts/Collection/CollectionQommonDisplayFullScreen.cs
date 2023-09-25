using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionQommonDisplayFullScreen : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Image qommonDisplay;
    [SerializeField] private TextMeshProUGUI powerDisplay;
    [SerializeField] private TextMeshProUGUI manaDisplay;
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private TextMeshProUGUI descDisplay;

    public void Setup(int _cardId)
    {
        CardObject _qommon = CardsManager.Instance.GetCardObject(_cardId);
        qommonDisplay.sprite = _qommon.Details.Sprite;
        powerDisplay.text = _qommon.Details.Power.ToString();
        manaDisplay.text = _qommon.Details.Mana.ToString();
        nameDisplay.text = _qommon.Details.Name;
        descDisplay.text = _qommon.Details.Description.Replace("\\n", "\n");
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        closeButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}
