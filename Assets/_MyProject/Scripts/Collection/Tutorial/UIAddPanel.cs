using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAddPanel : MonoBehaviour
{
    [SerializeField] private UIFinishPanel uiFinishPanel;
    [SerializeField] protected GameObject panel;
    

    public  Action OnShow;
    public static Action OnClose;

    private void OnEnable()
    {
        if (!UIMainMenu.HasShowenDeckTutorial)
        {
            OnShow += Show;
            OnClose += Close;
        }
    }

    private void OnDisable()
    {
        OnShow -= Show;
        OnClose -= Close;
    }


    private void Show()
    {
        panel.SetActive(true);
    }

    private void Close()
    {
        panel.SetActive(false);
        uiFinishPanel.OnShow?.Invoke();
    }
}
