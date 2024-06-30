using System;
using UnityEngine;
using UnityEngine.UI;

public class UiTutorialFlowGamePlay : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button button;

    public static Action OnShow;
    public static Action OnClose;

    public void OnEnable()
    {
        OnShow += Show;
        button.onClick.AddListener(Close);
    }
    
    public void OnDisable()
    {
        OnShow -= Show;
        button.onClick.RemoveListener(Close);
    }
    

    public void Show()
    {
        panel.SetActive(true);        
        
        //Tu cekaj animaciju da odradi
    }
    
    
    //Ubaci callback ako ti bude trebalo
    public void Close()
    {
        panel.SetActive(false);        
    }
    
}
