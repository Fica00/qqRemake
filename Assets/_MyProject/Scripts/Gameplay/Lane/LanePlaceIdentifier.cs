using System;
using UnityEngine;
using UnityEngine.UI;

public class LanePlaceIdentifier : MonoBehaviour
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public LaneLocation Location { get; private set; }
    [field: SerializeField] public bool IsMine { get; private set; }

    private LaneDisplay laneDisplay;
    private Image image;

    private void Awake()
    {
        laneDisplay = GetComponentInParent<LaneDisplay>();
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        GameplayManagerPvp.OpponentCanceledCommand += CheckIfShouldDestroyChild;
    }

    private void OnDisable()
    {
        GameplayManagerPvp.OpponentCanceledCommand += CheckIfShouldDestroyChild;
    }

    public bool CheckIfTileIsAvailable(CardObject _cardObject)
    {
        if (!IsMine)
        {
            return false;
        }

        if (!CanPlace())
        {
            return false;
        }

        if (!laneDisplay.CanPlace(_cardObject))
        {
            return false;
        }

        if (GameplayManager.Instance.MyPlayer.Energy<_cardObject.Stats.Energy)
        {
            return false;
        }

        return true;
    }

    private void CheckIfShouldDestroyChild(PlaceCommand _command)
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
