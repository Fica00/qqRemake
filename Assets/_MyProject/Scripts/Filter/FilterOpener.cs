using UnityEngine;
using UnityEngine.UI;

public class FilterOpener : MonoBehaviour
{
    [SerializeField] private FilterHandler filterHandler;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(ShowFilter);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(ShowFilter);
    }

    private void ShowFilter()
    {
        filterHandler.Setup();
    }
}
