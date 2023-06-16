using System;
using UnityEngine;
using UnityEngine.UI;

public class LanePlaceIdentifier : MonoBehaviour
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public LaneLocation Location { get; private set; }
    [field: SerializeField] public bool IsMine { get; private set; }

    LaneDisplay laneDisplay;
    Image image;

    private void Awake()
    {
        laneDisplay = GetComponentInParent<LaneDisplay>();
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        CardInteractions.DragStarted += CheckIfTileIsAvailable;
        CardInteractions.DragEnded += TurnOffAvailableColor;
        GameplayManagerPVP.OpponentCanceledCommand += CheckIfShouldDestroyChild;
    }

    private void OnDisable()
    {
        CardInteractions.DragStarted -= CheckIfTileIsAvailable;
        CardInteractions.DragEnded -= TurnOffAvailableColor;
        GameplayManagerPVP.OpponentCanceledCommand += CheckIfShouldDestroyChild;
    }

    void CheckIfTileIsAvailable(CardObject _cardObject)
    {
        if (_cardObject.CardLocation==CardLocation.Table)
        {
            return;
        }
        if (!IsMine)
        {
            return;
        }

        if (!CanPlace())
        {
            return;
        }

        if (!laneDisplay.CanPlace(_cardObject))
        {
            return;
        }

        if (GameplayManager.Instance.MyPlayer.Energy<_cardObject.Stats.Energy)
        {
            return;
        }

        Color _color = image.color;
        _color.a = 0.4f;
        image.color = _color;
    }

    void TurnOffAvailableColor()
    {
        Color _color = image.color;
        _color.a = 0f;
        image.color = _color;
    }

    void CheckIfShouldDestroyChild(PlaceCommand _command)
    {
        if (Id!=_command.PlaceId)
        {
            return;
        }

        if (transform.childCount==0)
        {
            Debug.Log("I dont have children");
            return;
        }

        CardObject _cardObject = GetComponentInChildren<CardObject>();
        if (_cardObject == null)
        {
            Debug.Log("I have children but they are not card");
            return;
        }

        if (_cardObject.Details.Id!=_command.Card.Details.Id)
        {
            Debug.Log("I have card as child but not desired one");
            return;
        }

        Destroy(_cardObject.gameObject);
    }

    public bool CanPlace()
    {
        foreach (Transform _child in transform)
        {
            CardObject _cardObject = _child.GetComponent<CardObject>();
            if (_cardObject != null)
            {
                return false;
            }
        }
        return true;
    }
}
