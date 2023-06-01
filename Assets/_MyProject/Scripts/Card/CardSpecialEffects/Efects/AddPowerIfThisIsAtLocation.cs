using UnityEngine;

public class AddPowerIfThisIsAtLocation : CardSpecialEffectBase
{
    [SerializeField] int powerToAdd;
    [SerializeField] LaneLocation location;

    public override void Subscribe()
    {
        TableHandler.OnRevealdCard += CheckLocation;
        GameplayManager.UpdatedGameState += Destroy;
    }

    void Destroy()
    {
        switch (GameplayManager.Instance.GameplayState)
        {
            case GameplayState.ResolvingBeginingOfRound:
                GameplayManager.UpdatedGameState -= Destroy;
                TableHandler.OnRevealdCard -= CheckLocation;
                Destroy(gameObject);
                break;
            case GameplayState.Playing:
                break;
            case GameplayState.Waiting:
                break;
            case GameplayState.ResolvingEndOfRound:
                break;
            default:
                break;
        }
    }

    private void CheckLocation(CardObject _cardObject)
    {
        if (_cardObject != cardObject)
        {
            return;
        }

        if (cardObject.LaneLocation == location)
        {
            cardObject.Stats.Power += powerToAdd;
            //todo show special effect
        }
    }
}
