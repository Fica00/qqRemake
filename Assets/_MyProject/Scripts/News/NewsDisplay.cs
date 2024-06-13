using System;
using UnityEngine;
using UnityEngine.UI;

public class NewsDisplay : MonoBehaviour
{
    public static Action OnClicked;
    [SerializeField] private Image imageDisplay;
    [SerializeField] private Button button;
    [SerializeField] private Button close;

    private string url;
    private bool canClose;

    public void Setup(NewsData _data)
    {
        imageDisplay.sprite = _data.Sprite;
        url = _data.Url;
        canClose = _data.AllowToClose;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    

    private void OnEnable()
    {
        button.onClick.AddListener(Click);
        close.onClick.AddListener(ClosePressed);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(Click);
        close.onClick.RemoveListener(ClosePressed);
    }

    private void Click()
    {
        DoClose();
        if (string.IsNullOrEmpty(url))
        {
            return;
        }
        Application.OpenURL(url);
    }
    
    private void ClosePressed()
    {
        if (!canClose)
        {
            return;
        }

        DoClose();
    }

    private void DoClose()
    {
        OnClicked?.Invoke();
        gameObject.SetActive(false);
    }
}
