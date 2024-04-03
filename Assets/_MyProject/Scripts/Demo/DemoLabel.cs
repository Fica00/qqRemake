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
            _textToDisplay += "Coming soon ";
        }

        display.text = _textToDisplay;
    }
}
