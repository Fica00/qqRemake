using UnityEngine;

public class ShopPanel : BasePanel
{
    public static ShopPanel Instance;
    [SerializeField] private PassPanel passPanel;
    [SerializeField] private MarketplacePanel marketplacePanel;
    
    private void Awake()
    {
        Instance = this;
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
    }
    
    private void CloseAll()
    {
        passPanel.Close();
        marketplacePanel.Close();
    }
}
