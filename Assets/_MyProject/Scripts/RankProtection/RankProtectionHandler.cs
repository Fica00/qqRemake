using UnityEngine;

public class RankProtectionHandler : MonoBehaviour
{
   [SerializeField] private GameObject holder;

   private void OnEnable()
   {
      if (DataManager.Instance.PlayerData.SawBetTutorial)
      {
         return;
      }

      DataManager.Instance.PlayerData.SawBetTutorial = true;
      Show();
   }

   private void Show()
   {
      holder.SetActive(true);
   }
}
