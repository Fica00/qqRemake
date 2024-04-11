using System;
using UnityEngine;
using UnityEngine.UI;

public class UIClick : MonoBehaviour
{
   [SerializeField] private ButtonType type;
   private Button button;

   private void OnEnable()
   {
      button = GetComponent<Button>();

      button.onClick.AddListener(Click);
   }

   private void OnDisable()
   {
      if (button==default)
      {
         return;
      }
      
      button.onClick.RemoveListener(Click);
   }

   private void Click()
   {
      switch (type)
      {
         case ButtonType.Click:
            AudioManager.Instance.PlaySoundEffect(AudioManager.CONFIRM);
            break;
         case ButtonType.Confirm:
            AudioManager.Instance.PlaySoundEffect(AudioManager.CONFIRM);
            break;
         case ButtonType.Cancel:
            AudioManager.Instance.PlaySoundEffect(AudioManager.BACK);
            break;
         case ButtonType.Play:
            AudioManager.Instance.PlaySoundEffect(AudioManager.PLAY);
            break;
         default:
            throw new ArgumentOutOfRangeException();
      }
   }
}
