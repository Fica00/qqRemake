using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardObject : MonoBehaviour
{
    [field: SerializeField] public CardDetails Details { get; private set; }
    [field: SerializeField] public CardDisplay Display { get; private set; }
    [field: SerializeField] public CardReveal Reveal { get; private set; }
    [field: SerializeField] public List<CardEffectBase> SpecialEffects { get; private set; }

    public CardStats Stats { get; private set; }

    [HideInInspector] public bool IsMy;

    [HideInInspector] public bool CanChangePlace = true; // used to determin if card can move from table back to hand

    private CardInteractions cardInputInteractions;

    public CardLocation CardLocation { get; private set; }

    [HideInInspector] public bool IsForcedToBePlaced = false;

    public void Setup(bool _isMy)
    {
        Stats = new CardStats()
        {
            Power = Details.Power,
            Energy = Details.Mana
        };
        IsMy = _isMy;
        if (IsMy)
        {
            cardInputInteractions = gameObject.AddComponent<CardInteractions>();
        }
        else
        {
            cardInputInteractions = gameObject.AddComponent<CardOpponentInteractions>();
        }

        foreach (var _specialEffect in SpecialEffects)
        {
            _specialEffect.Setup(this);
        }

        cardInputInteractions.Setup(this);
        Reveal.Setup(this);
        Display.Setup(this);
        ManageBeheviour();

        GameplayManager.UpdatedGameState += ManageBeheviour;
    }

    private void OnDisable()
    {
        GameplayManager.UpdatedGameState -= ManageBeheviour;
    }

    private void ManageBeheviour()
    {
        switch (GameplayManager.Instance.GameplayState)
        {
            case GameplayState.ResolvingBeginingOfRound:
                cardInputInteractions.CanDrag = false;
                break;
            case GameplayState.Playing:
                if (CardLocation == CardLocation.Table)
                {
                    if (CanChangePlace && !IsForcedToBePlaced)
                    {
                        CancelReveal();
                    }
                }
                else
                {
                    cardInputInteractions.CanDrag = true;
                }
                break;
            case GameplayState.Waiting:
                cardInputInteractions.CancelDrag();
                cardInputInteractions.CanDrag = false;
                if (CardLocation == CardLocation.Table && CanChangePlace)
                {
                    PrepareForReveal();
                }
                break;
            case GameplayState.ResolvingEndOfRound:
                cardInputInteractions.CancelDrag();
                cardInputInteractions.CanDrag = false;
                break;
            default:
                throw new Exception("Dont know how to handle state: " + GameplayManager.Instance.GameplayState);
        }
    }

    public void SetCardLocation(CardLocation _newLocation)
    {
        CardLocation = _newLocation;
        switch (CardLocation)
        {
            case CardLocation.Hand:
                Display.ShowCardInHand();
                break;
            case CardLocation.Table:
                Display.ShowCardOnTable();
                break;
            case CardLocation.Discarded:
                break;
            default:
                throw new Exception("Dont know how to handle location: " + _newLocation);
        }
    }

    public bool TryToPlace(LanePlaceIdentifier _placeIdentifier)
    {
        GameplayPlayer _player = IsMy ? GameplayManager.Instance.MyPlayer : GameplayManager.Instance.OpponentPlayer;
        if (_player.Energy < Stats.Energy)
        {
            return false;
        }
        LaneDisplay _laneDisplay = _placeIdentifier.gameObject.GetComponentInParent<LaneDisplay>();
        _placeIdentifier = _laneDisplay.GetPlaceLocation(IsMy);

        if (_placeIdentifier == null)
        {
            return false;
        }

        if (!_laneDisplay.CanPlace(this))
        {
            return false;
        }

        PlaceCommand _command = new PlaceCommand()
        {
            PlaceId = _placeIdentifier.Id,
            Card = this,
            IsMyPlayer = IsMy,
            Location = _placeIdentifier.Location
        };

        _player.Energy -= Stats.Energy;

        _player.RemoveCardFromHand(this);
        _player.AddCardToTable(_command);
        return true;
    }

    public void ForcePlace(LaneDisplay _laneDisplay)
    {
        GameplayPlayer _player = IsMy ? GameplayManager.Instance.MyPlayer : GameplayManager.Instance.OpponentPlayer;
        LanePlaceIdentifier _placeIdentifier = _laneDisplay.GetPlaceLocation(IsMy);

        if (!_laneDisplay.CanPlace(this))
        {
            return;
        }

        PlaceCommand _command = new PlaceCommand()
        {
            PlaceId = _placeIdentifier.Id,
            Card = this,
            IsMyPlayer = IsMy,
            Location = _placeIdentifier.Location
        };

        IsForcedToBePlaced = true;
        _player.RemoveCardFromHand(this);
        _player.AddCardToTable(_command);
        PrepareForReveal();
        if (GameplayManager.IsPvpGame)
        {
            ((GameplayManagerPVP.Instance)as GameplayManagerPVP).ForcePlace(_command);
        }
    }

    public void PrepareForReveal()
    {
        Reveal.PrepareForReveal();
        Display.HideCardOnTable();
    }

    public void CancelReveal()
    {
        Reveal.CancelReveal();
        Display.ShowCardOnTable();
    }

    public IEnumerator RevealCard()
    {
        yield return StartCoroutine(Reveal.Reveal());
        foreach (var _specialEffect in SpecialEffects)
        {
            if (_specialEffect != null)
            {
                _specialEffect.Subscribe();
            }
        }
    }

    public LaneLocation LaneLocation
    {
        get
        {
            LaneDisplay _laneDisplay = GetComponentInParent<LaneDisplay>();
            if (_laneDisplay == null)
            {
                throw new Exception("Cant find lane display for to determin lain location");
            }
            return _laneDisplay.Location;
        }
    }
}
