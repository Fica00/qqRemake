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
        DeckData _nextDeck = default;
        bool _fetchNext = false;
        foreach (var _deck in _playerData.Decks)
        {
            if (_deck.Id==_playerData.SelectedDeck)
            {
                _fetchNext = true;
                continue;
            }

            if (_fetchNext)
            {
                _nextDeck = _deck;
            }
        }

        if (_nextDeck==default)
        {
            _nextDeck = _playerData.Decks[0];
        }

        _playerData.SelectedDeck = _nextDeck.Id;
    }
}
