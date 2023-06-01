using UnityEngine;

public class LaneAbilityAddPowerIfAmountOfCards : LaneAbilityBase
{
    [SerializeField] int amountOfCards;
    [SerializeField] int addPower;

    bool[] appliedPower = new bool[2];//0 for player, 1 for opponent
    public override void Subscribe()
    {
        for (int i = 0; i < appliedPower.Length; i++)
        {
            appliedPower[i] = false;
        }

        TableHandler.OnRevealdCard += CheckCount;
        CountCards();
    }

    private void OnDestroy()
    {
        TableHandler.OnRevealdCard += CheckCount;
    }

    void CheckCount(CardObject _cardObject)
    {
        if (_cardObject.LaneLocation != laneDisplay.Location)
        {
            return;
        }

        CountCards();
    }

    void CountCards()
    {
        int _amountOfMyCardsOnLane = 0;
        int _amountOfOpponentCardsOnLane = 0;
        switch (laneDisplay.Location)
        {
            case LaneLocation.Top:
                _amountOfMyCardsOnLane = GameplayManager.Instance.MyPlayer.CardsOnTop.Count;
                _amountOfOpponentCardsOnLane = GameplayManager.Instance.BotPlayer.CardsOnTop.Count;
                break;
            case LaneLocation.Mid:
                _amountOfMyCardsOnLane = GameplayManager.Instance.MyPlayer.CardsOnMid.Count;
                _amountOfOpponentCardsOnLane = GameplayManager.Instance.BotPlayer.CardsOnMid.Count;
                break;
            case LaneLocation.Bot:
                _amountOfMyCardsOnLane = GameplayManager.Instance.MyPlayer.CardsOnBot.Count;
                _amountOfOpponentCardsOnLane = GameplayManager.Instance.BotPlayer.CardsOnBot.Count;
                break;
            default:
                break;
        }

        TryToRevard(_amountOfMyCardsOnLane, 0);
        TryToRevard(_amountOfOpponentCardsOnLane, 1);


        void TryToRevard(int _amountOfCards, int _playerNumber)
        {
            if (_amountOfCards == amountOfCards)
            {
                if (!appliedPower[_playerNumber])
                {
                    appliedPower[_playerNumber] = true;
                    laneDisplay.LaneSpecifics.ChangeExtraPower(_playerNumber, addPower);
                }
            }
            else
            {
                if (appliedPower[_playerNumber])
                {
                    appliedPower[_playerNumber] = false;
                    laneDisplay.LaneSpecifics.ChangeExtraPower(_playerNumber, -addPower);
                }
            }
        }

    }
}
