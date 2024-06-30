using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEnterNamePanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button button;
    [SerializeField] private TMP_InputField inputField;

    private Action OnNameSubmited;

    private void OnEnable()
    {
        button.onClick.AddListener(SubmitName);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(SubmitName);
    }

    public void Show(Action OnNameSubmited)
    {
        this.OnNameSubmited = OnNameSubmited;
        panel.SetActive(true);

    }


    private void SubmitName()
    {
        Debug.Log("inputField"+inputField.text);
        // DataManager.Instance.PlayerData.Name 

        if (TryUpdateName())
        {
            panel.SetActive(false);
            OnNameSubmited?.Invoke();
        }
        
        
    }
    
    private bool TryUpdateName()
    {
        string _name = inputField.text;
        if (string.IsNullOrEmpty(_name))
        {
            DialogsManager.Instance.OkDialog.Setup("Please enter name");
            return false;
        }

        if (_name.Length < 3 || _name.Length > 10)
        {
            DialogsManager.Instance.OkDialog.Setup("Name must contain more than 3 characters and less than 10");
            return false;
        }

        if (DataManager.Instance.PlayerData.Name == _name)
        {
            return true;
        }

        DataManager.Instance.PlayerData.Name = _name;
        return true;
    }
    
}
