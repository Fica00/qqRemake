using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePassOfferMarketplace : MonoBehaviour
{
    public Action<GamePassOffer> OnClicked;
    [SerializeField] private Image imageDisplay;
    [SerializeField] private TextMeshProUGUI coinsDisplay;
    [SerializeField] private TextMeshProUGUI storageDisplay;
    [SerializeField] private TextMeshProUGUI costDisplay;
    [SerializeField] private Button buyButton;
    private GamePassOffer offer;
    
    private void OnEnable()
    {
        buyButton.onClick.AddListener(OnClick);    
    }

    private void OnDisable()
    {
        buyButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        OnClicked?.Invoke(offer);
    }

    public void Setup(GamePassOffer _offer, bool _showBuyButton)
    {
        offer = _offer;
        imageDisplay.sprite = offer.GamePass.Sprite;
        coinsDisplay.text = offer.GamePass.Coins.ToString();
        storageDisplay.text = offer.GamePass.StorageSize.ToString();
        costDisplay.text = offer.Cost.ToString();
        buyButton.gameObject.SetActive(_showBuyButton);
    }

}
