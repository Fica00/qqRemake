using UnityEngine;

public class LaneAbilityAddXPowerIfYouHaveMoreCardsHere : LaneAbilityBase
{
    [SerializeField] int powerAmount;

    bool[] appliedPower = new bool[2];//0 for player, 1 for opponent

    public override void Subscribe()
    {

        TableHandler.OnRevealdCard += CheckCount;
        CountCards();
    }

    private void OnDisable()
    {
        TableHandler.OnRevealdCard -= CheckCount;
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
        int _amountOfMyCardsOnLane = GameplayManager.Instance.TableHandler.GetCards(true, laneDisplay.Location).Count;
        int _amountOfOpponentCardsOnLane = GameplayManager.Instance.TableHandler.GetCards(false, laneDisplay.Location).Count;

        TryToRevard(_amountOfMyCardsOnLane, _amountOfOpponentCardsOnLane, 0);
        TryToRevard(_amountOfMyCardsOnLane, _amountOfOpponentCardsOnLane, 1);

        if (appliedPower[0] || appliedPower[1])
        {
            laneDisplay.AbilityShowAsActive();
        }
        else
        {
            laneDisplay.AbilityShowAsInactive();
        }

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
