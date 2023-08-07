using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneAbilityOnTurnXAllPutCardHere : LaneAbilityBase
{
    [SerializeField] private int round;
    private bool isSubscribed = false;

    public override void Subscribe()
    {
        isSubscribed = true;
        GameplayManager.UpdatedRound += CheckRound;
        CheckRound();
    }

    private void OnDisable()
    {
        if (isSubscribed)
        {
            GameplayManager.UpdatedRound -= CheckRound;
        }
    }

    private void CheckRound()
    {
        if (round == GameplayManager.Instance.CurrentRound)
        {
            laneDisplay.AbilityShowAsActive();
            PlacePlayersCard(GameplayManager.Instance.MyPlayer);
            if (!GameplayManager.IsPvpGame)
            {
                PlacePlayersCard(GameplayManager.Instance.OpponentPlayer);
            }
        }
        else if (round<GameplayManager.Instance.CurrentRound)
        {
            laneDisplay.AbilityShowAsInactive();
            isSubscribed = false;
            GameplayManager.UpdatedRound -= CheckRound;
        }
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
