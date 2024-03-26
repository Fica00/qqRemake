using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultHandler : MonoBehaviour
{
   [SerializeField] private Image resultDisplay;
   [SerializeField] private Image treasureDisplay;
   [SerializeField] private List<ResultSprite> sprites;
   [SerializeField] private Button quit;
   [SerializeField] private Button playAgain;
   
   public void Show(GameResult _result)
   {
      gameObject.SetActive(true);
      ResultSprite _resultSprite = sprites.Find(_sprite => _sprite.Result == _result);
      resultDisplay.sprite = _resultSprite.Sprite;
      treasureDisplay.sprite = _resultSprite.Treasure;
   }

   private void OnEnable()
   {
      quit.onClick.AddListener(Quit);
      playAgain.onClick.AddListener(PlayAgain);
   }

   private void OnDisable()
   {
      quit.onClick.RemoveListener(Quit);
      playAgain.onClick.RemoveListener(PlayAgain);
   }
   
   private void Quit()
   {
      SceneManager.LoadMainMenu();
   }

   private void PlayAgain()
   {
      if (SceneManager.IsAIScene)
      {
         SceneManager.ReloadScene();
      }
      else
      {
         UIPlayPanel.PlayAgain=true;
         SceneManager.LoadMainMenu();
      }
   }
}
