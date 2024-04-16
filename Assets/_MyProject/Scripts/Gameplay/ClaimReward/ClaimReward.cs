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
   [SerializeField] private int expReward;
   [SerializeField] private TextMeshProUGUI expDisplay;
   [SerializeField] private Image rankImage;
   [SerializeField] private MissionDisplayAfterGame missionDisplayAfterGame;
   private bool didIWin;
   
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
      levelFill.fillAmount = RankSo.GetRankData(DataManager.Instance.PlayerData.RankPoints).Percentage;
      treasureFade.FadeOut(1, null);
      claimFade.FadeOut(1,() =>
      {
         claimHolder.SetActive(false);
         levelHolder.SetActive(true);
         levelFade.FadeIn(1, () =>
         {
            if (didIWin)
            {
               DataManager.Instance.PlayerData.RankPoints += GameplayManager.Instance.CurrentBet;
            }
            else
            {
               DataManager.Instance.PlayerData.RankPoints -= GameplayManager.Instance.CurrentBet;
            }
            levelFill.DOFillAmount(RankSo.GetRankData(DataManager.Instance.PlayerData.RankPoints).Percentage, 1f);
            ShowProgress();
            missionDisplayAfterGame.Setup();
         });
      });
   }

   private void Leave()
   {
      next.interactable=false;
      GameplayUI.Instance.ClosingAnimation(() =>
      {
         UIMainMenu.ShowStartingAnimation = true;
         SceneManager.Instance.LoadMainMenu(false);
      });
   }

   public void Setup(GameResult _result)
   {
      claimHolder.SetActive(true);
      levelHolder.SetActive(false);
      gameObject.SetActive(true);
      expDisplay.text = $"+{expReward} XP";
      Sprite _sprite;
      switch (_result)
      {
         case GameResult.IWon:
            _sprite = won;
            didIWin = true;
            AudioManager.Instance.PlaySoundEffect(AudioManager.WIN);
            break;
         case GameResult.ILost:
            _sprite = lost;
            AudioManager.Instance.PlaySoundEffect(AudioManager.LOSE);
            break;
         case GameResult.Draw:
            AudioManager.Instance.PlaySoundEffect(AudioManager.DRAW);
            didIWin = true;
            GameplayManager.Instance.HalfCurrentBetWithoutNotify();
            _sprite = draw;
            break;
         case GameResult.IForefiet:
            AudioManager.Instance.PlaySoundEffect(AudioManager.LOSE);
            _sprite = escaped;
            break;
         case GameResult.Escaped:
            didIWin = true;
            AudioManager.Instance.PlaySoundEffect(AudioManager.WIN);
            _sprite = won;
            break;
         default:
            throw new ArgumentOutOfRangeException(nameof(_result), _result, null);
      }

      DataManager.Instance.PlayerData.AmountOfRankGamesPlayed++;
      resultDisplay.sprite = _sprite;
      DataManager.Instance.PlayerData.Exp += expReward;
      ShowProgress();
   }

   private void ShowProgress()
   {
      RankData _rankData = RankSo.GetRankData(DataManager.Instance.PlayerData.RankPoints);
      progressDisplay.text = $"{_rankData.PointsOnRank}/{_rankData.RankSo.AmountOfOrbs}";
      levelDisplay.text = _rankData.Level.ToString();
      rankImage.sprite = _rankData.RankSo.Sprite;
   }
}
