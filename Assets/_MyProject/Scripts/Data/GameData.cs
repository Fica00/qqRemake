using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int AmountOfDecksPerPlayer;
    public List<GamePassOffer> GamePassOffers = new ();
    public Dictionary<string,GamePassOffer> Marketplace = new ();

    public void RemoveOfferFromMarketplace(GamePassOffer _offer)
    {
        var _offerInMarketplace = GetMarketplaceOffer(_offer,Marketplace);
        if (_offerInMarketplace.Key == default || _offerInMarketplace.Value == default)
        {
            return;
        }
        
        Marketplace.Remove(_offerInMarketplace.Key);
    }

    public static KeyValuePair<string, GamePassOffer> GetMarketplaceOffer(GamePassOffer _offer, Dictionary<string,
    GamePassOffer> _offers)
    {
        foreach (var _offerInMarketplace in _offers)
        {
            if (_offerInMarketplace.Value.Equals(_offer))
            {
                return _offerInMarketplace;
            }
        }

        return new KeyValuePair<string, GamePassOffer>();
    }
}
