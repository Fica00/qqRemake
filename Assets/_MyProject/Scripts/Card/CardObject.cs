using System;
using UnityEngine;

[Serializable]
public class CardObject : MonoBehaviour
{
    [field: SerializeField] public CardDetails Details { get; private set; }
    [field: SerializeField] public CardDisplay Display { get; private set; }
    public CardStats Stats { get; private set; }

    [HideInInspector] public bool IsMy;

    [SerializeField] CardInputInteractions cardInputInteractions;

    public CardLocation CardLocation { get; private set; }

    public void Setup(bool _isMy)
    {
        Stats = new CardStats()
        {
            Power = Details.Power,
            Mana = Details.Mana
        };
        IsMy = _isMy;
        cardInputInteractions.Setup(this);
        Display.Setup(this);
    }

    public void SetCardLocation(CardLocation _newLocation)
    {
        CardLocation = _newLocation;
    }
}
