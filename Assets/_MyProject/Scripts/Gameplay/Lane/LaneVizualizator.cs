using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LaneVizualizator : MonoBehaviour
{
    [SerializeField] private LaneLocation location;
    [Space()]
    [SerializeField]
    protected List<LanePlaceIdentifier> myPlaces;
    [SerializeField] protected Image myLane;
    [SerializeField] protected GameObject laneIndicator;
    [Space()]
    [SerializeField]
    private List<LanePlaceIdentifier> opponentPlaces;
    [SerializeField] private Image opponentLane;

    [SerializeField] private Image wholeLanePurple;
    [SerializeField] private Image wholeLaneYellow;
    [SerializeField] private Image wholeLaneBlue;
    [SerializeField] private Image myLaneDotted;
    [SerializeField] private Image opponentLaneDotted;

    [SerializeField] private LaneDisplay laneDisplay;
    [SerializeField] private GameObject tornadoAnimation;
    [SerializeField] private GameObject smokeAnimation;

    private void OnEnable()
    {
        GameplayManager.OnFlashPlace += FlashSpot;
        GameplayManager.OnFlashWholePlace += FlashWholePlace;
        GameplayManager.OnHighlihtWholePlace += HighlihtWholePlace;
        GameplayManager.OnHighlihtWholePlaceDotted += HighlihtWholePlaceDotted;
        GameplayManager.OnHideHighlightWholePlace += HideHighlihtWholePlace;
        GameplayManager.OnHideHighlightWholePlaceDotted += HideHighlihtWholePlaceDotted;
        GameplayManager.OnFlashAllSpotsOnLocation += FlashAllSpotsOnLocation;
        CardInteractions.DragStarted += CheckIfLaneIsAvailable;
        CardInteractions.DragEnded += TurnOffAvailableColor;
        LaneAbilityBase.OnActivated += CheckForEffect;
    }


    private void OnDisable()
    {
        GameplayManager.OnFlashPlace -= FlashSpot;
        GameplayManager.OnFlashWholePlace -= FlashWholePlace;
        GameplayManager.OnHighlihtWholePlace -= HighlihtWholePlace;
        GameplayManager.OnHighlihtWholePlaceDotted -= HighlihtWholePlaceDotted;
        GameplayManager.OnHideHighlightWholePlace -= HideHighlihtWholePlace;
        GameplayManager.OnHideHighlightWholePlaceDotted -= HideHighlihtWholePlaceDotted;
        GameplayManager.OnFlashAllSpotsOnLocation -= FlashAllSpotsOnLocation;
        CardInteractions.DragStarted -= CheckIfLaneIsAvailable;
        CardInteractions.DragEnded -= TurnOffAvailableColor;
        LaneAbilityBase.OnActivated -= CheckForEffect;
    }

    private void FlashSpot(int _locatoinId, Color _color, int _amount)
    {
        Image _placeImage = GetPlaceImage(_locatoinId);

        if (_placeImage == null)
        {
            return;
        }

        FlashImage(_placeImage, _color, _amount);
    }

    private void FlashWholePlace(LaneLocation _location, bool _mySide, Color _color, int _amount)
    {
        if (_location != location)
        {
            return;
        }

        Image _laneImage = _mySide ? myLane : opponentLane;
        FlashImage(_laneImage, _color, _amount);
    }

    private void FlashImage(Image _image, Color _color, int _amountOfFlashes)
    {
        Color _startingColor = _image.color;
        if (_color == new Color(0, 0, 0, 0))
        {
            _color = _image.color;
        }
        _color.a = 0;
        _image.color = _color;
        float _duration = 0.3f;

        Sequence _sequence = DOTween.Sequence();

        for (int i = 0; i < _amountOfFlashes; i++)
        {
            _sequence.Append(DOTween.To(() => _color.a, x => _color.a = x, 1, _duration).OnUpdate(() => { _image.color = _color; }));
            _sequence.Append(DOTween.To(() => _color.a, x => _color.a = x, 0, _duration).OnUpdate(() => { _image.color = _color; }));
        }
        _sequence.OnComplete(()=> _image.color = _startingColor);
        _sequence.Play();
    }

    private void HighlihtWholePlace(LaneLocation _location, bool _mySide, Color _color)
    {
        if (_location != location)
        {
            return;
        }

        Image _laneImage = _mySide ? myLane : opponentLane;
        ShowWholeLane(_laneImage, _color);
    }
    
    private void HighlihtWholePlaceDotted(LaneLocation _location, bool _mySide)
    {
        if (_location != location)
        {
            return;
        }

        Image _image = _mySide ? myLaneDotted : opponentLaneDotted;
        Color _color = _image.color;
        float _animationTime = 0.5f;
        DOTween.To(() => _color.a, x => _color.a = x, 1, _animationTime)
            .OnUpdate(() => 
            {
                _image.color = _color;
            });
    }

    private void HideHighlihtWholePlace(LaneLocation _location, bool _mySide, Color _color)
    {
        if (_location != location)
        {
            return;
        }

        Image _laneImage = _mySide ? myLane : opponentLane;

        if (_laneImage.color == _color)
        {
            float _animationTime = 0.3f;
            Sequence _sequence = DOTween.Sequence();
            _sequence.Append(DOTween.To(() => _color.a, x => _color.a = x, 0, _animationTime).OnUpdate(() => { _laneImage.color = _color; }));
            _sequence.Play();
        }
    }
    
    private void HideHighlihtWholePlaceDotted(LaneLocation _location, bool _mySide)
    {
        if (_location != location)
        {
            return;
        }

        Image _image = _mySide ? myLaneDotted : opponentLaneDotted;
        Color _color = _image.color;
        float _animationTime = 0.5f;
        DOTween.To(() => _color.a, x => _color.a = x, 0, _animationTime)
            .OnUpdate(() => 
            {
                _image.color = _color;
            });
    }

    private void FlashAllSpotsOnLocation(LaneLocation _location, bool _mySide, Color _color, int _amount)
    {
        if (_location != location)
        {
            return;
        }

        List<LanePlaceIdentifier> _places = _mySide ? myPlaces : opponentPlaces;
        foreach (var _place in _places)
        {
            FlashImage(_place.GetComponent<Image>(), _color, _amount);
        }
    }

    private void ShowWholeLane(Image _image, Color _color)
    {
        if (_color == new Color(0, 0, 0, 0))
        {
            _color = _image.color;
        }
        _color.a = 0;
        _image.color = _color;

        float _animationTime = 0.3f;
        Sequence _sequence = DOTween.Sequence();
        _sequence.Append(DOTween.To(() => _color.a, x => _color.a = x, 1, _animationTime).OnUpdate(() => { _image.color = _color; }));
        _sequence.Play();
    }

    private Image GetPlaceImage(int _locationId)
    {
        Image _image = GetImage(myPlaces);
        if (_image == null)
        {
            _image = GetImage(opponentPlaces);
        }

        return _image;

        Image GetImage(List<LanePlaceIdentifier> _places)
        {
            foreach (var _place in _places)
            {
                if (_place.Id == _locationId)
                {
                    return _place.GetComponent<Image>();
                }
            }
            return null;
        }
    }

    public void ShowWholeLanePurple()
    {
        Color _color = wholeLanePurple.color;
        float _animationTime=0.5f;
        DOTween.To(() => _color.a, x => _color.a = x, 1, _animationTime)
            .OnUpdate(() => { wholeLanePurple.color = _color; });
    }
    
    public void ShowWholeLaneYellow()
    {
        Color _color = wholeLaneYellow.color;
        float _animationTime=0.5f;
        DOTween.To(() => _color.a, x => _color.a = x, 1, _animationTime)
            .OnUpdate(() => { wholeLaneYellow.color = _color; });
    }
    
    public void ShowWholeLaneBlue()
    {
        Color _color = wholeLaneBlue.color;
        float _animationTime=0.5f;
        DOTween.To(() => _color.a, x => _color.a = x, 1, _animationTime)
            .OnUpdate(() => { wholeLaneBlue.color = _color; });
    }

    protected virtual void CheckIfLaneIsAvailable(CardObject _card)
    {
        foreach (var _myPlace in myPlaces)
        {
            if (_myPlace.CheckIfTileIsAvailable(_card))
            {
                laneIndicator.SetActive(true);
                return;
            }
        }
        
        laneIndicator.SetActive(false);
    }
    
    private void TurnOffAvailableColor()
    {
        laneIndicator.SetActive(false);
    }
    
    private void CheckForEffect(LaneDisplay _lane)
    {
        if (_lane != laneDisplay)
        {
            return;
        }
        
        HandleAnimationObject(true, LaneVizualizatorTrigger.Activation);
    }

    public void HandleAnimationObject(bool _status, LaneVizualizatorTrigger _tiger = LaneVizualizatorTrigger.None)
    {
        GameObject _animationObject = GetAnimationObject();
        if (_animationObject==null)
        {
            return;
        }
        
        _animationObject.SetActive(_status);
        
        GameObject GetAnimationObject()
        {
            if (!GameplayManager.Instance.LaneAbilities.ContainsKey(laneDisplay))
            {
                return null;
            }

            LaneAbility _lane = GameplayManager.Instance.LaneAbilities[laneDisplay];
            switch (_lane.Id)
            {
                case 21:
                    if (_tiger == LaneVizualizatorTrigger.Drag || _tiger == LaneVizualizatorTrigger.None)
                    {
                        return tornadoAnimation;
                    }
                    return null;

                case 18:
                    if (_tiger == LaneVizualizatorTrigger.Activation || _tiger == LaneVizualizatorTrigger.None)
                    {
                        return smokeAnimation;
                    }
                    return null;
                default:
                    return null;
            }
        }
    }
}
