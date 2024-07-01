using System;
using UnityEngine;
using UnityEngine.UI;

public class WithdrawalPanel : MonoBehaviour
{
    private const double MAX_AMOUNT = 100;
    private const double MIN_AMOUNT = 1f;
    private const double MAX_AMOUNT_OF_DECIMALS = 6;
    
    [SerializeField] private InputField address;
    [SerializeField] private InputField amount;
    [SerializeField] private Button max;
    [SerializeField] private Button paste;
    [SerializeField] private Button confirm;
    [SerializeField] private OkDialog okDialog;
    [SerializeField] private WalletPanel walletPanel;

    private void OnEnable()
    {
        max.onClick.AddListener(SetAmountToMax);
        confirm.onClick.AddListener(Confirm);
        paste.onClick.AddListener(PasteText);
        PlayerData.UpdatedUserWalletAddress += Setup;

        Setup();
    }

    private void Setup()
    {
        address.text = string.Empty;
        amount.text = string.Empty;
    }
    
    private void OnDestroy()
    {
        max.onClick.RemoveListener(SetAmountToMax);
        confirm.onClick.RemoveListener(Confirm);
        paste.onClick.RemoveListener(PasteText);
        PlayerData.UpdatedUserWalletAddress -= Setup;
    }

    private void SetAmountToMax()
    {
        double _amount = DataManager.Instance.PlayerData.USDT;
        if (_amount>MAX_AMOUNT)
        {
            _amount = MAX_AMOUNT;
        }

        amount.text = _amount.ToString();
    }

    private void PasteText()
    {
        JavaScriptManager.OnGotTextFromKeyboard += DoPaste;
        JavaScriptManager.Instance.RequestTextFromKeyboard();
        
        void DoPaste(string _text)
        {
            JavaScriptManager.OnGotTextFromKeyboard -= DoPaste;
            if (string.IsNullOrEmpty(_text))
            {
                return;
            }
            address.text = _text;
        }
    }


    private void Confirm()
    {
        string _amountText = amount.text;
        if (string.IsNullOrEmpty(_amountText))
        {
            okDialog.Setup("Please enter amount");
            return;
        }

        if (_amountText.Contains("."))
        {
            int _amountOfDecimals = _amountText.Split(".")[1].Length;
            if (_amountOfDecimals>MAX_AMOUNT_OF_DECIMALS)
            {
                okDialog.Setup("Maximum amount of decimals is "+ MAX_AMOUNT_OF_DECIMALS);
                return;
            }
        }

        double _amount;
        try
        {
            _amount = Convert.ToDouble(_amountText);
        }
        catch
        {
            okDialog.Setup("Amount field should only contain numbers and decimal places");
            return;
        }

        if (_amount<MIN_AMOUNT)
        {
            okDialog.Setup("Minimum amount is "+ MIN_AMOUNT);
            return;
        }

        if (_amount>MAX_AMOUNT)
        {
            okDialog.Setup("Max amount is "+MAX_AMOUNT);
            return;
        }

        if (_amount>DataManager.Instance.PlayerData.USDT)
        {
            okDialog.Setup("You don't have enough founds");
            return;
        }

        WithdrawalData _withdrawalData = new WithdrawalData
        {
            WithdrawalId = Guid.NewGuid().ToString(),
            WalletAddress = address.text,
            Amount = _amount,
            Status = WithdrawalStatus.Created,
            UserId = FirebaseManager.Instance.PlayerId,
            Error = string.Empty,
            RequestTime = DateTime.UtcNow,
        };

        ManageInteractables(false);
        Debug.Log(11111);
        
        FirebaseManager.Instance.RequestWithdrawal(_withdrawalData, _ =>
        {
            DataManager.Instance.PlayerData.USDT -= _amount;
            ManageInteractables(true);
            okDialog.Setup("Withdrawal request successfully sent, withdrawal id: "+ _withdrawalData.WithdrawalId);
        }, _error =>
        {
            okDialog.Setup("Failed to send request:\n"+_error);
            ManageInteractables(true);
        });
    }


    private void ManageInteractables(bool _status)
    {
        amount.interactable = _status;
        max.interactable = _status;
        confirm.interactable = _status;
        paste.interactable = _status;
        walletPanel.ManageInteractables(_status);
    }
    
}
