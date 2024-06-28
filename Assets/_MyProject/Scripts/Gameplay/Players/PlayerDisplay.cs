using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private Button showStats;
    [SerializeField] private PlayerStatsDisplay statsDisplay;
    [SerializeField] private Transform holder;
    [SerializeField] private AvatarDisplay avatarDisplay;
    private GameplayPlayer player;
    
    private void OnEnable()
    {
        showStats.onClick.AddListener(ShowStats);
        
        ShowName();
        GameplayManager.OnGameplayStarted += ShowName;
    }

    private void OnDisable()
    {
        showStats.onClick.RemoveListener(ShowStats);
        GameplayManager.OnGameplayStarted -= ShowName;
        DOTween.KillAll();
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

                SocketServerCommunication.Instance.RegisterMessage(gameObject, nameof(RequestOpponentStats));
            }
        }
    }

    private void RequestOpponentStats()
    {
        SocketServerCommunication.Instance.RegisterMessage(gameObject,nameof(TellOpponentMyStats), JsonConvert.SerializeObject(GetMyStats()));
    }

    private OpponentStats GetMyStats()
    {
        GameplayPlayer _myPlayer = GameplayManager.Instance.MyPlayer;
        OpponentStats _stats = new OpponentStats
        {
            Name = DataManager.Instance.PlayerData.Name,
            AmountOfCardsInHand = _myPlayer.AmountOfCardsInHand,
            AmountOfDiscardedCards = _myPlayer.AmountOfDiscardedCards,
            AmountOfCCardsInDeck = _myPlayer.AmountOfCardsInDeck,
            AmountOfDestroyedCards = _myPlayer.AmountOfDestroyedCards
        };

        return _stats;
    }
    
    private void TellOpponentMyStats(string _data)
    {
        OpponentStats _stats = JsonConvert.DeserializeObject<OpponentStats>(_data);
        statsDisplay.Show(
            _stats.AmountOfCardsInHand,
            _stats.AmountOfDiscardedCards,            
            _stats.AmountOfCCardsInDeck,            
            _stats.AmountOfDestroyedCards            
        );
    }

    public void Setup(GameplayPlayer _player)
    {
        player = _player;
        ShowName();
        avatarDisplay.Setup(_player.IsMy ? AvatarSo.Get(DataManager.Instance.PlayerData.SelectedAvatar) : AvatarSo.Get(BotPlayer.AvatarId));
    }

    private void ShowName()
    {
        holder.DOScale(Vector3.one, 1).OnComplete(() =>
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
                    else
                    {
                        SocketServerCommunication.Instance.RegisterMessage(gameObject, nameof(RequestName));
                    }
                }
            } 
        });
    }

    private void RequestName()
    {
        OpponentStats _stats = new OpponentStats() { Name = DataManager.Instance.PlayerData.Name, AvatarId = DataManager.Instance.PlayerData.SelectedAvatar};
        SocketServerCommunication.Instance.RegisterMessage(gameObject, nameof(ShowOpponentName), JsonConvert.SerializeObject(_stats));
    }

    private void ShowOpponentName(string _data)
    {
        OpponentStats _stats = JsonConvert.DeserializeObject<OpponentStats>(_data);
        nameDisplay.text = _stats.Name;
        avatarDisplay.Setup(AvatarSo.Get(_stats.AvatarId));
    }
}
