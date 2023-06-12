using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class GameplayYesNo : MonoBehaviour
{
    public const string FONT_GREEN = "Green";
    public const string FONT_RED = "Red";
    [HideInInspector] public UnityEvent OnLeftButtonPressed;
    [HideInInspector] public UnityEvent OnRightButtonPressed;

    [SerializeField] TextMeshProUGUI questionDisplay;
    [SerializeField] Button leftButton;
    [SerializeField] TextMeshProUGUI leftButtonText;
    [SerializeField] Button rightButton;
    [SerializeField] TextMeshProUGUI rightButtonText;

    [SerializeField] TMP_FontAsset greenFont;
    [SerializeField] TMP_FontAsset redFont;

    Button backgroundButton;

    public void Setup(string _question,string _leftButtonText,string _rightButtonText, string _fontKey)
    {
        questionDisplay.text = _question;
        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true);
        leftButtonText.text = _leftButtonText;
        rightButtonText.text = _rightButtonText;
        SetFont(_fontKey);
        gameObject.SetActive(true);
    }

    public void Setup(string _question,string _fontKey)
    {
        questionDisplay.text = _question;
        SetFont(_fontKey);
        gameObject.SetActive(true);
    }

    void SetFont(string _key)
    {
        if (_key==FONT_GREEN)
        {
            questionDisplay.font = greenFont;
        }
        else if (_key==FONT_RED)
        {
            questionDisplay.font= redFont;
        }
    }

    private void Awake()
    {
        backgroundButton=GetComponent<Button>();
    }

    private void OnEnable()
    {
        leftButton.onClick.AddListener(LeftPressed);
        backgroundButton.onClick.AddListener(LeftPressed);
        rightButton.onClick.AddListener(RightPressed);
    }

    private void OnDisable()
    {
        leftButton.onClick.RemoveListener(LeftPressed);
        backgroundButton.onClick.RemoveListener(LeftPressed);
        rightButton.onClick.RemoveListener(RightPressed);
    }

    void LeftPressed()
    {
        OnLeftButtonPressed?.Invoke();
        Close();
    }

    void RightPressed()
    {
        OnRightButtonPressed?.Invoke();
        Close();
    }

    void Close()
    {
        OnLeftButtonPressed.RemoveAllListeners();
        OnRightButtonPressed.RemoveAllListeners();
        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }
}
