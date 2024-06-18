using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QoomonUnlockingPanel : MonoBehaviour
{
    [SerializeField] private GameObject holder;
    [SerializeField] private GameObject qommonHolder;
    [SerializeField] private Image qommonDisplay;
    [SerializeField] private TextMeshProUGUI qommonPower;
    [SerializeField] private TextMeshProUGUI qommonMana;
    [SerializeField] private TextMeshProUGUI qommonName;
    [SerializeField] private TextMeshProUGUI qommonDesc;
    [SerializeField] private Button claim;
    [SerializeField] private GameObject backHolder;
    [SerializeField] private Button backButton;
    private Action callBack;
    
    private void OnEnable()
    {
        claim.onClick.AddListener(ShowBack);
        backButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        claim.onClick.RemoveListener(ShowBack);
        backButton.onClick.RemoveListener(Close);
    }

    private void ShowBack()
    {
        claim.gameObject.SetActive(false);
        backHolder.SetActive(true);
    }

    private void Close()
    {
        DOTween.KillAll();
        claim.gameObject.SetActive(true);
        backHolder.SetActive(false);
        holder.SetActive(false);
        callBack?.Invoke();
    }

    public void Setup(int _qommonId, Action _callBack)
    {
        callBack = _callBack;
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
        
        qommonHolder.transform?.DOScale(Vector3.one, 2);
    }
}
