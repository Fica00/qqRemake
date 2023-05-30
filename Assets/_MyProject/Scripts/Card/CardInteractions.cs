using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInteractions : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static Action<CardObject> OnClicked;

    protected CardObject cardObject;
    CardDisplay cardDisplay;
    bool isDragging = false;
    Vector2 pointerDownPosition;
    const float dragThreshold = 5f;
    bool canChangePlace;

    public void Setup(CardObject _cardObject)
    {
        cardObject = _cardObject;
        cardDisplay = cardObject.Display;
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
        cardDisplay.transform.localPosition = Vector3.zero;

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
                    // Additional actions when the drag gesture is confirmed
                }
            }

            if (isDragging)
            {
                Vector2 dragDelta = eventData.delta;
                cardDisplay.transform.position += (Vector3)dragDelta;
            }
        }
    }
}
