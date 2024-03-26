using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image background;
    private TextMeshProUGUI text;

    private Color startingBackgroundColor;
    private Color startingTextColor;

    private void Awake()
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();

        startingBackgroundColor = background.color;
        startingTextColor = text.color;
    }

    public void OnPointerEnter(PointerEventData _)
    {
        background.color = new Color(1, 1f,0.87f,0.8f);;
        text.color = new Color(0.92f,0.65f,0.41f,1);
    }

    public void OnPointerExit(PointerEventData _)
    {
        background.color = startingBackgroundColor;
        text.color = startingTextColor;
    }
}
