using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInputInteractions : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static Action<CardObject> OnClicked;

    CardObject cardObject;
    CardDisplay cardDisplay;
    bool isDragging = false;
    Vector2 pointerDownPosition;
    const float dragThreshold = 5f;

    public void Setup(CardObject _cardObject)
    {
        cardObject = _cardObject;
        cardDisplay = cardObject.Display;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = false;
        pointerDownPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragging)
        {
            OnClicked?.Invoke(cardObject);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging)
        {
            Vector2 dragDelta = eventData.position - pointerDownPosition;
            if (dragDelta.magnitude >= dragThreshold)
            {
                isDragging = true;
                // Additional actions when the drag gesture is confirmed
            }
        }

        if (isDragging)
        {
            // Additional actions while the Image is being dragged
            Vector2 dragDelta = eventData.delta;
            cardDisplay.transform.position += (Vector3)dragDelta;
        }
    }
}
