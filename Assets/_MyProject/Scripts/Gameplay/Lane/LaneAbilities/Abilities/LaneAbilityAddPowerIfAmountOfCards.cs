using UnityEngine;

public class LaneAbilityAddPowerIfAmountOfCards : LaneAbilityBase
{
    [SerializeField] private int amountOfCards;
    [SerializeField] private int addPower;

    private bool[] appliedPower = new bool[2];//0 for player, 1 for opponent
    
    public override void Subscribe()
    {
        isSubscribed = true;
        for (int i = 0; i < appliedPower.Length; i++)
        {
            appliedPower[i] = false;
        }

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
