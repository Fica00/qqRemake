using UnityEngine;
using UnityEngine.UI;

public class LanePlaceIdentifier : MonoBehaviour
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public LaneLocation Location { get; private set; }
    [field: SerializeField] public bool IsMine { get; private set; }

    LaneDisplay laneDisplay;
    Image image;

    private void Awake()
    {
        laneDisplay = GetComponentInParent<LaneDisplay>();
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        CardInteractions.DragStarted += CheckIfTileIsAvailable;
        CardInteractions.DragEnded += TurnOffAvailableColor;
    }

    private void OnDisable()
    {
        CardInteractions.DragStarted -= CheckIfTileIsAvailable;
        CardInteractions.DragEnded -= TurnOffAvailableColor;
    }

    void CheckIfTileIsAvailable(CardObject _cardObject)
    {
        if (!IsMine)
        {
            return;
        }

        if (!CanPlace())
        {
            return;
        }

        if (!laneDisplay.CanPlace(_cardObject))
        {
            return;
        }

        Color _color = image.color;
        _color.a = 0.4f;
        image.color = _color;
    }

    void TurnOffAvailableColor()
    {
        Color _color = image.color;
        _color.a = 0f;
        image.color = _color;
    }

    public bool CanPlace()
    {
        foreach (Transform _child in transform)
        {
            CardObject _cardObject = _child.GetComponent<CardObject>();
            if (_cardObject != null)
            {
                return false;
            }
        }
        return true;
    }
}
