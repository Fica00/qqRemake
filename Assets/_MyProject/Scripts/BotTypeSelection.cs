using TMPro;
using UnityEngine;

public class BotTypeSelection : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    public static BotType BotType = BotType.Version3;

    private void OnEnable()
    {
        dropdown.onValueChanged.AddListener(SelectBot);
        dropdown.value = (int)BotType;
    }

    private void OnDisable()
    {
        dropdown.onValueChanged.RemoveListener(SelectBot);
    }

    private void SelectBot(int _value)
    {
        BotType = (BotType)_value;
    }
}
