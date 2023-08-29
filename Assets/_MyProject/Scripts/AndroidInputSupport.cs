using TMPro;
using UnityEngine;

public class AndroidInputSupport : MonoBehaviour
{
    private TMP_InputField inputField;
    private bool isAndroid;
    private void Awake()
    {
        if (!(Screen.height > Screen.width || Application.platform == RuntimePlatform.WindowsEditor))
        {
            isAndroid = false;
        }
        else
        {
            isAndroid = true;
        }
        inputField = GetComponent<TMP_InputField>();
    }

    private void OnEnable()
    {
        inputField.onSelect.AddListener(EnableAndroidInput);
        inputField.onDeselect.AddListener(DisableAndroidInput);
    }

    private void OnDisable()
    {
        inputField.onSelect.RemoveListener(EnableAndroidInput);
        inputField.onDeselect.RemoveListener(DisableAndroidInput);
    }

    private void DisableAndroidInput(string _arg0)
    {
        if (!isAndroid)
        {
            return;
        }
        JavaScriptManager.Instance.HideKeyboard();
    }

    private void EnableAndroidInput(string _arg0)
    {
        if (!isAndroid)
        {
            return;
        }
        EnableAndroidInput();
    }

    private void EnableAndroidInput()
    {
        Debug.Log("Sending request for the input");
        JavaScriptManager.Instance.DisplayKeyboard();
        JavaScriptManager.UpdatedInput.AddListener(ShowInput);
    }

    private void ShowInput(string _text)
    {
        inputField.text = _text;
    }
}
