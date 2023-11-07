using System.Collections.Generic;
using UnityEngine;

public class GamePassStorageDisplay : BasePanel
{
    [SerializeField] private Transform offersHolder;
    [SerializeField] private GamePassDisplay offerPrefab;
    [SerializeField] private SellPassPanel sellPanel;

    private List<GamePassDisplay> shownOffers = new(); 
    
    public override void Show()
    {
        ClearShownOffers();
        
        foreach (var _offer in DataManager.Instance.PlayerData.GamePasses)
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

    private void ShowSelected(GamePassDisplay _offerDisplay)
    {
        ShowSelected(_offerDisplay.GamePass);
    }
    
    public void ShowSelected(GamePass _gamePass)
    {
        foreach (var _shownOffer in shownOffers)
        {
            if (_shownOffer.GamePass == _gamePass)
            {
                _shownOffer.ShowAsSelected();
            }
            else
            {
                _shownOffer.ShowAsDeselected();
            }
        }

        sellPanel.ShowPass(DataManager.Instance.PlayerData.GamePasses.IndexOf(_gamePass));
    }
}
