using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFinishPanel : MonoBehaviour
{
   
    [SerializeField] protected GameObject panel;
    [SerializeField] protected Button button;

    public  Action OnShow;
    public Action OnCLose;

    private void OnEnable()
    {
        if (!UIMainMenu.HasShowenDeckTutorial)
        {
            OnShow += Show;
            OnCLose += Close;
            button.onClick.AddListener(Close);
        }
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(Close);
        OnShow -= Show;
        OnCLose -= Close;
    }


    private void Show()
    {
        panel.SetActive(true);
    }

    private void Close()
    {
        panel.SetActive(false);
        UIMainMenu.HasShowenDeckTutorial = true;
        SceneManager.Instance.LoadMainMenu(false);
    }
    

}
