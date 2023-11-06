using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class GamePass
{
    public int PictureId;
    public double Coins;
    public double StorageSize;
    public string Name;

    [JsonIgnore] public Sprite Sprite => CardsManager.Instance.GetCardSpriteGamePass(PictureId);

    public GamePass()
    {
        
    }
    
    public GamePass(GamePass _stats)
    {
        PictureId = _stats.PictureId;
        Coins = _stats.Coins;
        StorageSize = _stats.StorageSize;
        Name = _stats.Name;
    }
    
    public override bool Equals(object _obj)
    {
        if (_obj == null || GetType() != _obj.GetType())
            return false;

        var _other = (GamePass)_obj;
        return PictureId == _other.PictureId &&
               Coins == _other.Coins &&
               StorageSize == _other.StorageSize &&
               Name == _other.Name;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int _hash = 17;
            _hash = _hash * 23 + PictureId.GetHashCode();
            _hash = _hash * 23 + Coins.GetHashCode();
            _hash = _hash * 23 + StorageSize.GetHashCode();
            _hash = _hash * 23 + (Name != null ? Name.GetHashCode() : 0);
            return _hash;
        }
    }
}
