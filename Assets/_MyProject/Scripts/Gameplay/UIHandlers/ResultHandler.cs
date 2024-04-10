using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultHandler : MonoBehaviour
{
   [SerializeField] private Image resultDisplay;
   [SerializeField] private Image treasureDisplay;
   [SerializeField] private List<ResultSprite> sprites;
   [SerializeField] private ClaimReward claimReward;
   [SerializeField] private FadeAnimations fadeAnimations;
   
   public void Show(GameResult _result)
   {
      gameObject.SetActive(true);
      ResultSprite _resultSprite = sprites.Find(_sprite => _sprite.Result == _result);
      resultDisplay.sprite = _resultSprite.Sprite;
      resultDisplay.SetNativeSize();
      treasureDisplay.sprite = _resultSprite.Treasure;
      fadeAnimations.FadeIn(1, () =>
      {
         claimReward.Setup(_result);
      });
   }
}
