using UnityEngine;
using TMPro;
using System;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class LocationAbilityDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI abilityDesc;
    [SerializeField] private TextMeshProUGUI shiningDesc;
    [SerializeField] Image lightUpEffect;
    [SerializeField] Image tableDisplay;
    Action callback;
    private Sequence descFlashing;

    public void Reveal(string _desc, Action _callback)
    {
        callback = _callback;
        abilityDesc.text = _desc;
        shiningDesc.text = _desc;
        FlashEffect();
    }

    void FlashEffect()
    {
        Sequence _sequence = DOTween.Sequence();
        Color _color = lightUpEffect.color;
        _color.a = 1;
        float _duration = 0.2f;
        _sequence.Append(lightUpEffect.DOColor(_color, _duration));
        _color.a = 0;
        _sequence.Append(lightUpEffect.DOColor(_color, _duration));
        _color.a = 1;
        _sequence.Append(lightUpEffect.DOColor(_color, _duration));
        _color.a = 0;
        _sequence.Append(lightUpEffect.DOColor(_color, _duration));
        _sequence.OnComplete(() =>
            {
                ShowDesc();
            });
        _sequence.Play();
    }

    void ShowDesc()
    {
        Sequence _sequence = DOTween.Sequence();
        float _duration = 0.5f;
        _sequence.Append(tableDisplay.DOColor(new Color(1, 1, 1, 1), _duration));
        Color _color = abilityDesc.color;
        _color.a = 1;
        _sequence.Join(abilityDesc.DOColor(_color, _duration));
        _sequence.OnComplete(() =>
        {
            callback?.Invoke();
        });
    }
    
    public void AbilityShowAsActive()
    {
        if (descFlashing!=null)
        {
            return;
        }

        descFlashing = DOTween.Sequence();
        Color _color = shiningDesc.color;
        float _duration = 1;
        descFlashing.Append(DOTween.To(() => _color.a, x => _color.a = x, 1, _duration).OnUpdate(() => { shiningDesc.color = _color; }));
        descFlashing.Append(DOTween.To(() => _color.a, x => _color.a = x, 0, _duration).OnUpdate(() => { shiningDesc.color = _color; }));
        descFlashing.SetLoops(-1);
        descFlashing.Play();
    }

    public void AbilityShowAsInactive()
    {
        if (descFlashing==null)
        {
            return;
        }
        
        descFlashing.Kill();
        descFlashing = null;
        Color _color = shiningDesc.color;
        _color.a = 0;
        shiningDesc.color = _color;
    }

    public void AbilityFlash()
    {
        StartCoroutine(FlashEffect());
        
        IEnumerator FlashEffect()
        {
            AbilityShowAsActive();
            yield return new WaitForSeconds(1.5f);
            AbilityShowAsInactive();
        }
    }
}
