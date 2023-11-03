using UnityEngine;
using UnityEngine.UI;

public class SellPassPanel : BasePanel
{
    [SerializeField] private Button goToMarketplace;

    private void OnEnable()
    {
        goToMarketplace.onClick.AddListener(ShowMarketplace);
    }

    private void OnDisable()
    {
        goToMarketplace.onClick.RemoveListener(ShowMarketplace);
    }

    private void ShowMarketplace()
    {
        ShopPanel.Instance.ShowMarketplace(false);
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
