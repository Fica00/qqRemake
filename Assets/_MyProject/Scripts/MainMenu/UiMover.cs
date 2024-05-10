using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiMover : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Button button; 


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void OnDrag(PointerEventData _eventData)
    {
        rectTransform.position = _eventData.position;
        button.interactable = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        button.interactable = true;
    }



}