using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ResultHandler : MonoBehaviour
{
   [SerializeField] private Image resultDisplay;
   [SerializeField] private Image treasureDisplay;
   [SerializeField] private Image opponentForfeited;
   [SerializeField] private List<ResultSprite> sprites;
   [SerializeField] private ClaimReward claimReward;
   [SerializeField] private FadeAnimations fadeAnimations;
   [SerializeField] private GameObject holder;
   private GameResult result;
   
   public void Show(GameResult _result)
   {
      result = _result;
      gameObject.SetActive(true);
      if (result == GameResult.Escaped)
      {
         opponentForfeited.DOFade(1, 1).OnComplete(() =>
         {
            Invoke(nameof(Setup),3);
         });
      }
      else
      {
         Setup();
      }
   }

   private void Setup()
   {
      holder.SetActive(true);
      opponentForfeited.gameObject.SetActive(false);
      ResultSprite _resultSprite = sprites.Find(_sprite => _sprite.Result == result);
      resultDisplay.sprite = _resultSprite.Sprite;
      resultDisplay.SetNativeSize();
      treasureDisplay.sprite = _resultSprite.Treasure;
      fadeAnimations.FadeIn(1, () =>
      {
         claimReward.Setup(result);
      });
   }
}
