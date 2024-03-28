using UnityEngine;
using TMPro;
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class LocationAbilityDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI abilityDesc;
    [SerializeField] private GameObject abilityDescHolder;
    [SerializeField] private TextMeshProUGUI shiningDesc;
    [SerializeField] private Image lightUpEffect;
    [SerializeField] private Image tableDisplay;
    [SerializeField] private Image background;
    [SerializeField] private Color unactiveColor;
    [SerializeField] private Color greyColor;
    private Action callback;
    private Sequence descFlashing;

    public void Reveal(string _desc, Action _callback)
    {
        background.color = Color.black;
        callback = _callback;
        abilityDesc.text = _desc;
        shiningDesc.text = _desc;
        StartCoroutine(ShowAnimation());
    }
    
    public void Reveal(string _desc)
    {
        background.color = greyColor;
        abilityDesc.text = _desc;
        shiningDesc.text = _desc;
        StartCoroutine(ShowAnimation());
    }

    private IEnumerator ShowAnimation()
    {
        abilityDescHolder.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        abilityDescHolder.SetActive(true);
        yield return new WaitForSeconds(2);
        callback?.Invoke();
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
        descFlashing.Append(DOTween.To(() => _color.a, x => _color.a = x, 0, _duration).OnUpdate(() => { shiningDesc.color = _color;}));
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
        abilityDesc.color=unactiveColor;
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
