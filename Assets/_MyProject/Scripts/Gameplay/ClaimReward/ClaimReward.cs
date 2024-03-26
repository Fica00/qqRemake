using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClaimReward : MonoBehaviour
{
   [SerializeField] private Button claim;
   [SerializeField] private GameObject claimHolder;
   [SerializeField] private GameObject levelHolder;
   [SerializeField] private Image levelFill;
   [SerializeField] private Image resultDisplay;
   [SerializeField] private TextMeshProUGUI levelDisplay;
   [SerializeField] private TextMeshProUGUI progressDisplay;
   [SerializeField] private Button next;
   
   [SerializeField] private Sprite won;
   [SerializeField] private Sprite escaped;

   private GameResult result;

   private void OnEnable()
   {
      claim.onClick.AddListener(ShowLevel);
      next.onClick.AddListener(Leave);
   }

   private void OnDisable()
   {
      claim.onClick.RemoveListener(ShowLevel);
      next.onClick.AddListener(Leave);
   }

   private void ShowLevel()
   {
      claimHolder.SetActive(false);
      levelHolder.SetActive(true);
   }

   private void Leave()
   {
      GameplayUI.Instance.ShowResultHandler(result);
      gameObject.SetActive(false);
   }

   public void Setup(GameResult _result)
   {
      DataManager.Instance.PlayerData.Exp += 25;
      result = _result;
      resultDisplay.sprite = _result == GameResult.IWon ? won : escaped;
      levelDisplay.text = DataManager.Instance.PlayerData.Level.ToString();
      progressDisplay.text = $"{DataManager.Instance.PlayerData.CurrentExpOnLevel}/100";
      levelFill.fillAmount = DataManager.Instance.PlayerData.LevelPercentage;
      claimHolder.SetActive(true);
      levelHolder.SetActive(false);
      gameObject.SetActive(true);
   }
}
