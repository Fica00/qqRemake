using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneAbilityOnTurnXAllPutCardHere : LaneAbilityBase
{
    [SerializeField] int round;
    bool isSubscribed = false;

    public override void Subscribe()
    {
        isSubscribed = true;
        GameplayManager.UpdatedRound += CheckRound;
    }

    private void OnDisable()
    {
        if (isSubscribed)
        {
            GameplayManager.UpdatedRound -= CheckRound;
        }
    }

    void CheckRound()
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
    }

    void PlacePlayersCard(GameplayPlayer _player)
    {
        CardObject _card = _player.GetQommonFromHand();
        if (_card == null)
        {
            return;
        }
        _card.ForcePlace(laneDisplay);
    }
}
