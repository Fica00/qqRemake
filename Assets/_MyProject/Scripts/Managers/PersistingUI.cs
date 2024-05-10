using System.Collections;
using SceneManagement;
using UnityEngine;

public class PersistingUI : MonoBehaviour
{
    public static PersistingUI Instance;
    private Canvas canvas;

    private void Awake()
    {
        if (Instance==null)
        {
            canvas = GetComponent<Canvas>();
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneLoader.OnLoadedScene += RefreshCanvas;
    }

    private void OnDisable()
    {
        SceneLoader.OnLoadedScene -= RefreshCanvas;
    }

    private void RefreshCanvas()
    {
        StartCoroutine(RefreshRoutine());
        IEnumerator RefreshRoutine()
        {
            yield return null;
            UpdateCanvasOrder();
        }
    }
    
    public void UpdateCanvasOrder()
    {
        if (canvas.sortingOrder == 100)
        {
            canvas.sortingOrder--;
        }
        else
        {
            canvas.sortingOrder++;
        }
    }
}
