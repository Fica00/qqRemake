using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIPopOpenYourDeckPanel : MonoBehaviour
{
    [SerializeField] private UINewCardGreatPanel uiNewCardGreatPanel;
   
    
    [SerializeField] protected GameObject panel;
    //[SerializeField] protected Button button;

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


    private  void Show()
    {
        panel.SetActive(true);
    }

    private  void Close()
    {
        panel.SetActive(false);
        uiNewCardGreatPanel.OnShow?.Invoke();
    }
    
}
