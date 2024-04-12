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
        
        ShowName();
    }

    private void OnDisable()
    {
        showStats.onClick.RemoveListener(ShowStats);
    }

    private void ShowStats()
    {
        if (statsDisplay.IsOpen)
        {
            statsDisplay.Close();
            return;
        }
        
        if (player.IsMy)
        {
            statsDisplay.Show(player.AmountOfCardsInHand, player.AmountOfDiscardedCards, DataManager.Instance.PlayerData.OwnedQoomons.Count, player.AmountOfDestroyedCards);
        }
        else
        {
            if (SceneManager.IsAIScene)
            {
                statsDisplay.Show(player.AmountOfCardsInHand, player.AmountOfDiscardedCards, 15, player.AmountOfDestroyedCards);
            }
            else
            {
                if (SceneManager.IsGameplayTutorialScene)
                {
                    return;
                }

                statsDisplay.Show(
                    int.Parse(PhotonManager.Instance.GetOpponentsProperty(PhotonManager.AMOUNT_OF_CARDS_IN_HAND)),
                    int.Parse(PhotonManager.Instance.GetOpponentsProperty(PhotonManager.AMOUNT_OF_DISCARDED_CARDS)),            
                    int.Parse(PhotonManager.Instance.GetOpponentsProperty(PhotonManager.AMOUNT_OF_CARDS_IN_COLLECTION)),            
                    int.Parse(PhotonManager.Instance.GetOpponentsProperty(PhotonManager.AMOUNT_OF_DESTROYED_CARDS))            
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
        if (player!= null && player.IsMy)
        {
            nameDisplay.text = DataManager.Instance.PlayerData.Name;
        }
        else
        {
            if (SceneManager.IsAIScene)
            {
                nameDisplay.text = "Player"+Random.Range(1000,10000);
            }
            else
            {
                if (SceneManager.IsGameplayTutorialScene)
                {
                    nameDisplay.text = Tutorial.MatchMaking.OpponentsName;
                }
                else
                {
                    nameDisplay.text = PhotonManager.Instance.GetOpponentsProperty(PhotonManager.NAME);
                }
            }
        }
    }
}
