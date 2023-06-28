using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class LaneVizualizator : MonoBehaviour
{
    [SerializeField] private LaneLocation location;
    [Space()]
    [SerializeField]
    private List<LanePlaceIdentifier> myPlaces;
    [SerializeField] private Image myLane;
    [Space()]
    [SerializeField]
    private List<LanePlaceIdentifier> opponentPlaces;
    [SerializeField] private Image opponentLane;

    private void OnEnable()
    {
        GameplayManager.OnFlashPlace += FlashSpot;
        GameplayManager.OnFlashWholePlace += FlashWholePlace;
        GameplayManager.OnHighlihtWholePlace += HighlihtWholePlace;
        GameplayManager.OnHideHighlightWholePlace += HideHighlihtWholePlace;
        GameplayManager.OnFlashAllSpotsOnLocation += FlashAllSpotsOnLocation;
    }

    private void OnDisable()
    {
        GameplayManager.OnFlashPlace -= FlashSpot;
        GameplayManager.OnFlashWholePlace -= FlashWholePlace;
        GameplayManager.OnHighlihtWholePlace -= HighlihtWholePlace;
        GameplayManager.OnHideHighlightWholePlace -= HideHighlihtWholePlace;
        GameplayManager.OnFlashAllSpotsOnLocation -= FlashAllSpotsOnLocation;
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
}
