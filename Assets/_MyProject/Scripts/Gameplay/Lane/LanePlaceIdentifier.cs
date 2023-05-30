using UnityEngine;

public class LanePlaceIdentifier : MonoBehaviour
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public LaneLocation Location { get; private set; }
    [field: SerializeField] public bool IsMine { get; private set; }

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
