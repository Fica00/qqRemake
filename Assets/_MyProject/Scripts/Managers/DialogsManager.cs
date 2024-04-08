using UnityEngine;

public class DialogsManager : MonoBehaviour
{
    public static DialogsManager Instance;
    [field: SerializeField] public OkDialog OkDialog { get; private set; }
    [field: SerializeField] public YesNoDialog YesNoDialog { get; private set; }

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateCanvasOrder()
    {
        Canvas _canvas = GetComponent<Canvas>();
        if (_canvas.sortingOrder == 100)
        {
            _canvas.sortingOrder--;
        }
        else
        {
            _canvas.sortingOrder++;
        }
    }
}
