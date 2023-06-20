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
       
        TryToRevard(_amountOfMyCardsOnLane, 0);
        TryToRevard(_amountOfOpponentCardsOnLane, 1);

        if (appliedPower[0] || appliedPower[1])
        {
            laneDisplay.AbilityShowAsActive();
        }
        else
        {
            laneDisplay.AbilityShowAsInactive();
        }


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
