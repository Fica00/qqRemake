
public class DoublePowerOnCurrentLane : CardSpecialEffectBase
{
    public override void Subscribe()
    {
        //nothing to do here
    }

    public bool IsMy => cardObject.IsMy;
}
