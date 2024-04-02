using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelRewardsPanel : MonoBehaviour
{
   public static Action<int> OnUnlockedNewQoomon;
   [SerializeField] private Transform holder;
   [SerializeField] private LevelRewardDisplay rewardPrefab;
   [SerializeField] private Button close;
   [SerializeField] private TextMeshProUGUI currentLevelDisplay;
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

      if (DataManager.Instance.PlayerData.ClaimedLevelRewards.Contains(_reward.Level))
      {
         return;
      }
      
      DataManager.Instance.PlayerData.ClaimedLevelReward(_reward.Level);

      if (!DataManager.Instance.PlayerData.OwnedQoomons.Contains(_reward.QoomonId))
      {
         DataManager.Instance.PlayerData.AddQoomon(_reward.QoomonId);
      }
      
      OnUnlockedNewQoomon?.Invoke(_reward.QoomonId);
      Start();
   }

   private void Close()
   {
      SceneManager.Instance.LoadMainMenu();
   }
}
