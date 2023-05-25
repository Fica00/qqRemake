using UnityEngine;

public class LaneDisplay : MonoBehaviour
{
    [field: SerializeField] public LaneLocation Location { get; private set; }
    [field: SerializeField] public PowerDisplay PowerDisplay { get; private set; }
    [field: SerializeField] public LocationAbilityDisplay AbilityDisplay { get; private set; }
}
