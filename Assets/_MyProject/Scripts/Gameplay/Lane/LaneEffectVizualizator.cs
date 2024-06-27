using UnityEngine;
using UnityEngine.EventSystems;

public class LaneEffectVizualizator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private LaneVizualizator vizualizator;
    private CardObject trackedQoomon;

    private void OnEnable()
    {
        CardInteractions.DragStarted += TrackQoomon;
        CardInteractions.DragEnded += EndTracking;
    }

    private void OnDisable()
    {
        CardInteractions.DragStarted -= TrackQoomon;
        CardInteractions.DragEnded -= EndTracking;
    }

    private void TrackQoomon(CardObject _qoomon)
    {
        trackedQoomon = _qoomon;
    }
    
    private void EndTracking()
    {
        trackedQoomon = null;
    }

    public void OnPointerEnter(PointerEventData _eventData)
    {
        if (trackedQoomon==null)
        {
            return;
        }

        vizualizator.HandleAnimationObject(true, LaneVizualizatorTrigger.Drag);
    }

    public void OnPointerExit(PointerEventData _)
    {
        vizualizator.HandleAnimationObject(false);
    }
}
