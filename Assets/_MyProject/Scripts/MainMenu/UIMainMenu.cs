using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] Button playButton;

    [Space()]
    [SerializeField] UIPlayPanel playPanel;

    private void OnEnable()
    {
        playButton.onClick.AddListener(ShowPlayPanel);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(ShowPlayPanel);
    }

    void ShowPlayPanel()
    {
        playPanel.Setup();
    }
}
