using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1000)]  //Pitati sutra ficu
public class UIGoToCollectionPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button button;

    private void Awake()
    {
        UIMainMenu.OnGoToCollection += Setup;
        Debug.Log("2.1.2");
    }

    private void OnEnable()
    {
        button.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        UIMainMenu.OnGoToCollection -= Setup;
        button.onClick.RemoveListener(Close);
    }

    public void Setup()
    {
        Debug.Log(2.2);
        panel.SetActive(true);
    }
    
    private void Close()
    {
       
        panel.SetActive(false);
    }
}
