using System.Collections.Generic;
using UnityEngine;

public class LaneAbility : MonoBehaviour
{
    [field: SerializeField] public List<LaneAbilityBase> abilities;
    [field: SerializeField] public string Description;

    public void Setup(LaneDisplay _laneDisplay)
    {
        foreach (var _ability in abilities)
        {
            _ability.Setup(_laneDisplay);
        }
    }
}
