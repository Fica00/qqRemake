using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelRewardsPanel : MonoBehaviour
{
   [SerializeField] private Transform holder;
   [SerializeField] private LevelRewardDisplay rewardPrefab;
   [SerializeField] private Button close;
   [SerializeField] private TextMeshProUGUI currentLevelDisplay;
   [SerializeField] private QoomonUnlockingPanel qoomonUnlockingPanel;
   [SerializeField] ScrollRect scrollRect;
   private List<GameObject> shownRewards = new();

   private void Start()
   {
      foreach (var _shownReward in shownRewards)
      {
         Destroy(_shownReward);
      }
      shownRewards.Clear();

      int _counter = 1;
      foreach (var _levelReward in DataManager.Instance.GameData.LevelRewards.OrderByDescending(_reward => _reward.Level))
      {
         LevelRewardDisplay _display = Instantiate(rewardPrefab, holder);
         _display.Setup(_levelReward,_counter%2==0);
         shownRewards.Add(_display.gameObject);
         _counter++;
      }
      scrollRect.verticalNormalizedPosition = 0f;
      currentLevelDisplay.text = "Current level: " + DataManager.Instance.PlayerData.Level;
      gameObject.SetActive(true);
   }

   private void OnEnable()
   {
      LevelRewardDisplay.OnClicked += TryToClaim;
      close.onClick.AddListener(Close);
   }

   private void OnDisable()
   {
      LevelRewardDisplay.OnClicked -= TryToClaim;
      close.onClick.RemoveListener(Close);
   }

   private void TryToClaim(LevelReward _reward)
   {
      if (DataManager.Instance.PlayerData.Level<_reward.Level)
      {
         return;
      }

      if (DataManager.Instance.PlayerData.HasClaimedLevelReward(_reward.Level))
      {
         return;
      }

      int _randomQoomon = DataManager.Instance.PlayerData.GetQoomonFromPool();
      ClaimedLevelReward _levelReward = new ClaimedLevelReward { Level = _reward.Level, QoomonId = _randomQoomon};
      if (_randomQoomon!=-1)
      {
         qoomonUnlockingPanel.Setup(_levelReward.QoomonId,null);
         DataManager.Instance.PlayerData.AddQoomon(_levelReward.QoomonId);
      }

      DataManager.Instance.PlayerData.ClaimedLevelReward(_levelReward);
      
      Start();
   }

   private void Close()
   {
      SceneManager.Instance.LoadMainMenu();
   }
}
