using System.Collections.Generic;
using UnityEngine;

public class LaneDisplay : MonoBehaviour
{
    [field: SerializeField] public LaneLocation Location { get; private set; }
    [field: SerializeField] public LocationAbilityDisplay AbilityDisplay { get; private set; }
    [field: SerializeField] public LaneVizualizator Visualizator { get; private set; }

    [HideInInspector] public LaneSpecifics LaneSpecifics = new LaneSpecifics();

    [SerializeField] PowerDisplay powerDisplay;
    [SerializeField] List<LanePlaceIdentifier> myPlaces;
    [SerializeField] List<LanePlaceIdentifier> opponentPlaces;


    private void OnEnable()
    {
        GameplayPlayer.AddedCardToTable += CheckIfCardShouldBePlacedOnThisLane;
        GameplayManagerPVP.OpponentAddedCommand += CheckIfCardShouldBePlacedOnThisLane;
        TableHandler.UpdatedPower += ShowPower;
    }

    private void OnDisable()
    {
        GameplayPlayer.AddedCardToTable -= CheckIfCardShouldBePlacedOnThisLane;
        GameplayManagerPVP.OpponentAddedCommand -= CheckIfCardShouldBePlacedOnThisLane;
        TableHandler.UpdatedPower -= ShowPower;
    }

    public void CheckIfCardShouldBePlacedOnThisLane(PlaceCommand _command)
    {
        LanePlaceIdentifier _place = null;
        bool _isMine = _command.IsMyPlayer;

        if (_command.Location != Location)
        {
            return;
        }

        if (_isMine)
        {
            _place = CheckForMathicngPlace(myPlaces, _command.PlaceId);
        }
        else
        {
            _place = CheckForMathicngPlace(opponentPlaces, _command.PlaceId);
        }

        if (_place == null)
        {
            return;
        }

        _command.Card.transform.SetParent(_place.transform);
        _command.Card.transform.localPosition = Vector3.zero;
    }

    void ShowPower(LaneLocation _location)
    {
        if (_location != Location)
        {
            return;
        }

        int _myPower = GameplayManager.Instance.TableHandler.GetPower(true, Location);
        int _opponentPower = GameplayManager.Instance.TableHandler.GetPower(false, Location);
        powerDisplay.ShowPower(_myPower, _opponentPower);
    }

    LanePlaceIdentifier CheckForMathicngPlace(List<LanePlaceIdentifier> _places, int _id)
    {
        foreach (var _place in _places)
        {
            if (_place.Id == _id)
            {
                return _place;
            }
        }

        return null;
    }

    public LanePlaceIdentifier GetPlaceLocation(bool _isMyPlayer)
    {
        List<LanePlaceIdentifier> _lanePlaces = _isMyPlayer ? myPlaces : opponentPlaces;
        foreach (var _lane in _lanePlaces)
        {
            if (_lane.CanPlace())
            {
                return _lane;
            }
        }

        return null;
    }

    public bool CanPlace(CardObject _cardObject)
    {
        if (LaneSpecifics.CantPlaceCommonsThatCost.Contains(_cardObject.Stats.Energy))
        {
            return false;
        }
        if (LaneSpecifics.CantPlaceCommonsOnRound.Contains(GameplayManager.Instance.CurrentRound))
        {
            return false;
        }
        return true;
    }
}
