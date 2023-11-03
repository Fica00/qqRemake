using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int AmountOfDecksPerPlayer;
    public List<GamePassOffer> GamePassOffers;
    public Dictionary<string,GamePassOffer> Marketplace;

    public void RemoveOfferFromMarketplace(GamePassOffer _offer)
    {
        foreach (var _offerInMarketplace in Marketplace)
        {
            if (_offerInMarketplace.Value.Equals(_offer))
            {
                Marketplace.Remove(_offerInMarketplace.Key);
                return;
            }
        }
    }
}
