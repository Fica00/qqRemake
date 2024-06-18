using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class Utils
{
    public static string RemoveWhitespace(this string _input)
    {
        return new string(_input.ToCharArray()
            .Where(_c => !char.IsWhiteSpace(_c))
            .ToArray());
    }

    public static string GetItemName(ItemType _item)
    {
        switch (_item)
        {
            case ItemType.Qoomon:
                return "Qoomon";
            case ItemType.Exp:
                return "EXP";
            default:
                throw new ArgumentOutOfRangeException(nameof(_item), _item, null);
        }
    }

    public static void DoColor(TextMeshProUGUI _text, float _alpha, float _duration, float _delay, Action _callBack = null)
    {
        Color _color = _text.color;
        _color.a = _alpha;
        _text.DOColor(_color, _duration).SetDelay(_delay).OnComplete(() =>
        {
            _callBack?.Invoke();
        });
    }    
    
    public static void DoScale(Image _image, Vector3 _size, float _duration, float _delay, Action _callBack = null)
    {
        _image.transform.DOScale(_size, _duration).SetDelay(_delay).OnComplete(() =>
        {
            _callBack?.Invoke();
        });
    }
    
    public static void DoScale(GameObject _gameObject, Vector3 _size, float _duration, float _delay, Action _callBack = null)
    {
        _gameObject.transform.DOScale(_size, _duration).SetDelay(_delay).OnComplete(() =>
        {
            _callBack?.Invoke();
        });
    }
    
    public static void DoColor(Image _image, float _alpha, float _duration, float _delay, Action _callBack = null)
    {
        Color _color = _image.color;
        _color.a = _alpha;
        _image.DOColor(_color, _duration).SetDelay(_delay).OnComplete(() =>
        {
            _callBack?.Invoke();
        });
    }

    public static void SetAlphaZero(this Image _image)
    {
        Color _color = _image.color;
        _color.a = 0;
        _image.color = _color;
    }
    
    public static void SetAlphaOne(this Image _image)
    {
        Color _color = _image.color;
        _color.a = 1;
        _image.color = _color;
    }
    
    public static void SetAlphaZero(this TextMeshProUGUI _text)
    {
        Color _color = _text.color;
        _color.a = 0;
        _text.color = _color;
    }
    
    public static void SetAlphaOne(this TextMeshProUGUI _text)
    {
        Color _color = _text.color;
        _color.a = 1;
        _text.color = _color;
    }
    
    public static List<CardObject> OrderQommons(List<int> _qommonIds)
    {
        List<CardObject> _cards = new List<CardObject>();
        foreach (var _qommonId in _qommonIds)
        {
            _cards.Add(CardsManager.Instance.GetCardObject(_qommonId));
        }

        return OrderQommons(_cards);
    }

    public static List<CardObject> OrderQommons(List<CardObject> _qoomons)
    {
        switch (FilterHandler.FilterType)
        {
            case FilterType.ByName:
                return _qoomons.OrderBy(_qommonInDeck => _qommonInDeck.Details.Name).ToList();
            case FilterType.ByMana:
                return _qoomons.OrderBy(_qommonInDeck => _qommonInDeck.Details.Mana).ToList();
            case FilterType.ByPower:
                return _qoomons.OrderBy(_qommonInDeck => _qommonInDeck.Details.Power).ToList();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static DateTime WithoutHours(this DateTime _date)
    {
        return new DateTime(_date.Year, _date.Month, _date.Day,0,0,0);
    }
}
