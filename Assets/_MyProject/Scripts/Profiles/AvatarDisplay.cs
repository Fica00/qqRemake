using System;
using UnityEngine;
using UnityEngine.UI;

public class AvatarDisplay : MonoBehaviour
{
    public static Action<AvatarSo> OnClicked;
    [SerializeField] private Image avatarDisplay;
    [SerializeField] private Button button;

    [SerializeField] private Image[] images;
    private AvatarSo avatarSo;
    private bool ignoreClick;

    public void Setup(AvatarSo _avatar, bool _ignoreClick=true)
    {
        ignoreClick = _ignoreClick;
        avatarSo = _avatar;
        avatarDisplay.sprite = _avatar.Sprite;
    }

    public void GreyOut()
    {
        Color _color = Color.white;
        _color.a = 0.6f;
        foreach (var _image in images)
        {
            _image.color = _color;
        }
    }

    private void OnEnable()
    {
        button.onClick.AddListener(Select);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(Select);
    }

    private void Select()
    {
        if (ignoreClick)
        {
            return;
        }
        OnClicked?.Invoke(avatarSo);
    }
}
