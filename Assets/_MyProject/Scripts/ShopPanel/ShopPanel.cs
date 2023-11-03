using UnityEngine;

public class ShopPanel : BasePanel
{
    public static ShopPanel Instance;
    [SerializeField] private PassPanel passPanel;
    [SerializeField] private MarketplacePanel marketplacePanel;
    [SerializeField] private GamePassOffersDisplay offersDisplay;
    
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        ShowMain();
    }

    private void OnDisable()
    {
        
    }

    public void ShowMarketplace(bool _canBuy)
    {
        CloseAll();
        marketplacePanel.Show(_canBuy);
    }

    public void ShowMain()
    {
        CloseAll();
        passPanel.Show();
        offersDisplay.Show();
    }
    
    private void CloseAll()
    {
        passPanel.Close();
        marketplacePanel.Close();
        offersDisplay.Close();
    }

    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
