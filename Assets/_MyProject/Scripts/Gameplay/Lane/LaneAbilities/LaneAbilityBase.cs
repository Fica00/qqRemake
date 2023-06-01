using UnityEngine;

public class LaneAbilityBase : MonoBehaviour
{
    protected LaneDisplay laneDisplay;

    public void Setup(LaneDisplay _laneDisplay)
    {
        laneDisplay = _laneDisplay;
        Subscribe();
    }

    public virtual void Subscribe()
    {
        throw new System.Exception("Subscribe virtual method is not being implemented");
    }
}
