using UnityEngine;
using UnityEngine.UI;

public class SoundEffectsHandler : MonoBehaviour
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
        DataManager.Instance.PlayerData.PlaySoundEffects = !DataManager.Instance.PlayerData.PlaySoundEffects;
        Show();
    }

    private void Show()
    {
        image.sprite = DataManager.Instance.PlayerData.PlaySoundEffects ? on : off;
    }
}
