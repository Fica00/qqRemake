using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class UINewChallengerAwaitsPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button button;
    public static Action OnChallangerAwaitsClick;
    private void OnEnable()
    {
       button.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(Close);
    }

    public void Setup()
    {
        panel.SetActive(true);
    }
    

    private void Close()
    {
        panel.SetActive(false);
        OnChallangerAwaitsClick?.Invoke();
    }
}
