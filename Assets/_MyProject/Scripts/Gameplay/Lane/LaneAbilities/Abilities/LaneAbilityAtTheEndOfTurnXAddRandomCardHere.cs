using UnityEngine;

public class LaneAbilityAtTheEndOfTurnXAddRandomCardHere : LaneAbilityBase
{
    [SerializeField] private int round;

    private bool subscribed2;

    public override void Subscribe()
    {
        isSubscribed = true;
        GameplayManager.OnFinishedGameplayLoop += CheckRound;
    }

    private void OnDisable()
    {
        if (!isSubscribed)
        {
            return;
        }
        GameplayManager.OnFinishedGameplayLoop -= CheckRound;
    }

    private void CheckRound()
    {
        if (round - 1 == GameplayManager.Instance.CurrentRound)
        {
            laneDisplay.AbilityShowAsActive();
            GameplayManager.UpdatedGameState += CheckState;
            subscribed2 = true;
        }
        else if (round - 1 > GameplayManager.Instance.CurrentRound)
        {
            if (subscribed2 == false)
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
        if (GameplayManager.Instance.GameplayState != GameplayState.ResolvingEndOfRound)
        {
            return;
        }

        AddRandomCard(GameplayManager.Instance.MyPlayer);
        if (!GameplayManager.IsPvpGame)
        {
            AddRandomCard(GameplayManager.Instance.OpponentPlayer);
        }

        GameplayManager.UpdatedGameState -= CheckState;
        subscribed2 = false;
    }

    private void AddRandomCard(GameplayPlayer _player)
    {
        var _cardList = _player.CardsInDeck;

        if (_cardList.Count == 0)
        {
            return;
        }

        CardObject _randomCard = _cardList[Random.Range(0, _cardList.Count)];

        _randomCard.ForcePlace(laneDisplay);
    }
}