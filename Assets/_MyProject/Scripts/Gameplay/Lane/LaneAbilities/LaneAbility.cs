using System.Collections.Generic;
using UnityEngine;

public class LaneAbility : MonoBehaviour
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public List<LaneAbilityBase> Abilities { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public int FontSize { get; private set; }

    public void Setup(LaneDisplay _laneDisplay)
    {
        foreach (var _ability in Abilities)
        {
            _ability.Setup(_laneDisplay);
        }
    }
}
