using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GlowAnimation : MonoBehaviour
{
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();  
    }

    private void Start()
    {
        Sequence _sequence = DOTween.Sequence();
        Color _color = image.color;
        float _animationTime = 3;

        _sequence.Append(DOTween.To(() => _color.a, x => _color.a = x, 0.5f, _animationTime).OnUpdate(() => { image.color = _color; }));
        _sequence.Append(DOTween.To(() => _color.a, x => _color.a = x, 1, _animationTime).OnUpdate(() => { image.color = _color; }));
        _sequence.SetLoops(-1);
        _sequence.Play();
    }
}
