using UnityEngine;
using System;

[Serializable]
public class CardDetails
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public Sprite SpriteInHand;
    [field: SerializeField] public Sprite SpriteGamePass;
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public int Power { get; private set; }
    [field: SerializeField] public int Mana { get; private set; }
}
