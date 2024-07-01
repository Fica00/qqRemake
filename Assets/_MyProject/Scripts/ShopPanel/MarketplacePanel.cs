using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketplacePanel : BasePanel
{
    [SerializeField] private GamePassOfferMarketplace offerPrefab;
    [SerializeField] private Transform offerHolder;
    [SerializeField] private Button reloadButton;
    private List<GamePassOfferMarketplace> shownOffers = new ();
    private GamePassOffer selectedOffer;
    private bool canBuy;

    private float reloadCooldown;

    private void OnEnable()
    {
        reloadButton.onClick.AddListener(Reload);
    }

    private void OnDisable()
    {
        reloadButton.onClick.RemoveListener(Reload);
    }

    private void Reload()
    {
        if (reloadCooldown>0)
        {
            DialogsManager.Instance.OkDialog.Setup($"Refresh will be available in {(int)reloadCooldown}s");
            return;
        }

        reloadCooldown = 10;
        ClearShownOffers();
        FirebaseManager.Instance.RefreshMarketplace(OnRefreshedMarketplace);
    }

    private void OnRefreshedMarketplace()
    {
        Show(canBuy);
    }

    public void Show(bool _canBuyOffers)
    {
        canBuy = _canBuyOffers;
        ClearShownOffers();

        if (DataManager.Instance.GameData.Marketplace!=null)
        {
            foreach (var _offer in DataManager.Instance.GameData.Marketplace.Values)
            {
                GamePassOfferMarketplace _offerDisplay = Instantiate(offerPrefab, offerHolder);
                _offerDisplay.Setup(_offer,_canBuyOffers);
                _offerDisplay.OnClicked += TryToBuy;
                shownOffers.Add(_offerDisplay);
            }
        }
        
        gameObject.SetActive(true);
    }

    private void ClearShownOffers()
    {
        foreach (var _shownOffer in shownOffers)
        {
            _shownOffer.OnClicked -= TryToBuy;
            Destroy(_shownOffer.gameObject);
        }
        
        shownOffers.Clear();
    }

    private void TryToBuy(GamePassOffer _offer)
    {
        if (_offer.Owner == FirebaseManager.Instance.PlayerId)
        {
            DialogsManager.Instance.OkDialog.Setup("You can't buy your own game pass!");
            return;
        }

        if (DataManager.Instance.PlayerData.USDT<_offer.Cost)
        {
            DialogsManager.Instance.OkDialog.Setup("You don't have enough USDC");
            return;
        }
        
        selectedOffer = _offer;
        FirebaseManager.Instance.RemoveGamePassFromMarketplace(selectedOffer,HandleRemovedProduct);
    }

    private void HandleRemovedProduct(bool _result)
    {
        if (_result)
        {
            CryptoManager.Instance.Purchase(selectedOffer.Cost,selectedOffer.Owner, HandlePurchaseResult);
        }
        else
        {
            DialogsManager.Instance.OkDialog.Setup("Something went wrong, please try again later");
        }
    }

    private void HandlePurchaseResult(PurchaseResponse _result)
    {
        if (_result.Result == PurchaseResult.Successful)
        {
            _result.Message = "Purchase Successful!";
            GamePass _newGamePass = new GamePass(selectedOffer.GamePass);
            DataManager.Instance.PlayerData.AddGamePass(_newGamePass);
            DataManager.Instance.PlayerData.USDT -= selectedOffer.Cost;
            DataManager.Instance.GameData.RemoveOfferFromMarketplace(selectedOffer);
            Show(true);
        }
        
        PurchaseResultDisplay.Instance.Setup(_result);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (reloadCooldown>0)
        {
            reloadCooldown -= Time.deltaTime;
        }
    }
}
