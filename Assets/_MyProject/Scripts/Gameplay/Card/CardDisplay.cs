using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private CardInHandDisplay cardInHandDisplay;
    [SerializeField] private CardOnTableDisplay cardOnTableMyDisplay;
    [SerializeField] private CardOnTableDisplay cardOnTableOpponentDisplay;
    [SerializeField] private GameObject destroyEffect;

    private CardOnTableDisplay cardOnTableHandler;
    private CardObject cardObject;

    public void Setup(CardObject _cardObject)
    {
        cardObject = _cardObject;
        cardOnTableHandler = _cardObject.IsMy ? cardOnTableMyDisplay : cardOnTableOpponentDisplay;
        cardInHandDisplay.Setup(_cardObject);
        cardOnTableHandler.Setup(_cardObject);
    }

    public void ShowDrawnAnimation()
    {
        ShowCardInHand();
        cardInHandDisplay.ShowDrawnAnimation();
    }

    public void ShowCardInHand()
    {
        cardInHandDisplay.Show();
        cardOnTableHandler.Hide();
    }

    public void HideCardInHand()
    {
        cardInHandDisplay.Hide();
    }

    public void ShowCardOnTable()
    {
        cardOnTableHandler.Show();
        cardInHandDisplay.Hide();
    }

    public void Hide()
    {
        cardOnTableHandler.Hide();
        cardInHandDisplay.Hide();
    }

    public void HideCardOnTable()
    {
        cardOnTableHandler.Hide();
    }

    public void DiscardInHandAnimation(Action _callBack)
    {

        Image _qommonDisplay = cardInHandDisplay.QommonDisplay;
        Vector3 _endValue = _qommonDisplay.transform.localPosition;
        _endValue.y += 200;
        cardInHandDisplay.transform.DOLocalMove(_endValue, 0.5f).OnComplete(() =>
        {
            Color _color = _qommonDisplay.color;
            DOTween.To(() => _color.a, x => _color.a = x, 0, 0.3f).OnUpdate(() => { _qommonDisplay.color = _color; })
                .OnComplete(() =>
                {
                    _callBack?.Invoke();
                    _color.a = 1;
                    _qommonDisplay.color = _color;
                });
        });
    }

    public void ShowDestroyEffect(Action _callBack)
    {
        StartCoroutine(DestroyRoutine());
        
        IEnumerator DestroyRoutine()
        {
            destroyEffect.SetActive(true);
            yield return new WaitForSeconds(0.5f);

            Image _qommonImage = cardOnTableHandler.QommonDisplay;
            Color _color = _qommonImage.color;
            DOTween.To(() => _color.a, x => _color.a = x, 0, 0.5f).
                OnUpdate(() => { _qommonImage.color = _color; });

            yield return new WaitForSeconds(0.5f);
            destroyEffect.SetActive(false);
            _callBack?.Invoke();
        }
    }

    public void ShowBladesEffect()
    {
        StartCoroutine(BaledsRoutine());
        
        IEnumerator BaledsRoutine()
        {
            destroyEffect.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            destroyEffect.SetActive(false);
        }
    }

    public void EnlargedPowerAnimation(bool _showForMyQommon)
    {
        StartCoroutine(ShowAnimation());
        
        IEnumerator ShowAnimation()
        {
            yield return new WaitForSeconds(0.1f);
            if (_showForMyQommon)
            {
                cardOnTableMyDisplay.EnlargedPowerAnimation();
            }
            else
            {
                cardOnTableOpponentDisplay.EnlargedPowerAnimation();
            }
        }
    }

    public void ForcePowerTextUpdateOcto()
    {
        cardInHandDisplay.Show();
    }

    public void ForcePowerTextUpdateOnTable()
    {
        cardOnTableHandler.Show();
    }
}
