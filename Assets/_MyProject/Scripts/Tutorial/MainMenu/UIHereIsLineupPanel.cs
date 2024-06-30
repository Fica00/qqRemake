using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHereIsLineupPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button button;

    [SerializeField] private UINewChallengerAwaitsPanel uiNewChallengerAwaits;

    
    

    private void OnEnable()
    {
        UIMainMenu.OnShowLineup += Setup;
        button.onClick.AddListener(OnClose);
    }

    private void OnDisable()
    {
        UIMainMenu.OnShowLineup -= Setup;
        button.onClick.RemoveListener(OnClose);
    }

    public void Setup()
    {
        Debug.Log("2");
        panel.SetActive(true);
    }
    
    private void OnClose()
    {
        Debug.Log("3");
        panel.SetActive(false);
        uiNewChallengerAwaits.Setup();
        
    }
}
