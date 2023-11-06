using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BuyPassPanel : BasePanel
{
    [SerializeField] private Button goToMarketplace;
    [SerializeField] private GameObject noOfferImage;
    [SerializeField] private Image offerDisplay;
    [SerializeField] private TextMeshProUGUI coinsDisplay;
    [SerializeField] private TextMeshProUGUI storageDisplay;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Dropdown gamePassSelection;
    [SerializeField] private GamePassOffersDisplay offersDisplay;
    private GamePassOffer selectedOffer;
    
    private void OnEnable()
    {
        goToMarketplace.onClick.AddListener(ShowMarketplace);
        gamePassSelection.onValueChanged.AddListener(ShowSelectedGamePass);
        buyButton.onClick.AddListener(BuyGamePass);
    }

    private void OnDisable()
    {
        goToMarketplace.onClick.RemoveListener(ShowMarketplace);
        gamePassSelection.onValueChanged.RemoveListener(ShowSelectedGamePass);
        buyButton.onClick.RemoveListener(BuyGamePass);
    }

    private void ShowMarketplace()
    {
        ShopPanel.Instance.ShowMarketplace(true);
    }

    public void SetOffer(GamePassOffer _offer)
    {
        selectedOffer = _offer;
        ShowOffer();
    }

    private void SetupDropDown()
    {
        List<string> _newOptions = new List<string> ();
        _newOptions.Add("Please select option");
        foreach (var _gamePassOffer in GetOffersInOrder())
        {
            _newOptions.Add(_gamePassOffer.GamePass.Name);
        }

        gamePassSelection.AddOptions(_newOptions);
    }
    
    private void ShowSelectedGamePass(int _indexOfOffer)
    {
        if (_indexOfOffer==0)
        {
            SetOffer(default);
            offersDisplay.ShowSelected(default);
            return;
        }

        GamePassOffer _offer = GetOffersInOrder()[_indexOfOffer-1];
        offersDisplay.SetOffer(_offer);
        ShowOffer();
    }

    public override void Show()
    {
        selectedOffer = default;
        ShowOffer();
        SetupDropDown();
        offersDisplay.SetOffer(default);
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
    
    private void ShowOffer()
    {
        if (selectedOffer==default)
        {
            noOfferImage.SetActive(true);
            offerDisplay.gameObject.SetActive(false);
            coinsDisplay.text = "x";
            storageDisplay.text = "x";
            buyButton.enabled = false;
            gamePassSelection.SetValueWithoutNotify(0);
        }
        else
        {
            noOfferImage.SetActive(false);
            offerDisplay.gameObject.SetActive(true);
            GamePassOffer _offer = selectedOffer;
            offerDisplay.sprite = _offer.GamePass.Sprite;
            coinsDisplay.text = _offer.GamePass.Coins.ToString();
            storageDisplay.text = _offer.GamePass.StorageSize.ToString();
            buyButton.enabled = true;
            gamePassSelection.captionText.text = _offer.Cost.ToString();
        }
    }

    private List<GamePassOffer> GetOffersInOrder()
    {
        return DataManager.Instance.GameData.GamePassOffers.ToList().OrderBy(_offer => _offer.Cost).ToList();
    }

    private void BuyGamePass()
    {
        if (selectedOffer==default)
        {
            UIManager.Instance.OkDialog.Setup("Please elect the game pass");
            return;
        }

        UIManager.Instance.YesNoDialog.OnYesPressed.AddListener(YesBuy);
        UIManager.Instance.YesNoDialog.Setup("Continue with the purchase?");
    }

    private void YesBuy()
    {
        StripeManager.Instance.Purchase(selectedOffer.Cost,HandlePurchaseResult);
    }

    private void HandlePurchaseResult(PurchaseResponse _result)
    {
        if (_result.Successfully)
        {
            GamePass _newGamePass = new GamePass(selectedOffer.GamePass);
            DataManager.Instance.PlayerData.AddGamePass(_newGamePass);
            gamePassSelection.value = 0;
            UIManager.Instance.OkDialog.Setup("Successfully purchased!");
        }
    }
}
