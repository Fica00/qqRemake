
public class CardEffectDoublePowerOnCurrentLane : CardEffectBase
{
    public override void Subscribe()
    {
        //nothing to do here
    }

    public bool IsMy => cardObject.IsMy;
}
