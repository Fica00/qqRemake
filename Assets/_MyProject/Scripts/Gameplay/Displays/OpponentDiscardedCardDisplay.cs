using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OpponentDiscardedCardDisplay : MonoBehaviour
{
   public static OpponentDiscardedCardDisplay Instance;
   [SerializeField] private Image background;
   [SerializeField] private Image qommonDisplay;
   [SerializeField] private Transform endPosition;
   [SerializeField] private Transform startPosition;

   private void Awake()
   {
      Instance = this;
   }

   public void Show(int _cardId)
   {
      Color _backgroundColor = background.color;
      Color _qommonColor = qommonDisplay.color;
      qommonDisplay.sprite = CardsManager.Instance.GetCardSprite(_cardId);
      background.transform.position = startPosition.position;
      background.gameObject.SetActive(true);
      
      _backgroundColor.a = 1;
      _qommonColor.a = 1;

      background.transform.DOMove(endPosition.position, 0.5f).OnComplete(() =>
      {
         DOTween.To(() => _backgroundColor.a, x => _backgroundColor.a = x, 0, 0.5f)
            .OnUpdate(() =>
            {
               background.color = _backgroundColor;
               qommonDisplay.color = _backgroundColor;
            })
            .OnComplete(() =>
            {
               background.gameObject.SetActive(false);
            });
      });
   }
}
