using UnityEngine;

public class LaneAbilityAddXEnergyIfYouHaveNCardsHere : LaneAbilityBase
{
    [SerializeField] int energyAmount;
    [SerializeField] int amountOfCards;

    public override void Subscribe()
    {
        GameplayManager.UpdatedRound += CountCards;
        CountCards();
    }

    void CountCards()
    {
        int _myAmountOfCards = GameplayManager.Instance.TableHandler.GetCards(true, laneDisplay.Location).Count;
        int _opponentAmountOfCards = GameplayManager.Instance.TableHandler.GetCards(false, laneDisplay.Location).Count;

        if (_myAmountOfCards==amountOfCards)
        {
            laneDisplay.AbilityShowAsActive();
            GameplayManager.Instance.MyPlayer.Energy += energyAmount;
        }
        else
        {
            laneDisplay.AbilityShowAsInactive();
        }

        if (!GameplayManager.IsPVPGame&&_opponentAmountOfCards==amountOfCards)
        {
            GameplayManager.Instance.OpponentPlayer.Energy += energyAmount;
        }
    }
}
