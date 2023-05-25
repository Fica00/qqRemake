using UnityEngine;
using TMPro;
using System.Collections;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class LocationAbilityDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI abilityDesc;
    [SerializeField] Image lightUpEffect;
    [SerializeField] Image tableDisplay;
    Action callback;

    public void Reveal(string _desc, Action _callback)
    {
        callback = _callback;
        abilityDesc.text = _desc;
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
}
