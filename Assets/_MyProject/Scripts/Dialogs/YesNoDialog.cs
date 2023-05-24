using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class YesNoDialog : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnYesPressed;
    [HideInInspector] public UnityEvent OnNoPressed;

    [SerializeField] TextMeshProUGUI questionDisplay;
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;

    public void Setup(string _question)
    {
        questionDisplay.text = _question;
        gameObject.SetActive(true);
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

    void YesPressed()
    {
        OnYesPressed?.Invoke();
        Close();
    }

    void NoPressed()
    {
        OnNoPressed?.Invoke();
        Close();
    }

    void Close()
    {
        OnYesPressed.RemoveAllListeners();
        OnNoPressed.RemoveAllListeners();

        gameObject.SetActive(false);
    }

}
