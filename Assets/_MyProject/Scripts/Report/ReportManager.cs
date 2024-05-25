using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class ReportManager : MonoBehaviour
{
    public static ReportManager Instance;

    [SerializeField] private GameObject reportButtonContainerGO;
    [SerializeField] private Button reportButton;
    [SerializeField] private Button closeReportButton;
    [SerializeField] private Button sendReportButton;
    [SerializeField] private InputField reportText;
    [SerializeField] private bool isHideReport;

    private void Awake()
    {
        if (Instance == null)
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
        CheckIsHided();
    }

    private void OnDestroy()
    {
        reportButton.onClick.RemoveListener(OpenReportInputField);
        closeReportButton.onClick.RemoveListener(Close);
        sendReportButton.onClick.RemoveListener(OnSendReport);
    }

    private void CheckIsHided()
    {
        
        if(isHideReport)
        {
            reportButton.gameObject.SetActive(false);
        }
        else
        {
            reportButton.gameObject.SetActive(true);
        }
    }
    
    private void ClearInputField()
    {
        reportText.text = "";
    }

    private void OnDisable()
    {
        reportButton.onClick.RemoveListener(OpenReportInputField);
        closeReportButton.onClick.RemoveListener(Close);
        sendReportButton.onClick.RemoveListener(OnSendReport);
    }

    private void OpenReportInputField()
    {
        reportButtonContainerGO.SetActive(true);
        reportButton.gameObject.SetActive(false);
        sendReportButton.gameObject.SetActive(true);
    }

    
    private void OnSendReport()
    {
        if (string.IsNullOrEmpty(reportText.text))
        {
            DialogsManager.Instance.OkDialog.Setup("Please fill in the report filed");
            return;
        }

        ReportDate _reportData = new ReportDate
        {
            UserID = FirebaseManager.Instance.PlayerId,
            ReportDateTime = DateTime.Now.ToString(),
            ReportStringText = reportText.text,
            SceneName = SceneManager.CurrentSceneName,
            Device = DeviceData.Get(),
            PlayerData = JsonConvert.SerializeObject(DataManager.Instance.PlayerData)
        };

        FirebaseManager.Instance.ReportBug(_reportData, Successfully, Failed);
    }

    private void Successfully(string _data)
    {
        DialogsManager.Instance.OkDialog.Setup("Successfully reported!");
        Close();
    }

    private void Failed(string _data)
    {
        DialogsManager.Instance.OkDialog.Setup("Failed to report, please try again");
    }

    private void Close()
    {
        sendReportButton.gameObject.SetActive(false);
        reportButton.gameObject.SetActive(true);
        reportButtonContainerGO.SetActive(false);
    }
}