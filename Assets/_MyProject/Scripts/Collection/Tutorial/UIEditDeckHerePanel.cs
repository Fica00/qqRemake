

using System;
using UnityEngine;
using UnityEngine.UI;

public class UIEditDeckHerePanel : MonoBehaviour
{
    [SerializeField] private UIPopOpenYourDeckPanel uiPopOpenYourDeckPanel;
  
    [SerializeField] protected GameObject panel;
    [SerializeField] protected Button button;

    public  Action OnShow;
    public Action OnClose;

    private void OnEnable()
    {
        Debug.Log("!UIMainMenu.HasShowenDeckTutorial"+!UIMainMenu.HasShowenDeckTutorial);
        if (!UIMainMenu.HasShowenDeckTutorial)
        {
            Show();
            OnClose += Close;
            button.onClick.AddListener(Close);
        }
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(Close);
        OnClose -= Close;
    }


    private void Show()
    {
        panel.SetActive(true);
    }

    private void Close()
    {
        panel.SetActive(false);
        uiPopOpenYourDeckPanel.OnShow?.Invoke();
        
    }
}
