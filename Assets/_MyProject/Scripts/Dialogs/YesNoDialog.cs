using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class YesNoDialog : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnYesPressed;
    [HideInInspector] public UnityEvent OnNoPressed;

    [SerializeField] private TextMeshProUGUI questionDisplay;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    public void Setup(string _question)
    {
        questionDisplay.text = _question;
        gameObject.SetActive(true);
        DialogsManager.Instance.UpdateCanvasOrder();
    }

    private void OnEnable()
    {
        yesButton.onClick.AddListener(YesPressed);
        noButton.onClick.AddListener(NoPressed);
    }

    private void OnDisable()
    {
        yesButton.onClick.AddListener(YesPressed);
        noButton.onClick.AddListener(NoPressed);
    }

    private void YesPressed()
    {
        OnYesPressed?.Invoke();
        Close();
    }

    private void NoPressed()
    {
        OnNoPressed?.Invoke();
        Close();
    }

    private void Close()
    {
        OnYesPressed.RemoveAllListeners();
        OnNoPressed.RemoveAllListeners();

        gameObject.SetActive(false);
    }

}
