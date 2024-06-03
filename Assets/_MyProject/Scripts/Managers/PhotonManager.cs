using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public const string NAME = "name";
    public const string DECK_NAME = "deckName";
    public const string AMOUNT_OF_CARDS_IN_HAND = "amountOfCardsInHand";
    public const string AMOUNT_OF_DISCARDED_CARDS = "amountOfDiscardedCards";
    public const string AMOUNT_OF_DESTROYED_CARDS = "amountOfDestroyedCards";
    public const string AMOUNT_OF_CARDS_IN_DECK = "amountOfCardsInDeck";
    public static PhotonManager Instance;
    
    public bool IsMasterClient => PhotonNetwork.IsMasterClient;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void JoinRandomRoom()
    {
        SetPhotonPlayerProperties();
    }

    private void SetPhotonPlayerProperties()
    {
        Hashtable _myProperties = new Hashtable
            {
                [NAME] = DataManager.Instance.PlayerData.Name,
                [DECK_NAME] = DataManager.Instance.PlayerData.GetSelectedDeck().Name,
                [AMOUNT_OF_CARDS_IN_DECK] = 12,
                [AMOUNT_OF_CARDS_IN_HAND] = 0,
                [AMOUNT_OF_DESTROYED_CARDS] = 0,
                [AMOUNT_OF_DISCARDED_CARDS] = 0
            };
        PhotonNetwork.LocalPlayer.CustomProperties = _myProperties;
    }

    public void TryUpdateCustomProperty(string _key, string _value)
    {
        if (PhotonNetwork.CurrentRoom is null or null)
        {
            return;
        }
        Hashtable _existingProperties = PhotonNetwork.LocalPlayer.CustomProperties;
        if (!_existingProperties.ContainsKey(_key))
        {
            return;
        }
        _existingProperties[_key] = _value;
        PhotonNetwork.LocalPlayer.SetCustomProperties(_existingProperties);
    }

    public string GetOpponentsProperty(string _key)
    {
        Player _opponent = default;
        foreach (var _potentialOpponent in PhotonNetwork.CurrentRoom.Players)
        {
            if (_potentialOpponent.Value.IsLocal)
            {
                continue;
            }

            _opponent = _potentialOpponent.Value;
            break;
        }

        return _opponent.CustomProperties[_key].ToString();
    }

}