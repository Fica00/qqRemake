using UnityEngine;
using UnityEngine.UI;

public class SimpleTutorial : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Image image;
    [SerializeField] private Button close;
    
    private int index;
    
    private void OnEnable()
    {
        close.onClick.AddListener(Close);
        SwipeInput.OnSwipedRight += ShowPrevious;
        SwipeInput.OnSwipedLeft += ShowNext;
    }

    private void OnDisable()
    {
        close.onClick.RemoveListener(Close);
        SwipeInput.OnSwipedRight -= ShowPrevious;
        SwipeInput.OnSwipedLeft -= ShowNext;
    }

    private void Close()
    {
        if (index!=4)
        {
            return;
        }

        DataManager.Instance.PlayerData.HasFinishedTutorial = 1;
        SceneManager.Instance.LoadMainMenu();
    }

    private void ShowPrevious()
    {
        if (index>0)
        {
            index--;
        }
        
        Show();
    }

    private void ShowNext()
    {
        if (index<sprites.Length-1)
        {
            index++;
        }
        
        Show();
    }

    private void Start()
    {
        Show();
    }

    private void Show()
    {
        image.sprite = sprites[index];
    }
}
