using UnityEngine;

public class LaneAbilityOnTurnXIfYouLeadDrawNCards : LaneAbilityBase
{
    [SerializeField] int round;
    [SerializeField] int amountOfCards;

    public override void Subscribe()
    {
        GameplayManager.UpdatedRound += CheckForRound;
    }

    void CheckForRound()
    {
        if (GameplayManager.Instance.CurrentRound==round)
        {
            laneDisplay.AbilityShowAsActive();

            int _myCalculatedPower = GameplayManager.Instance.TableHandler.GetPower(true, laneDisplay.Location);
            int _opponentCalculatedPower = GameplayManager.Instance.TableHandler.GetPower(false, laneDisplay.Location);

            if (_myCalculatedPower>_opponentCalculatedPower)
            {
                for (int i = 0; i < amountOfCards; i++)
                {
                    GameplayManager.Instance.DrawCard(GameplayManager.Instance.MyPlayer);
                }
            }
            else if (_opponentCalculatedPower>_myCalculatedPower&&!GameplayManager.IsPVPGame)
            {
                for (int i = 0; i < amountOfCards; i++)
                {
                    GameplayManager.Instance.DrawCard(GameplayManager.Instance.OpponentPlayer);
                }
            }
        }
        else
        {
            laneDisplay.AbilityShowAsInactive();
        }
    }
}
