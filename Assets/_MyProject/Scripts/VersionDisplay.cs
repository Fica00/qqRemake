using TMPro;
using UnityEngine;

public class VersionDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI display;
    private void OnEnable()
    {
        display.text = JavaScriptManager.Instance.Version;
    }
}
