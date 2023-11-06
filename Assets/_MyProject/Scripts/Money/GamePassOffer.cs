using System;

[Serializable]
public class GamePassOffer
{
    public GamePass GamePass;
    public double Cost;
    public string Owner;
    
    public override bool Equals(object _obj)
    {
        if (_obj == null || GetType() != _obj.GetType())
            return false;

        var _offer = (GamePassOffer)_obj;
        return Equals(GamePass, _offer.GamePass) && Cost == _offer.Cost && Owner == _offer.Owner;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GamePass, Cost, Owner);
    }
}