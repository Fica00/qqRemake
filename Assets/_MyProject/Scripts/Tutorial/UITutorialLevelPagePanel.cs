using System;
using UnityEngine;

public class UITutorialLevelPagePanel : MonoBehaviour
{
   [SerializeField] private GameObject panel;

   public  Action OnShow;
   public  Action OnClose;
   


   private void OnEnable()
   {
      if (UIMainMenu.HasPlayedFirstAiGame)
      {
         OnShow += Show;
         OnClose += Close;
      }
   }

   private void OnDisable()
   {
     
         
         OnShow -= Show;
         OnClose -= Close;
      
      
   }


   public void Show()
   {
      panel.SetActive(true);
   }


   private void Close()
   {
      panel.SetActive(false);
   }

}
