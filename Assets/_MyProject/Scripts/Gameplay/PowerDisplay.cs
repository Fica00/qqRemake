using UnityEngine;
using TMPro;

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
    }
}
