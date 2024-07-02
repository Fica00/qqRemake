using System;
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
        
        if (SceneManager.IsAIScene)
        {
            if (player.IsMy)
            {
                statsDisplay.Show(player.AmountOfCardsInHand, player.AmountOfDiscardedCards, player.AmountOfCardsInDeck, player.AmountOfDestroyedCards);
            }
            else
            {
                statsDisplay.Show(player.AmountOfCardsInHand, player.AmountOfDiscardedCards, player.AmountOfCardsInDeck, player.AmountOfDestroyedCards);
            }
        }
        else
        {
            if (player != default && player.IsMy)
            {
                statsDisplay.Show(player.AmountOfCardsInHand, player.AmountOfDiscardedCards, player.AmountOfCardsInDeck, player.AmountOfDestroyedCards);
            }
            else
            {
                if (SceneManager.IsGameplayTutorialScene)
                {
                    statsDisplay.Show(player.AmountOfCardsInHand, player.AmountOfDiscardedCards, player.AmountOfCardsInDeck, player.AmountOfDestroyedCards);
                    return;
                }
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
            if (SceneManager.IsGameplayTutorialScene)
            {
                nameDisplay.text = Tutorial.MatchMaking.MyName;
            }
        }
        else
        {
            if (SceneManager.IsAIScene)
            {
                nameDisplay.text = BotPlayer.Name;
            }
            else
            {
                if (SceneManager.IsGameplayTutorialScene)
                {
                    nameDisplay.text = Tutorial.MatchMaking.OpponentsName;
                }
            }
        }
    }
}
