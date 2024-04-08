using UnityEngine;

public class LaneAbilityOnTurnXAllPutCardHere : LaneAbilityBase
{
    [SerializeField] private int round;
    private bool subscribed2;
    
    public int Round => round;

    public override void Subscribe()
    {
        isSubscribed = true;
        GameplayManager.UpdatedRound += CheckRound;
        CheckRound();
    }

    private void OnDisable()
    {
        if (subscribed2)
        {
            subscribed2 = false;
            GameplayManager.UpdatedGameState -= CheckState;
        }
        if (!isSubscribed)
        {
            return;
        }
        GameplayManager.UpdatedRound -= CheckRound;
    }

    private void CheckRound()
    {
        if (round == GameplayManager.Instance.CurrentRound)
        {
            laneDisplay.AbilityShowAsActive();
            GameplayManager.UpdatedGameState += CheckState;
            subscribed2 = true;
        }
        else if (round>GameplayManager.Instance.CurrentRound)
        {
            if (subscribed2==false)
            {
                return;
            }
            laneDisplay.AbilityShowAsInactive();
            isSubscribed = false;
            GameplayManager.UpdatedRound -= CheckRound;
        }
    }

    private void CheckState()
    {
        if (GameplayManager.Instance.GameplayState!= GameplayState.ResolvingEndOfRound)
        {
            return;
        }
        
        PlacePlayersCard(GameplayManager.Instance.MyPlayer);
        if (!GameplayManager.IsPvpGame)
        {
            PlacePlayersCard(GameplayManager.Instance.OpponentPlayer);
        }
        
        GameplayManager.UpdatedGameState -= CheckState;
        subscribed2 = false;
    }

    private void PlacePlayersCard(GameplayPlayer _player)
    {
        CardObject _card = _player.GetQommonFromHand();
        if (_card == null)
        {
            return;
        }
        _card.ForcePlace(laneDisplay);
    }
}
