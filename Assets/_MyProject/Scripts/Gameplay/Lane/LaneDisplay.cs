using System;
using System.Collections.Generic;
using UnityEngine;

public class LaneDisplay : MonoBehaviour
{
    [field: SerializeField] public LaneLocation Location { get; private set; }
    [field: SerializeField] public LocationAbilityDisplay AbilityDisplay { get; private set; }
    [field: SerializeField] public LaneVizualizator Visualizator { get; private set; }

    [HideInInspector] public LaneSpecifics LaneSpecifics = new LaneSpecifics();

    [SerializeField] private PowerDisplay powerDisplay;
    [SerializeField] private List<LanePlaceIdentifier> myPlaces;
    [SerializeField] private List<LanePlaceIdentifier> opponentPlaces;

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

    private void ShowPower(LaneLocation _location)
    {
        if (_location != Location)
        {
            return;
        }

        int _myPower = GameplayManager.Instance.TableHandler.GetPower(true, Location);
        int _opponentPower = GameplayManager.Instance.TableHandler.GetPower(false, Location);
        powerDisplay.ShowPower(_myPower, _opponentPower);
    }

    private LanePlaceIdentifier CheckForMathicngPlace(List<LanePlaceIdentifier> _places, int _id)
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
        if (LaneSpecifics.MaxAmountOfQommons <= AmountOfQommonsHere(_cardObject.IsMy))
        {
            return false;
        }
        return true;
    }

    public bool CanRemoveCards()
    {
        return LaneSpecifics.CanRemoveCards;
    }

    private int AmountOfQommonsHere(bool _isMyPlayer)
    {
        int _amount = 0;
        List<LanePlaceIdentifier> _lanePlaces = _isMyPlayer ? myPlaces : opponentPlaces;

        foreach (var _lane in _lanePlaces)
        {
            if (_lane.CanPlace())
            {
                continue;
            }

            _amount++;
        }

        return _amount;
    }

    public void AbilityShowAsActive()
    {
        AbilityDisplay.AbilityShowAsActive();
    }

    public void AbilityShowAsInactive()
    {
        AbilityDisplay.AbilityShowAsInactive();
    }

    public void AbilityFlash()
    {
        AbilityDisplay.AbilityFlash();
    }

    public void ShowWinner(Action _callBack)
    {
        int _myPower = GameplayManager.Instance.TableHandler.GetPower(true, Location);
        int _opponentPower = GameplayManager.Instance.TableHandler.GetPower(false, Location);
        powerDisplay.ShowWinner(_myPower,_opponentPower,_callBack);
    }

    public void ShowEnlargedPowerAnimation(bool _showMyPower)
    {
        powerDisplay.EnlargedPowerAnimation(_showMyPower);
    }
}
