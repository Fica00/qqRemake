using System.Collections.Generic;
using UnityEngine;

public class LaneDisplay : MonoBehaviour
{
    [field: SerializeField] public LaneLocation Location { get; private set; }
    [SerializeField] PowerDisplay powerDisplay;
    [field: SerializeField] public LocationAbilityDisplay AbilityDisplay { get; private set; }

    [SerializeField] List<LanePlaceIdentifier> MyPlaces;
    [SerializeField] List<LanePlaceIdentifier> OpponentPlaces;

    private void OnEnable()
    {
        GameplayPlayer.AddedCardToTable += TryToPlaceCard;
        TableHandler.UpdatedPower += ShowPower;
    }

    private void OnDisable()
    {
        GameplayPlayer.AddedCardToTable -= TryToPlaceCard;
        TableHandler.UpdatedPower -= ShowPower;
    }

    private void TryToPlaceCard(PlaceCommand _command)
    {
        LanePlaceIdentifier _place = null;
        bool _isMine = _command.Player.IsMy;

        if (_command.Location != Location)
        {
            return;
        }

        if (_isMine)
        {
            _place = CheckForMathicngPlace(MyPlaces, _command.PlaceId);
        }
        else
        {
            _place = CheckForMathicngPlace(OpponentPlaces, _command.PlaceId);
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
}
