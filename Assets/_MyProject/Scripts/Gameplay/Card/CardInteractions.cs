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
    private CardDisplay cardDisplay;
    private bool isDragging = false;
    private Vector2 pointerDownPosition;
    private const float dragThreshold = 5f;
    private bool canChangePlace;
    [HideInInspector] public bool CanDrag;

    public void Setup(CardObject _cardObject)
    {
        cardObject = _cardObject;
        cardDisplay = cardObject.Display;
    }

    public void CancelDrag()
    {
        if (isDragging)
        {
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

    private void HandlePointerUp(PointerEventData eventData)
    {
        if (cardObject.CardLocation == CardLocation.Table)
        {
            if (isDragging && canChangePlace)
            {
                GameplayManager.Instance.MyPlayer.CancelCommand(cardObject);
                HandleDragEnded(eventData);
            }
            else
            {
                ShowDetails();
            }
            return;
        }
        else
        {
            if (!isDragging)
            {
                ShowDetails();
            }
            else
            {
                HandleDragEnded(eventData);
            }
        }
    }

    private void ShowDetails()
    {
        if (cardObject.Reveal.IsRevealing)
        {
            return;
        }

        OnClicked?.Invoke(cardObject);
    }

    private void HandleDragEnded(PointerEventData _eventData)
    {
        EndDrag();

        List<RaycastResult> _results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventData, _results);

        foreach (RaycastResult _result in _results)
        {
            if (_result.gameObject.name == "MyCommonPlaces")
            {
                AudioManager.Instance.PlaySoundEffect(AudioManager.CARD_SOUND);
                cardObject.TryToPlace(_result.gameObject.GetComponentInChildren<LanePlaceIdentifier>());
                return;
            }
        }
    }

    private void EndDrag()
    {
        DragEnded?.Invoke();
        cardDisplay.transform.localPosition = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        HandleDrag(eventData);
    }

    protected virtual void HandleDrag(PointerEventData eventData)
    {
        if (!CanDrag)
        {
            return;
        }

        if (!cardObject.IsMy)
        {
            return;
        }

        if (!isDragging)
        {
            Vector2 dragDelta = eventData.position - pointerDownPosition;
            if (dragDelta.magnitude >= dragThreshold)
            {
                isDragging = true;
                AudioManager.Instance.PlaySoundEffect(AudioManager.CARD_SOUND);
                DragStarted?.Invoke(cardObject);
                if (cardObject.CardLocation==CardLocation.Table)
                {
                    cardObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainHolder").transform);
                }
            }
        }

        if (isDragging)
        {
            Vector2 dragDelta = Camera.main.ScreenToWorldPoint(eventData.delta) - Camera.main.ScreenToWorldPoint(Vector2.zero);
            cardDisplay.transform.position += (Vector3)dragDelta;
        }
    }
}
