using TMPro;
using UnityEngine;

public class DemoLabel : MonoBehaviour
{
    [SerializeField] private string text;
    private TextMeshProUGUI display;

    private void Awake()
    {
        display = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        string _textToDisplay = string.Empty;
        _textToDisplay += text;
        if (JavaScriptManager.Instance.IsDemo)
        {
            if (!string.IsNullOrEmpty(text))
            {
                _textToDisplay += "\n";
            }
            _textToDisplay += "Coming soon";
        }
        else if (string.IsNullOrEmpty(_textToDisplay))
        {
            return;
        }

        display.text = _textToDisplay;
    }
}
