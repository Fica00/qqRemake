using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class PowerDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI myPower;
    [SerializeField] private TextMeshProUGUI opponentPower;
    [SerializeField] private TMP_FontAsset winningFontAsset;
    [SerializeField] private TMP_FontAsset lossingFontAsset;
    [SerializeField] private TMP_FontAsset drawFontAsset;

    private int increasedFontSize = 90;
    private int decreasedFontSize = 70;
    private int normalFontSize = 80;
    private int winFontSize = 115;

    public void ShowPower(int _myPower, int _opponentPower)
    {
        myPower.text = _myPower.ToString();
        opponentPower.text = _opponentPower.ToString();

        if (_myPower == _opponentPower)
        {
            myPower.font = drawFontAsset;
            Normal(myPower);
            opponentPower.font = drawFontAsset;
            Normal(opponentPower);
        }
        else if (_myPower > _opponentPower)
        {
            myPower.font = winningFontAsset;
            Increase(myPower);
            opponentPower.font = lossingFontAsset;
            Decrease(opponentPower);
        }
        else
        {
            myPower.font = lossingFontAsset;
            Decrease(myPower);
            opponentPower.font = winningFontAsset;
            Increase(opponentPower);
        }
    }

    public void Increase(TextMeshProUGUI _text)
    {
        float _currentSize = _text.fontSize;
        DOTween.To(() => _currentSize, x => _currentSize = x, increasedFontSize, 1f)
    .OnUpdate(() => _text.fontSize = _currentSize);
    }

    public void Decrease(TextMeshProUGUI _text)
    {
        float _currentSize = _text.fontSize;
        DOTween.To(() => _currentSize, x => _currentSize = x, decreasedFontSize, 1f)
    .OnUpdate(() => _text.fontSize = _currentSize);
    }

    public void Normal(TextMeshProUGUI _text)
    {
        float _currentSize = _text.fontSize;
        DOTween.To(() => _currentSize, x => _currentSize = x, normalFontSize, 1f)
    .OnUpdate(() => _text.fontSize = _currentSize);
    }

    public void ShowWinner(int _myPower,int _opponentPower,Action _callBack)
    {
        if (_myPower==_opponentPower)
        {
            return;
        }
        TextMeshProUGUI _text = _myPower > _opponentPower ? myPower : opponentPower;
        float _currentSize = _text.fontSize;
        DOTween.To(() => _currentSize, x => _currentSize = x, winFontSize, 0.1f)
            .OnUpdate(() => _text.fontSize = _currentSize).
            OnComplete(() =>
            {
                DOTween.To(() => _currentSize, x => _currentSize = x, increasedFontSize, 0.1f)
                    .OnUpdate(() => _text.fontSize = _currentSize).
                    OnComplete(()=> _callBack?.Invoke()).SetDelay(0.3f);
            });
    }
}
