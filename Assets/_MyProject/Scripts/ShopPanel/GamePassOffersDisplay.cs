using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamePassOffersDisplay : BasePanel
{
    [SerializeField] private Transform offersHolder;
    [SerializeField] private GamePassDisplay offerPrefab;
    [SerializeField] private BuyPassPanel buyPassPanel; 

    private List<GamePassDisplay> shownOffers = new(); 

    public override void Show()
    {
        ClearShownOffers();
        foreach (var _offer in DataManager.Instance.GameData.GamePassOffers.ToList())
        {
            GamePassDisplay _display = Instantiate(offerPrefab, offersHolder);
            _display.Setup(_offer);
            _display.OnSelected += ShowSelected;
            shownOffers.Add(_display);
        }
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        ClearShownOffers();
        gameObject.SetActive(false);
    }

    private void ClearShownOffers()
    {
        foreach (var _offer in shownOffers)
        {
            _offer.OnSelected -= ShowSelected;
            Destroy(_offer.gameObject);
        }
        
        shownOffers.Clear();
    }

    public void SetOffer(GamePassOffer _offer)
    {
        buyPassPanel.SetOffer(_offer);
        ShowSelected(_offer);
    }

    private void ShowSelected(GamePassDisplay _offerDisplay)
    {
        ShowSelected(_offerDisplay.Offer);
    }
    
    public void ShowSelected(GamePassOffer _offer)
    {
        foreach (var _shownOffer in shownOffers)
        {
            if (_shownOffer.Offer == _offer)
            {
                _shownOffer.ShowAsSelected();
            }
            else
            {
                _shownOffer.ShowAsDeselected();
            }
        }

        buyPassPanel.SetOffer(_offer);
    }
}
