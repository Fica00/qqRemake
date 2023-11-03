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
}
