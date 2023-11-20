using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LineUpHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lineUpDisplay;
    [SerializeField] private Button changeLineUpButton;

    private void OnEnable()
    {
        changeLineUpButton.onClick.AddListener(ChangeLineUp);
    }

    private void OnDisable()
    {
        changeLineUpButton.onClick.RemoveListener(ChangeLineUp);
    }

    private void ChangeLineUp()
    {
        PlayerData _playerData = DataManager.Instance.PlayerData;
        int _indexOfCurrentLineUp = _playerData.SelectedDeck;
        _indexOfCurrentLineUp++;
        if (_playerData.Decks.Count<=_indexOfCurrentLineUp)
        {
            _indexOfCurrentLineUp = 0;
        }

        _playerData.SelectedDeck = _indexOfCurrentLineUp;
    }
}
