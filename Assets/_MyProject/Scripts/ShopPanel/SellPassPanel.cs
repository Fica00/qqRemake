using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellPassPanel : BasePanel
{
    [SerializeField] private Button goToMarketplace;
    [SerializeField] private Image spriteDisplay;
    [SerializeField] private TextMeshProUGUI storageDisplay;
    [SerializeField] private TMP_InputField coinsInput;
    [SerializeField] private TMP_InputField costInput;
    [SerializeField] private TextMeshProUGUI coinsDisplay;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Button listButton;
    [SerializeField] private GamePassStorageDisplay passesDisplay;
    private GamePass showingPass;
    
    private void OnEnable()
    {
        goToMarketplace.onClick.AddListener(ShowMarketplace);
        coinsInput.onValueChanged.AddListener(ShowCoins);
        listButton.onClick.AddListener(ListPass);
    }

    private void OnDisable()
    {
        goToMarketplace.onClick.RemoveListener(ShowMarketplace);
        coinsInput.onValueChanged.RemoveListener(ShowCoins);
        listButton.onClick.RemoveListener(ListPass);
    }

    private void ShowCoins(string _text)
    {
        string _textToShow = "/ " + showingPass.StorageSize;
        if (string.IsNullOrEmpty(_text))
        {
            _textToShow = "? " + _textToShow;
        }
        else
        {
            double _number = double.Parse(_text);
            double _minNumber = Math.Min(showingPass.StorageSize, DataManager.Instance.PlayerData.Coins);
            if (_number>_minNumber)
            {
                _number = _minNumber;
            }

            _textToShow = _number + _textToShow;
        }

        coinsDisplay.text = _textToShow;
    }

    private void ShowMarketplace()
    {
        ShopPanel.Instance.ShowMarketplace(false);
    }
    
    public override void Show()
    {
        ShowPass(-1);
        passesDisplay.Show();
        gameObject.SetActive(true);
    }

    public void ShowPass(int _index)
    {
        if (_index==-1)
        {
            showingPass = default;
            spriteDisplay.sprite = defaultSprite;
            storageDisplay.text = "?";
            coinsDisplay.text = "? / ?";
            costInput.text = string.Empty;
            coinsInput.enabled = false;
        }
        else
        { 
            showingPass = DataManager.Instance.PlayerData.GamePasses[_index];
            spriteDisplay.sprite = showingPass.Sprite;
            storageDisplay.text = showingPass.StorageSize.ToString();
            coinsInput.text = string.Empty;
            coinsDisplay.text = "? / " + showingPass.StorageSize;
            costInput.text = string.Empty;
            coinsInput.enabled = true;
            
        }
        
        spriteDisplay.SetNativeSize();

    }

    private void ListPass()
    {
        if (showingPass == default)
        {
            return;
        }

        if (string.IsNullOrEmpty(coinsInput.text))
        {
            UIManager.Instance.OkDialog.Setup("Please select amount of gems");
            return;
        }

        if (string.IsNullOrEmpty(costInput.text))
        {
            UIManager.Instance.OkDialog.Setup("Please select the cost amount");
            return;
        }

        UIManager.Instance.YesNoDialog.OnYesPressed.AddListener(YesList);
        UIManager.Instance.YesNoDialog.Setup("Proceed with listing this game pass?");
    }

    private void YesList()
    {
        double _coins = double.Parse(coinsInput.text);
        double _cost = double.Parse(costInput.text);

        GamePass _pass = new GamePass(showingPass);
        _pass.Coins = _coins;
        GamePassOffer _offer = new GamePassOffer()
        {
            Cost = _cost, GamePass = _pass, Owner = FirebaseManager.Instance.PlayerId
        };

        FirebaseManager.Instance.AddOfferToMarketplace(_offer, HandleAddingResult);
    }
    
    private void HandleAddingResult(bool _result)
    {
        if (_result)
        {
            double _coins = double.Parse(coinsInput.text);
            DataManager.Instance.PlayerData.Coins -= _coins;
            DataManager.Instance.PlayerData.RemoveGamePass(showingPass);
            showingPass = default;
            ShowPass(-1);
            passesDisplay.Show();
        }
        else
        {
            UIManager.Instance.OkDialog.Setup("Something went wrong, please try again later");
        }
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
