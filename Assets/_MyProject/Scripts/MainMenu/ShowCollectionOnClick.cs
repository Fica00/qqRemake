using UnityEngine;
using UnityEngine.UI;

public class ShowCollectionOnClick : MonoBehaviour
{
    [SerializeField] private Button collectionButton;

    private void OnEnable()
    {
        collectionButton.onClick.AddListener(ShowCollection);   
    }

    private void OnDisable()
    {
        collectionButton.onClick.RemoveListener(ShowCollection);
    }

    private void ShowCollection()
    {
        BotHudHandler.Instance.ShowCollection();
    }
}
