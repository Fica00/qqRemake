using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInteractions : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static Action<CardObject> OnClicked;
    public static Action<CardObject> DragStarted;
    public static Action DragEnded;

    protected CardObject cardObject;
    CardDisplay cardDisplay;
    RectTransform cardRectTransform;
    bool isDragging = false;
    Vector2 pointerDownPosition;
    const float dragThreshold = 5f;
    bool canChangePlace;

    public void Setup(CardObject _cardObject)
    {
        cardObject = _cardObject;
        cardDisplay = cardObject.Display;
        cardRectTransform = cardDisplay.GetComponent<RectTransform>();
    }

    public void CancelDrag()
    {
        Debug.Log("Cancel drag detected");
        if (isDragging)
        {
            Debug.Log("Card is being draged");
            EndDrag();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        HandlePointerDown(eventData);
    }

    protected virtual void HandlePointerDown(PointerEventData _eventData)
    {
        isDragging = false;
        canChangePlace = cardObject.CanChangePlace;
        pointerDownPosition = _eventData.position;
    }

    public void OnPointerUp(PointerEventData _eventData)
    {
        HandlePointerUp(_eventData);
    }

    protected virtual void HandlePointerUp(PointerEventData eventData)
    {
        if (cardObject.CardLocation == CardLocation.Table)
        {
            if (canChangePlace)
            {
                GameplayManager.Instance.MyPlayer.CancelCommand(cardObject);
            }
            else
            {
                OnClicked?.Invoke(cardObject);
            }
            return;
        }
        else
        {
            if (!isDragging)
            {
                OnClicked?.Invoke(cardObject);
            }
            else
            {
                HandleDragEnded(eventData);
            }
        }
    }

    void HandleDragEnded(PointerEventData _eventData)
    {
        EndDrag();

        List<RaycastResult> _results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventData, _results);

        foreach (RaycastResult _result in _results)
        {
            LanePlaceIdentifier placeIdentifier = _result.gameObject.GetComponent<LanePlaceIdentifier>();
            if (placeIdentifier != null)
            {
                if (!placeIdentifier.IsMine)
                {
                    continue;
                }
                cardObject.TryToPlace(placeIdentifier);
                return;
            }
        }
    }

    void EndDrag()
    {
        Debug.Log("Ending drag");
        DragEnded?.Invoke();
        cardDisplay.transform.localPosition = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        HandleDrag(eventData);
    }

    protected virtual void HandleDrag(PointerEventData eventData)
    {
        if (cardObject.CardLocation == CardLocation.Table)
        {
            return;
        }
        else
        {
            if (!isDragging)
            {
                Vector2 dragDelta = eventData.position - pointerDownPosition;
                if (dragDelta.magnitude >= dragThreshold)
                {
                    isDragging = true;
                    DragStarted?.Invoke(cardObject);
                    // Additional actions when the drag gesture is confirmed
                }
            }

            if (isDragging)
            {
                Vector2 dragDelta = Camera.main.ScreenToWorldPoint(eventData.delta) - Camera.main.ScreenToWorldPoint(Vector2.zero);
                cardDisplay.transform.position += (Vector3)dragDelta;
            }
        }
    }
}
