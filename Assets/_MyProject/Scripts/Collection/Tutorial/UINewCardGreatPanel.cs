using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;

public class UINewCardGreatPanel : MonoBehaviour
{
    [SerializeField]private UIReplacePanel uiReplacePanel;
 
    
    [SerializeField] protected GameObject panel;
    [SerializeField] protected Button button;

    public  Action OnShow;
    public static Action  OnClose;

    private void OnEnable()
    {
        if (!UIMainMenu.HasShowenDeckTutorial)
        {
            OnShow += Show;
            OnClose += Close;
            button.onClick.AddListener(Close);
        }
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(Close);
        OnShow -= Show;
        OnClose -= Close;
    }


    private void Show()
    {
        panel.SetActive(true);
    }

    private void Close()
    {
        OnShow -= Show;
        panel.SetActive(false);
        uiReplacePanel.OnShow?.Invoke();
    }
    
}
