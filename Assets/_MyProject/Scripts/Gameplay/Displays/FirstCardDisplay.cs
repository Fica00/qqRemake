using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirstCardDisplay : MonoBehaviour
{
    [SerializeField] private GameObject holder;
    [SerializeField] private Button closeButton;
    [SerializeField] private Image qommonDisplay;
    [SerializeField] private TextMeshProUGUI powerDisplay;
    [SerializeField] private TextMeshProUGUI manaDisplay;
    [SerializeField] private TextMeshProUGUI nameDispaly;
    [SerializeField] private TextMeshProUGUI descDisplay;
    private List<int> qoomonsToShow = new();

    private void OnEnable()
    {
        closeButton.onClick.AddListener(Close);
        PlayerStatistics.OnSawNewQoomon += ShowNewQoomon;
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
        PlayerStatistics.OnSawNewQoomon += ShowNewQoomon;
    }

    private void ShowNewQoomon(int _qoomonId)
    {
        if (_qoomonId<=46)
        {
            return;
        }
        qoomonsToShow.Add(_qoomonId);
        if (holder.activeSelf)
        {
            return;
        }
        ShowCardDetails(_qoomonId);
    }

    private void ShowCardDetails(int _qoomonId)
    {
        holder.SetActive(true);
        qoomonsToShow.Remove(_qoomonId);
        CardObject _cardObject = CardsManager.Instance.GetCardObject(_qoomonId);
        AudioManager.Instance.PlaySoundEffect(AudioManager.CARD_SOUND);

        Vector3 _rotation = new Vector3(0, 0, 0) { y = _cardObject.IsMy ? 180 : 0 };
        qommonDisplay.transform.eulerAngles = _rotation;
        CardDetails _cardDetails = _cardObject.Details;

        qommonDisplay.sprite = _cardDetails.Sprite;

        nameDispaly.text = _cardDetails.Name;
        string _desc = _cardDetails.Description;
        _desc = _desc.Replace("\\n", "\n");
        descDisplay.text = _desc;
        manaDisplay.text = _cardDetails.Mana.ToString();
        powerDisplay.text = _cardDetails.Power.ToString();
    }

    private void Close()
    {
        holder.SetActive(false);
        if (qoomonsToShow.Count>0)
        {
            ShowCardDetails(qoomonsToShow[0]);
        }
    }
}
