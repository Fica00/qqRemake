using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OpenUrlOnClick : MonoBehaviour
{
    [SerializeField] private string url;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OpenUrl);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OpenUrl);
    }

    private void OpenUrl()
    {
        Application.OpenURL(url);
    }
}
