using UnityEngine;
using UnityEngine.UI;

public class GradiantValueBar : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image foreground;
    [SerializeField] private RectTransform gradiantRect;
    private Image gradiant;

    private void Awake()
    {
        gradiant = gradiantRect.GetComponent<Image>();
    }

    public void SetAmount(float _value)
    {
        foreground.fillAmount = _value;
        Vector2 _gradiantPosition = gradiantRect.anchoredPosition;
        _gradiantPosition.x = (foreground.rectTransform.rect.width * _value);
        gradiantRect.anchoredPosition = _gradiantPosition;
    }

    public void SetSprites(GradiantSprite _sprite)
    {
        Debug.Log(_sprite);
        background.sprite = _sprite.Background;
        foreground.sprite = _sprite.Foreground;
        gradiant.sprite = _sprite.Gradiant;
    }

    public void SetForeground(Sprite _sprite, bool _setAmountToFull=false)
    {
        Debug.Log(_sprite.name);
        foreground.sprite = _sprite;
        if (_setAmountToFull)
        {
            SetAmount(1);
        }
    }
}
