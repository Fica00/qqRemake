using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using System.Collections;

public class PowerDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI myPower;
    [SerializeField] private TextMeshProUGUI opponentPower;
    [SerializeField] private TMP_FontAsset winningFontAsset;
    [SerializeField] private TMP_FontAsset lossingFontAsset;
    [SerializeField] private TMP_FontAsset drawFontAsset;

    public void ShowPower(int _myPower, int _opponentPower)
    {
        myPower.text = _myPower.ToString();
        opponentPower.text = _opponentPower.ToString();

        if (_myPower == _opponentPower)
        {
            myPower.font = drawFontAsset;
            opponentPower.font = drawFontAsset;
        }
        else if (_myPower > _opponentPower)
        {
            myPower.font = winningFontAsset;
            opponentPower.font = lossingFontAsset;
        }
        else
        {
            myPower.font = lossingFontAsset;
            opponentPower.font = winningFontAsset;
        }

        UpdateText(myPower);
        UpdateText(opponentPower);

        void UpdateText(TextMeshProUGUI _text)
        {
            float _currentSize = _text.fontSize;
            DOTween.To(() => _currentSize, x => _currentSize = x, GetFontSize(Convert.ToInt32(_text.text)), 1f)
                .OnUpdate(() => _text.fontSize = _currentSize);
        }
    }

    public void ShowWinner(int _myPower,int _opponentPower,Action _callBack)
    {
        if (_myPower==_opponentPower)
        {
            _callBack?.Invoke();
            return;
        }
        TextMeshProUGUI _text = _myPower > _opponentPower ? myPower : opponentPower;
        _text.GetComponentInParent<Animator>().SetTrigger("Win");
        StartCoroutine(CallCallBackRoutine());
        
        IEnumerator CallCallBackRoutine()
        {
            yield return new WaitForSeconds(1.5f);
            _callBack?.Invoke();
        }

    }

    public void EnlargedPowerAnimation(bool _showMyPower)
    {
        TextMeshProUGUI _text = _showMyPower ? myPower : opponentPower;

        float _currentSize = _text.fontSize;
        float _desiredSize = GetFontSize(Convert.ToInt32(_text.text)) + 15;
        DOTween.To(() => _currentSize, x => _currentSize = x, _desiredSize, 0.1f)
            .OnUpdate(() => _text.fontSize = _currentSize)
            .OnComplete(() =>
            {
                LaneDisplay _location = GetComponent<LaneDisplay>();
                GameplayManager.Instance.TableHandler.GetPower(true, _location.Location);
                GameplayManager.Instance.TableHandler.GetPower(false, _location.Location);
                DOTween.To(() => _desiredSize, x => _desiredSize = x, _currentSize, 0.1f)
                    .OnUpdate(() => _text.fontSize = _desiredSize).SetDelay(.2f);
            }).SetDelay(.5f);
    }

    private int GetFontSize(int _power)
    {
        return _power > 1000 ? 65 : 80;
    }
}
