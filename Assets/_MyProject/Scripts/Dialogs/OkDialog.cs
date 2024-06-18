using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class OkDialog : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnOkPressed;
    [SerializeField] private TextMeshProUGUI messageDisplay;
    [SerializeField] private Button okButton;

    public void Setup(string _message)
    {
        messageDisplay.text = _message;
        gameObject.SetActive(true);
        PersistingUI.Instance.UpdateCanvasOrder();
    }

    private void OnEnable()
    {
        okButton.onClick.AddListener(OkPressed);
    }

    private void OnDisable()
    {
        okButton.onClick.RemoveListener(OkPressed);
    }

    private void OkPressed()
    {
        OnOkPressed?.Invoke();
        Close();
    }

    private void Close()
    {
        OnOkPressed.RemoveAllListeners();
        gameObject.SetActive(false);
    }

}
