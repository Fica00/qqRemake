using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialImages : MonoBehaviour
{
    [SerializeField] private GameObject holder;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Image image;
    [SerializeField] private Button next;
    [SerializeField] private Button previous;

    private Action callback;
    private int index;

    private void OnEnable()
    {
        next.onClick.AddListener(ShowNext);
        previous.onClick.AddListener(ShowPrevious);
        SwipeInput.OnSwipedRight += ShowPrevious;
        SwipeInput.OnSwipedLeft += ShowNext;
    }

    private void OnDisable()
    {
        next.onClick.RemoveListener(ShowNext);
        previous.onClick.AddListener(ShowPrevious);
        SwipeInput.OnSwipedRight -= ShowPrevious;
        SwipeInput.OnSwipedLeft -= ShowNext;
    }

    private void ShowPrevious()
    {
        if (index > 0)
        {
            index--;
        }

        Show();
    }

    private void ShowNext()
    {
        if (index < sprites.Length - 1)
        {
            index++;
        }
        else
        {
            DataManager.Instance.PlayerData.HasFinishedTutorial = 1;
            holder.SetActive(false);
            callback?.Invoke();
        }

        Show();
    }

    private void Show()
    {
        image.sprite = sprites[index];
    }

    public void Setup(Action _callback)
    {
        callback = _callback;
        holder.SetActive(true);
        Show();
    }
}