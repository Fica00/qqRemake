using System;
using UnityEngine;

public class LaneAbilityBase : MonoBehaviour
{
    public static Action<LaneDisplay> OnActivated;
    protected LaneDisplay laneDisplay;
    protected bool isSubscribed;

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
