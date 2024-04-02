using System;
using DG.Tweening;
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
   [SerializeField] private Sprite draw;
   [SerializeField] private Sprite lost;
   [SerializeField] private FadeAnimations treasureFade;
   [SerializeField] private FadeAnimations claimFade;
   [SerializeField] private FadeAnimations levelFade;

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
      levelFill.fillAmount = DataManager.Instance.PlayerData.LevelPercentage;
      treasureFade.FadeOut(1, null);
      claimFade.FadeOut(1,() =>
      {
         claimHolder.SetActive(false);
         levelHolder.SetActive(true);
         levelFade.FadeIn(1, () =>
         {
            DataManager.Instance.PlayerData.Exp += GameplayManager.Instance.CurrentBet;
            levelFill.DOFillAmount(DataManager.Instance.PlayerData.LevelPercentage, 1f);
            ShowProgress();
         });
      });
   }

   private void Leave()
   {
      next.interactable=false;
      GameplayUI.Instance.ClosingAnimation(() =>
      {
         UIMainMenu.ShowStartingAnimation = true;
         SceneManager.Instance.LoadMainMenu();
      });
   }

   public void Setup(GameResult _result)
   {
      claimHolder.SetActive(true);
      levelHolder.SetActive(false);
      gameObject.SetActive(true);
      Sprite _sprite;
      switch (_result)
      {
         case GameResult.IWon:
            _sprite = won;
            break;
         case GameResult.ILost:
            _sprite = lost;
            break;
         case GameResult.Draw:
            _sprite = draw;
            break;
         case GameResult.IForefiet:
            _sprite = escaped;
            break;
         case GameResult.Escaped:
            _sprite = escaped;
            break;
         default:
            throw new ArgumentOutOfRangeException(nameof(_result), _result, null);
      }

      resultDisplay.sprite = _sprite;
      ShowProgress();
   }

   private void ShowProgress()
   {
      progressDisplay.text = $"{DataManager.Instance.PlayerData.CurrentExpOnLevel}/{DataManager.Instance.PlayerData.GetXpForNextLevel()}";
      levelDisplay.text = DataManager.Instance.PlayerData.Level.ToString();
   }
}
