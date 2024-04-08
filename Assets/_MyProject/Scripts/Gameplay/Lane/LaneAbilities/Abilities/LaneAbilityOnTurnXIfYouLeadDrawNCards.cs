using UnityEngine;

public class LaneAbilityOnTurnXIfYouLeadDrawNCards : LaneAbilityBase
{
    [SerializeField] private int round;
    [SerializeField] private int amountOfCards;
    private bool subscribe2;

    public override void Subscribe()
    {
        isSubscribed = true;
        GameplayManager.UpdatedRound += CheckForRound;
    }

    private void OnDisable()
    {
        if (subscribe2)
        {
            subscribe2 = false;
        }
        if (!isSubscribed)
        {
            return;
        }
        GameplayManager.UpdatedRound -= CheckForRound;
    }

    private void CheckForRound()
    {
        if (GameplayManager.Instance.CurrentRound==round)
        {
            laneDisplay.AbilityShowAsActive();
            subscribe2 = true;
            GameplayManager.UpdatedGameState += CheckGameState;
        }
        else
        {
            laneDisplay.AbilityShowAsInactive();
        }
    }

    private void CheckGameState()
    {
        if (GameplayManager.Instance.GameplayState != GameplayState.ResolvingEndOfRound)
        {
            return;
        }
        
        GameplayManager.UpdatedGameState -= CheckGameState;
        int _myCalculatedPower = GameplayManager.Instance.TableHandler.GetPower(true, laneDisplay.Location);
        int _opponentCalculatedPower = GameplayManager.Instance.TableHandler.GetPower(false, laneDisplay.Location);

        if (_myCalculatedPower>_opponentCalculatedPower)
        {
            for (int i = 0; i < amountOfCards; i++)
            {
                GameplayManager.Instance.DrawCard(GameplayManager.Instance.MyPlayer);
            }
        }
        else if (_opponentCalculatedPower>_myCalculatedPower&&!GameplayManager.IsPvpGame)
        {
            for (int i = 0; i < amountOfCards; i++)
            {
                GameplayManager.Instance.DrawCard(GameplayManager.Instance.OpponentPlayer);
            }
        }

        subscribe2 = false;
    }
}
