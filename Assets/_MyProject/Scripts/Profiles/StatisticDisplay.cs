using TMPro;
using UnityEngine;

public class StatisticDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI labelDisplay;
    [SerializeField] private TextMeshProUGUI amountDisplay;
    
    public void Setup(string _label, int _amount)
    {
        labelDisplay.text = _label;
        amountDisplay.text = _amount.ToString();
    }
}
