using UnityEngine;
using TMPro;
using DG.Tweening;

public class PowerDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI myPower;
    [SerializeField] TextMeshProUGUI opponentPower;
    [SerializeField] TMP_FontAsset winningFontAsset;
    [SerializeField] TMP_FontAsset lossingFontAsset;
    [SerializeField] TMP_FontAsset drawFontAsset;

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
        float _increasedSize = 90;
        DOTween.To(() => _currentSize, x => _currentSize = x, _increasedSize, 1f)
    .OnUpdate(() => _text.fontSize = _currentSize);
    }

    public void Decrease(TextMeshProUGUI _text)
    {
        float _currentSize = _text.fontSize;
        float _increasedSize = 70;
        DOTween.To(() => _currentSize, x => _currentSize = x, _increasedSize, 1f)
    .OnUpdate(() => _text.fontSize = _currentSize);
    }

    public void Normal(TextMeshProUGUI _text)
    {
        float _currentSize = _text.fontSize;
        float _increasedSize = 80;
        DOTween.To(() => _currentSize, x => _currentSize = x, _increasedSize, 1f)
    .OnUpdate(() => _text.fontSize = _currentSize);
    }
}
