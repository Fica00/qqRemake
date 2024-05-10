using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReportManager : MonoBehaviour
{
    public static ReportManager Instance;

    [SerializeField] private GameObject reportButtonContainerGO;
    [SerializeField] private Button reportButton;
    [SerializeField] private Button closeReportButton;
    [SerializeField] private Button sendReportButton;
    [SerializeField] private InputField reportText;



    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        reportButton.onClick.AddListener(OpenReportInputField);
        closeReportButton.onClick.AddListener(Close);
        sendReportButton.onClick.AddListener(OnSendReport);
        ClearInputField();
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += RefreshCanvas;
    }

    

    private void OnDisable()
    {
        reportButton.onClick.RemoveListener(OpenReportInputField);
        closeReportButton.onClick.RemoveListener(Close);
        sendReportButton.onClick.RemoveListener(OnSendReport);
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= RefreshCanvas;


    }

    private void RefreshCanvas(Scene arg0, LoadSceneMode arg1)
    {
        DialogsManager.Instance.UpdateCanvasOrder();
    }

    private void OpenReportInputField()
    {
        DialogsManager.Instance.UpdateCanvasOrder();

        reportButtonContainerGO.SetActive(true);
        reportButton.gameObject.SetActive(false);
        sendReportButton.gameObject.SetActive(true);

    }

    private void OnSendReport()
    {

        if (string.IsNullOrEmpty(reportText.text))
        {
            DialogsManager.Instance.OkDialog.Setup("Please report");
            return;
        }

        ReportDate _reportData = new ReportDate();

        _reportData.UserID = FirebaseManager.Instance.PlayerId;
        _reportData.ReportDateTime = DateTime.Now.ToString();
        _reportData.ReportStringText = reportText.text;
        _reportData.SceneName = SceneManager.CurrentSceneName;
        //_reportData.Device =

        FirebaseManager.Instance.ReportBug(_reportData,Successfully,Faild);

        
    }

    private void Successfully(string _data)
    {
        DialogsManager.Instance.OkDialog.Setup("Successfuly");
        sendReportButton.gameObject.SetActive(false);
        reportButton.gameObject.SetActive(true);
        reportButtonContainerGO.SetActive(false);
    }

    private void Faild(string _data)
    {
        DialogsManager.Instance.OkDialog.Setup("Faild");

    }



    private void ClearInputField()
    {
        
        reportText.text = "";
    }

    private void Close()
    {
        sendReportButton.gameObject.SetActive(false);
        reportButton.gameObject.SetActive(true);
        reportButtonContainerGO.SetActive(false);
    }
}
