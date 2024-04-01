using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlockingPanel : MonoBehaviour
{
    [SerializeField] private GameObject holder;
    [SerializeField] private GameObject qommonHolder;
    [SerializeField] private Image qommonDisplay;
    [SerializeField] private TextMeshProUGUI qommonPower;
    [SerializeField] private TextMeshProUGUI qommonMana;
    [SerializeField] private TextMeshProUGUI qommonName;
    [SerializeField] private TextMeshProUGUI qommonDesc;
    [SerializeField] private Button claim;
    
    private void OnEnable()
    {
        LevelRewardsPanel.OnUnlockedNewQoomon += Setup;
        claim.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        LevelRewardsPanel.OnUnlockedNewQoomon -= Setup;
        claim.onClick.RemoveListener(Close);
    }

    private void Close()
    {
        holder.SetActive(false);
    }

    private void Setup(int _qommonId)
    {
        holder.SetActive(true);
        CardObject _qommon = CardsManager.Instance.GetCardObject(_qommonId);
        qommonHolder.transform.localScale = Vector3.zero;
        qommonDisplay.sprite = _qommon.Details.Sprite;
        qommonPower.text = _qommon.Details.Power.ToString();
        qommonMana.text = _qommon.Details.Mana.ToString();
        qommonName.text = _qommon.Details.Name;
        string _desc = _qommon.Details.Description;
        _desc = _desc.Replace("\\n", "\n");
        qommonDesc.text = _desc;
        qommonHolder.transform.DOScale(Vector3.one, 2);
    }
}
