using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchMakingPlayerDisplay : MonoBehaviour
{
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private TextMeshProUGUI deckName;

    public void Setup(string _name, string _deckName)
    {
        nameDisplay.text = _name;
        deckName.text = _deckName;
    }
}
