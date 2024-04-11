using UnityEngine;
using UnityEngine.UI;

public class BackgroundMusicHandler : MonoBehaviour
{
    [SerializeField] private Sprite on;
    [SerializeField] private Sprite off;

    [SerializeField] private Image image;
    [SerializeField] private Button button;

    private void OnEnable()
    {
        button.onClick.AddListener(Toggle);
        Show();
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(Toggle);
    }

    private void Toggle()
    {
        DataManager.Instance.PlayerData.PlayBackgroundMusic = !DataManager.Instance.PlayerData.PlayBackgroundMusic;
        Show();
    }

    private void Show()
    {
        image.sprite = DataManager.Instance.PlayerData.PlayBackgroundMusic ? on : off;
    }
}
