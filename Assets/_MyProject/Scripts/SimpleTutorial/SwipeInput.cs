using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class SwipeInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static Action OnSwipedLeft;
    public static Action OnSwipedRight;
    public static Action OnSwipedUp;
    public static Action OnSwipedDown;
    public static Action OnClicked;
    public static Action<int> OnZoom;  // This action will send zoom factor as a parameter

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private float initialTouchDistance; // To keep track of initial distance between two fingers
    private float swipeThreshold = 50f;

    public void OnPointerDown(PointerEventData _eventData)
    {
        if (Input.touchCount == 2) // Check for two finger touch
        {
            Touch _touch1 = Input.GetTouch(0);
            Touch _touch2 = Input.GetTouch(1);
            initialTouchDistance = Vector2.Distance(_touch1.position, _touch2.position);
        }
        else
        {
            touchStartPos = _eventData.position;
        }
    }

    public void OnPointerUp(PointerEventData _eventData)
    {
        if (Input.touchCount == 2)
        {
            DetectZoom();
        }
        else
        {
            touchEndPos = _eventData.position;
            DetectSwipe();
        }
    }

    void DetectZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch _touch1 = Input.GetTouch(0);
            Touch _touch2 = Input.GetTouch(1);
            float _currentTouchDistance = Vector2.Distance(_touch1.position, _touch2.position);

            if (_currentTouchDistance > initialTouchDistance)
            {
                OnZoom?.Invoke(1);
            }
            else if (_currentTouchDistance < initialTouchDistance)
            {
                OnZoom?.Invoke(-1);
            }
        }
    }

    void DetectSwipe()
    {
        float _swipeMagnitudeX = touchEndPos.x - touchStartPos.x;
        float _swipeMagnitudeY = touchEndPos.y - touchStartPos.y;

        if (Mathf.Abs(_swipeMagnitudeX) > swipeThreshold || Mathf.Abs(_swipeMagnitudeY) > swipeThreshold)
        {
            if (Mathf.Abs(_swipeMagnitudeX) > Mathf.Abs(_swipeMagnitudeY))
            {
                if (_swipeMagnitudeX > 0)
                    OnSwipeRight();
                else
                    OnSwipeLeft();
            }
            else
            {
                if (_swipeMagnitudeY > 0)
                    OnSwipeUp();
                else
                    OnSwipeDown();
            }
        }
        else
        {
            OnClick();
        }
    }

    void OnSwipeLeft()
    {
        OnSwipedLeft?.Invoke();
    }

    void OnSwipeRight()
    {
        OnSwipedRight?.Invoke();
    }

    void OnSwipeUp()
    {
        OnSwipedUp?.Invoke();
    }

    void OnSwipeDown()
    {
        OnSwipedDown?.Invoke();
    }

    void OnClick()
    {
        OnClicked?.Invoke();
    }
}