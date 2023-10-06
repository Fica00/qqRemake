using UnityEngine;

public class LaneAbilityAddXEnergyIfYouHaveNCardsHere : LaneAbilityBase
{
    [SerializeField] private int energyAmount;
    [SerializeField] private int amountOfCards;

    public override void Subscribe()
    {
        isSubscribed = true;
        GameplayManager.UpdatedRound += CountCards;
        CountCards();
    }

    private void OnDisable()
    {
        if (!isSubscribed)
        {
            return;
        }
        GameplayManager.UpdatedRound -= CountCards;
    }

    private void CountCards()
    {
        int _myAmountOfCards = GameplayManager.Instance.TableHandler.GetCards(true, laneDisplay.Location).Count;
        int _opponentAmountOfCards = GameplayManager.Instance.TableHandler.GetCards(false, laneDisplay.Location).Count;

        if (_myAmountOfCards==amountOfCards)
        {
            GameplayManager.Instance.MyPlayer.Energy += energyAmount;
        }

        if (!GameplayManager.IsPvpGame&&_opponentAmountOfCards==amountOfCards)
        {
            GameplayManager.Instance.OpponentPlayer.Energy += energyAmount;
        }
    }
}
