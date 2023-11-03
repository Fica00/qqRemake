using System.Collections.Generic;
using UnityEngine;

public class MarketplacePanel : BasePanel
{
    [SerializeField] private GamePassOfferMarketplace offerPrefab;
    [SerializeField] private Transform offerHolder;
    private List<GamePassOfferMarketplace> shownOffers = new ();
    private GamePassOffer selectedOffer;
    
    public void Show(bool _canBuyOffers)
    {
        ClearShownOffers();

        foreach (var _offer in DataManager.Instance.GameData.Marketplace.Values)
        {
            GamePassOfferMarketplace _offerDisplay = Instantiate(offerPrefab, offerHolder);
            _offerDisplay.Setup(_offer,_canBuyOffers);
            _offerDisplay.OnClicked += TryToBuy;
            shownOffers.Add(_offerDisplay);
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
        if (DataManager.Instance.PlayerData.GamePasses.Count>0)
        {
            UIManager.Instance.OkDialog.Setup("You already own a game pass!");
            return;
        }

        if (_offer.Owner == FirebaseManager.Instance.PlayerId)
        {
            UIManager.Instance.OkDialog.Setup("You can't buy your own game pass!");
            return;
        }

        if (DataManager.Instance.PlayerData.USDC<_offer.Cost)
        {
            UIManager.Instance.OkDialog.Setup("You don't have enough USDC");
            return;
        }
        
        selectedOffer = _offer;
        UIManager.Instance.YesNoDialog.OnYesPressed.AddListener(YesBuy);
        UIManager.Instance.YesNoDialog.Setup("Continue with the purchase?");
    }
    
    private void YesBuy()
    {
        CryptoManager.Instance.Purchase(selectedOffer.Cost, HandlePurchaseResult);
    }
    
    private void HandlePurchaseResult(PurchaseResponse _result)
    {
        if (_result.Successfully)
        {
            DataManager.Instance.PlayerData.AddGamePass(selectedOffer.GamePass);
            DataManager.Instance.PlayerData.USDC -= selectedOffer.Cost;
            DataManager.Instance.GameData.RemoveOfferFromMarketplace(selectedOffer);
            FirebaseManager.Instance.RemoveGamePassFromMarketplace(selectedOffer);
            UIManager.Instance.OkDialog.Setup("Successfully purchased!");
            Show(true);
        }
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
