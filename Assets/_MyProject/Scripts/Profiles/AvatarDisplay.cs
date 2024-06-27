using System;
using UnityEngine;
using UnityEngine.UI;

public class AvatarDisplay : MonoBehaviour
{
    public static Action<AvatarSo> OnClicked;
    [SerializeField] private Image avatarDisplay;
    [SerializeField] private Button button;
    private AvatarSo avatarSo;

    public void Setup(AvatarSo _avatar)
    {
        avatarSo = _avatar;
        avatarDisplay.sprite = _avatar.Sprite;
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
        OnClicked?.Invoke(avatarSo);
    }
}
