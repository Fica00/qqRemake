using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private Button showStats;
    [SerializeField] private PlayerStatsDisplay statsDisplay;
    private GameplayPlayer player;
    
    private void OnEnable()
    {
        showStats.onClick.AddListener(ShowStats);
    }

    private void OnDisable()
    {
        showStats.onClick.RemoveListener(ShowStats);
    }

    private void ShowStats()
    {
        if (player.IsMy)
        {
            statsDisplay.Show(player.AmountOfCardsInHand, player.AmountOfDiscardedCards, DataManager.Instance.PlayerData.OwnedQommons.Count, player.AmountOfDestroyedCards);
        }
        else
        {
            if (SceneManager.IsAIScene)
            {
                statsDisplay.Show(player.AmountOfCardsInHand, player.AmountOfDiscardedCards, 15, player.AmountOfDestroyedCards);
            }
            else
            {
                statsDisplay.Show(
                    PhotonManager.Instance.GetOpponentsProperty(PhotonManager.AMOUNT_OF_CARDS_IN_HAND),
                    PhotonManager.Instance.GetOpponentsProperty(PhotonManager.AMOUNT_OF_DISCARDED_CARDS),            
                    PhotonManager.Instance.GetOpponentsProperty(PhotonManager.AMOUNT_OF_CARDS_IN_COLLECTION),            
                    PhotonManager.Instance.GetOpponentsProperty(PhotonManager.AMOUNT_OF_DESTROYED_CARDS)            
                    );   
            }
        }
    }

    public void Setup(GameplayPlayer _player)
    {
        player = _player;
        ShowName();
    }

    private void ShowName()
    {
        if (player.IsMy)
        {
            nameDisplay.text = DataManager.Instance.PlayerData.Name;
        }
        else
        {
            nameDisplay.text = SceneManager.IsAIScene 
                ? "Bot" 
                : PhotonManager.Instance.GetOpponentsProperty(PhotonManager.NAME);
        }
    }
}
