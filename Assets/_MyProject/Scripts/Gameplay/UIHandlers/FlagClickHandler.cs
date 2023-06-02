using System;
using UnityEngine;
using UnityEngine.UI;

public class FlagClickHandler : MonoBehaviour
{
    public static Action OnClick;

    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(Forfiet);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(Forfiet);
    }

    void Forfiet()
    {
        OnClick?.Invoke();
    }
}
