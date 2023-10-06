using UnityEngine;

public class LaneAbilityAddXPowerIfYouHaveMoreCardsHere : LaneAbilityBase
{
    [SerializeField] private int powerAmount;

    private bool[] appliedPower = new bool[2];//0 for player, 1 for opponent

    public override void Subscribe()
    {
        isSubscribed = true;
        TableHandler.OnRevealdCard += CheckCount;
        CountCards();
    }

    private void OnDisable()
    {
        if (!isSubscribed)
        {
            return;
        }
        TableHandler.OnRevealdCard -= CheckCount;
    }

    private void CheckCount(CardObject _cardObject)
    {
        if (_cardObject.LaneLocation != laneDisplay.Location)
        {
            return;
        }

        CountCards();
    }

    private void CountCards()
    {
        int _amountOfMyCardsOnLane = GameplayManager.Instance.TableHandler.GetCards(true, laneDisplay.Location).Count;
        int _amountOfOpponentCardsOnLane = GameplayManager.Instance.TableHandler.GetCards(false, laneDisplay.Location).Count;

        TryToRevard(_amountOfMyCardsOnLane, _amountOfOpponentCardsOnLane, 0);
        TryToRevard(_amountOfOpponentCardsOnLane, _amountOfMyCardsOnLane, 1);

        void TryToRevard(int _amountOfMyCards,int _amountOfOpponentCards, int _playerNumber)
        {
            if (_amountOfMyCards> _amountOfOpponentCards)
            {
                if (!appliedPower[_playerNumber])
                {
                    appliedPower[_playerNumber] = true;
                    laneDisplay.LaneSpecifics.ChangeExtraPower(_playerNumber, powerAmount);
                }
            }
            else
            {
                if (appliedPower[_playerNumber])
                {
                    appliedPower[_playerNumber] = false;
                    laneDisplay.LaneSpecifics.ChangeExtraPower(_playerNumber, -powerAmount);
                }
            }
        }
    }
}
