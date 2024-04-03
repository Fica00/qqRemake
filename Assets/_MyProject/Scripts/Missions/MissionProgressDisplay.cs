using TMPro;
using UnityEngine;

public class MissionProgressDisplay : MonoBehaviour
{
    [SerializeField] private GameObject lockedDisplay;
    [SerializeField] private TextMeshProUGUI number;
    [SerializeField] private GameObject completed;

    public void Setup(bool _isLocked, int _number)
    {
        lockedDisplay.SetActive(_isLocked);
        completed.SetActive(!_isLocked);
        number.text = _number.ToString();
    }
}
