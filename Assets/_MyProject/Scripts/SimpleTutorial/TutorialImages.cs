using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialImages : MonoBehaviour
{
    [SerializeField] private GameObject holder;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Image image;

    protected Action Callback;
    private int index;

    private void OnEnable()
    {
        SwipeInput.OnClicked += ShowNext;
        SwipeInput.OnSwipedLeft += ShowNext;
    }

    private void OnDisable()
    {
        SwipeInput.OnClicked -= ShowNext;
        SwipeInput.OnSwipedLeft -= ShowNext;
    }

    private void ShowNext()
    {
        if (index < sprites.Length - 1)
        {
            index++;
        }
        else
        {
            Close();
        }

        Show();
    }

    protected virtual void Close()
    {
        DataManager.Instance.PlayerData.HasFinishedTutorial = 1;
        holder.SetActive(false);
        Callback?.Invoke();
    }

    public virtual void Setup(Action _callback)
    {
        Callback = _callback;
        holder.SetActive(true);
        Show();
    }
    
    protected void Show()
    {
        image.sprite = sprites[index];
    }
}