using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListPassPanel : BasePanel
{
    [SerializeField] private Button closeButton;
    [SerializeField] private InputField coinsInput;
    [SerializeField] private TextMeshProUGUI coinsInputLabel;
    [SerializeField] private InputField costInput;
    [SerializeField] private TextMeshProUGUI suggestionLabel;
    [SerializeField] private TextMeshProUGUI suggestionDisplay;
    [SerializeField] private SellPassPanel sellPassPanel;
    [SerializeField] private Button maxGemsButton;
    [SerializeField] private Button listButton;
    
    private GamePass gamePass;
    private double suggestionMultiplier = 10;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(Close);
        listButton.onClick.AddListener(ListPass);
        coinsInput.onValueChanged.AddListener(SuggestPrice);
        maxGemsButton.onClick.AddListener(SetMaxGems);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
        listButton.onClick.RemoveListener(ListPass);
        coinsInput.onValueChanged.RemoveListener(SuggestPrice);
        maxGemsButton.onClick.RemoveListener(SetMaxGems);
    }
    
    private void SuggestPrice(string _priceString)
    {
        if (string.IsNullOrEmpty(_priceString))
        {
            suggestionDisplay.text = "? USDC";
            return;
        }
        
        double _price = double.Parse(_priceString);
        _price *= suggestionMultiplier;
        suggestionDisplay.text =$"{_price} USDC";
    }

    private void SetMaxGems()
    {
        double _maxAmount = DataManager.Instance.PlayerData.Coins>=gamePass.StorageSize ? gamePass.StorageSize : DataManager.Instance.PlayerData.Coins;
        coinsInput.text = _maxAmount.ToString();
    }

    public void Show(GamePass _gamePass)
    {
        suggestionLabel.text = $"Suggested floor price {suggestionMultiplier}x gems";
        suggestionDisplay.text = "? USDC";
        coinsInputLabel.text = $"/ {_gamePass.StorageSize} (Capacity)";
        gamePass = _gamePass;
        gameObject.SetActive(true);
    }

    private void ListPass()
    {
        if (string.IsNullOrEmpty(coinsInput.text))
        {
            PurchaseResultDisplay.Instance.Setup(new PurchaseResponse { Message = "Please select amount of gems", Result = PurchaseResult.Failed });
            return;
        }

        if (string.IsNullOrEmpty(costInput.text))
        {
            PurchaseResultDisplay.Instance.Setup(new PurchaseResponse { Message = "Please select the price amount", Result = PurchaseResult.Failed });
            return;
        }

        DialogsManager.Instance.YesNoDialog.OnYesPressed.AddListener(YesList);
        DialogsManager.Instance.YesNoDialog.Setup("Proceed with listing this game pass?");
    }
    
    private void YesList()
    {
        double _coins = double.Parse(coinsInput.text);
        double _cost = double.Parse(costInput.text);

        GamePass _pass = new GamePass(gamePass);
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
            DataManager.Instance.PlayerData.RemoveGamePass(gamePass);
            sellPassPanel.ListSuccessful();
            Close();
        }
        else
        {
            PurchaseResultDisplay.Instance.Setup(new PurchaseResponse { Message = "Something went wrong, please try again later", Result = PurchaseResult.Failed });
        }
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
