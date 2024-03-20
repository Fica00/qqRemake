using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellPassPanel : BasePanel
{
    [SerializeField] private Button goToMarketplace;
    [SerializeField] private Image spriteDisplay;
    [SerializeField] private TextMeshProUGUI storageDisplay;
    [SerializeField] private TextMeshProUGUI coinsDisplay;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Button listButton;
    [SerializeField] private ListPassPanel listPassPanel;
    private GamePass showingPass;
    
    private void OnEnable()
    {
        goToMarketplace.onClick.AddListener(ShowMarketplace);
        listButton.onClick.AddListener(ListPass);
    }

    private void OnDisable()
    {
        goToMarketplace.onClick.RemoveListener(ShowMarketplace);
        listButton.onClick.RemoveListener(ListPass);
    }

    private void ShowMarketplace()
    {
        ShopPanel.Instance.ShowMarketplace(false);
    }
    
    public override void Show()
    {
        ShowPass(-1);
        gameObject.SetActive(true);
    }

    public void ListSuccessful()
    {
        showingPass = default;
        ShowPass(-1);
    }

    public void ShowPass(int _index)
    {
        if (_index==-1)
        {
            showingPass = default;
            spriteDisplay.sprite = defaultSprite;
            storageDisplay.text = "?";
            coinsDisplay.text = "? / ?";
        }
        else
        { 
            showingPass = DataManager.Instance.PlayerData.GamePasses[_index];
            spriteDisplay.sprite = showingPass.Sprite;
            storageDisplay.text = showingPass.StorageSize.ToString();
            coinsDisplay.text = "? / " + showingPass.StorageSize;
        }
        
        spriteDisplay.SetNativeSize();

    }

    private void ListPass()
    {
        if (showingPass == default)
        {
            return;
        }
        listPassPanel.Show(showingPass);   
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
