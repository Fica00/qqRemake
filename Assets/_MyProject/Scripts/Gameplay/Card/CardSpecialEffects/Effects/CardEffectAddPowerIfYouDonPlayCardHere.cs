using UnityEngine;

public class CardEffectAddPowerIfYouDonPlayCardHere : CardEffectBase
{
    [SerializeField] private int powerToAdd;
    private bool isActive;
    private bool addPower = true;
    private bool disableOnNextTurn;

    public override void Subscribe()
    {
        GameplayManager.Instance.HighlihtWholeLocationDotted(cardObject.LaneLocation, cardObject.IsMy);
        TableHandler.OnRevealdCard += CheckPlayedCard;
        GameplayManager.UpdatedGameState += CheckGameState;
        isActive = true;
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void CheckGameState()
    {
        if (GameplayManager.Instance.GameplayState == GameplayState.ResolvingBeginingOfRound)
        {
            if (!disableOnNextTurn)
            {
                disableOnNextTurn = true;
                addPower = true;
                return;
            }
            
            GameplayManager.Instance.HideHighlihtWholeLocationDotted(cardObject.LaneLocation, cardObject.IsMy);
            Unsubscribe();
            TryAwardPoints();
        }
    }

    private void Unsubscribe()
    {
        if (isActive)
        {
            TableHandler.OnRevealdCard -= CheckPlayedCard;
            GameplayManager.UpdatedGameState -= CheckGameState;
        }

        isActive = false;
    }


    private void CheckPlayedCard(CardObject _cardObject)
    {
        if (_cardObject.IsMy != cardObject.IsMy)
        {
            return;
        }
        
        if (_cardObject.LaneLocation != cardObject.LaneLocation)
        {
            return;
        }

        if (_cardObject==cardObject)
        {
            return;
        }
        
        addPower = false;
    }

    private void TryAwardPoints()
    {
        if (!addPower)
        {
            return;
        }
        
        for (int _i = 0; _i < GameplayManager.Instance.Lanes[(int)cardObject.LaneLocation].LaneSpecifics.AmountOfRevealEffects; _i++)
        {
            cardObject.Stats.Power += powerToAdd;
        }
    }
}
