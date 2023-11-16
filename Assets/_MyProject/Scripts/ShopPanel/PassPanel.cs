using UnityEngine;
using UnityEngine.UI;

public class PassPanel : BasePanel
{
    [SerializeField] private Button buyButton;
    [SerializeField] private Button sellButton;

    [SerializeField] private BuyPassPanel buyPassPanel;
    [SerializeField] private SellPassPanel sellPassPanel;
    
    [SerializeField] private GamePassStorageDisplay passesDisplay;

    private void OnEnable()
    {
        buyButton.onClick.AddListener(ShowBuyPanel);
        sellButton.onClick.AddListener(ShowSellPanel);
        PlayerData.UpdatedGamePasses += passesDisplay.Show;
    }

    private void OnDisable()
    {
        buyButton.onClick.RemoveListener(ShowBuyPanel);
        sellButton.onClick.RemoveListener(ShowSellPanel);
        PlayerData.UpdatedGamePasses -= passesDisplay.Show;
    }

    public override void Show()
    {
        ShowBuyPanel();
        passesDisplay.Show();
        gameObject.SetActive(true);
    }

    private void ShowBuyPanel()
    {
        CloseAll();
        buyPassPanel.Show();
    }

    private void ShowSellPanel()
    {
        CloseAll();
        sellPassPanel.Show();
    }

    private void CloseAll()
    {
        buyPassPanel.Close();
        sellPassPanel.Close();
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
