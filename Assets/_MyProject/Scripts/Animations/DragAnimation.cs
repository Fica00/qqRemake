using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DragAnimation : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        image.fillAmount = 0;
        image.DOFillAmount(1, 2).OnComplete(Start);
    }

}
