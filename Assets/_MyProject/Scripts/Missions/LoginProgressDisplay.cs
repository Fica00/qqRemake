using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginProgressDisplay : MonoBehaviour
{
    public static Action<int> OnClicked;
    [SerializeField] private Button claimButton;
    [SerializeField] private GameObject lockedDisplay;
    [SerializeField] private TextMeshProUGUI numberDisplay;
    [SerializeField] private GameObject completed;
    [SerializeField] private GameObject redDot;
    private int number;

    public void Setup(bool _isUnlocked, int _number)
    {
        redDot.SetActive(false);
        number = _number;
        lockedDisplay.SetActive(!_isUnlocked);
        numberDisplay.text = _number.ToString();
        if (DataManager.Instance.PlayerData.ClaimedLoginRewards.Contains(_number))
        {
            completed.SetActive(true);
        }
        else if (_isUnlocked)
        {
            redDot.SetActive(true);
        }
    }

    private void OnEnable()
    {
        claimButton.onClick.AddListener(Click);
    }

    private void OnDisable()
    {
        claimButton.onClick.RemoveListener(Click);
    }

    private void Click()
    {
        OnClicked?.Invoke(number);
    }
}
