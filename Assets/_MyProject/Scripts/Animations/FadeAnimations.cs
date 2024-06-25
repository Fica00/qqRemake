using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeAnimations : MonoBehaviour
{
    [SerializeField] private bool takeThisObjectAsWell;
    [SerializeField] private List<GameObject> ignoreObjects = new();

    private List<Image> images = new ();
    private List<TextMeshProUGUI> texts = new ();

    private void Awake()
    {
        GetAllComponentsInChildren(transform, images);
        GetAllComponentsInChildren(transform, texts);
        if (!takeThisObjectAsWell)
        {
            return;
        }
        Image _image = GetComponent<Image>();
        if (_image)
        {
            images.Add(_image);
        }

        TextMeshProUGUI _text = GetComponent<TextMeshProUGUI>();
        if (_text)
        {
            texts.Add(_text);
        }
    }
    
    private void GetAllComponentsInChildren<T>(Transform _parent, List<T> _results) where T : Component
    {
        foreach (Transform _child in _parent)
        {
            T _component = _child.GetComponent<T>();
            if (_component != null)
            {
                _results.Add(_component);
            }
            GetAllComponentsInChildren(_child, _results);
        }
    }

    public void FadeOut(float _duration, Action _callBack)
    {
        DoColor(0,1,_duration, _callBack);
    }

    public void FadeIn(float _duration, Action _callBack)
    {
        DoColor(1, 0,_duration, _callBack);
    }

    private void DoColor(float _newAlpha,float _oldAlpha, float _duration, Action _callBack)
    {
        foreach (var _image in images)
        {
            if (ignoreObjects.Contains(_image.gameObject))
            {
                continue;
            }
            Color _color = _image.color;
            _color.a = _oldAlpha;
            _image.color = _color;
            _color.a = _newAlpha;
            _image.DOColor(_color, _duration);
        }

        foreach (var _text in texts)
        {
            if (ignoreObjects.Contains(_text.gameObject))
            {
                continue;
            }
            Color _color = _text.color;
            _color.a = _oldAlpha;
            _text.color = _color;
            _color.a = _newAlpha;
            _text.DOColor(_color, _duration);
        }
        
        StartCoroutine(CallCallBack());

        IEnumerator CallCallBack()
        {
            yield return new WaitForSeconds(_duration); 
            _callBack?.Invoke();
        }
    }
}
