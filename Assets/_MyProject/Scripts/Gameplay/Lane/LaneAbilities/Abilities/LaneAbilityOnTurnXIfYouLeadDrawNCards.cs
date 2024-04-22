using UnityEngine;

public class LaneAbilityOnTurnXIfYouLeadDrawNCards : LaneAbilityBase
{
    [SerializeField] private int round;
    [SerializeField] private int amountOfCards;

    public override void Subscribe()
    {
        isSubscribed = true;
        GameplayManager.OnFinishedGameplayLoop += CheckForRound;
        GameplayManager.UpdatedRound += ShowActive;
    }

    private void OnDisable()
    {
        if (!isSubscribed)
        {
            return;
        }
        GameplayManager.OnFinishedGameplayLoop -= CheckForRound;
        GameplayManager.UpdatedRound -= ShowActive;
    }

    private void ShowActive()
    {
        if (GameplayManager.Instance.CurrentRound==round)
        {
            laneDisplay.AbilityShowAsActive();
        }
        else
        {
            laneDisplay.AbilityShowAsInactive();
        }
        
    }

    private void CheckForRound()
    {
        if (GameplayManager.Instance.CurrentRound!=round)
        {
            return;
        }
        
        RewardCards();
    }

    private void RewardCards()
    {
        int _myCalculatedPower = GameplayManager.Instance.TableHandler.GetPower(true, laneDisplay.Location);
        int _opponentCalculatedPower = GameplayManager.Instance.TableHandler.GetPower(false, laneDisplay.Location);

        if (_myCalculatedPower>_opponentCalculatedPower)
        {
            for (int _i = 0; _i < amountOfCards; _i++)
            {
                GameplayManager.Instance.DrawCard(GameplayManager.Instance.MyPlayer);
            }
        }
        else if (_opponentCalculatedPower>_myCalculatedPower&&!GameplayManager.IsPvpGame)
        {
            for (int _i = 0; _i < amountOfCards; _i++)
            {
                GameplayManager.Instance.DrawCard(GameplayManager.Instance.OpponentPlayer);
            }
        }
    }
}
