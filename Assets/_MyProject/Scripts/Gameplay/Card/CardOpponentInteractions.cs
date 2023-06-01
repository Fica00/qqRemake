using UnityEngine.EventSystems;

public class CardOpponentInteractions : CardInteractions
{
    protected override void HandlePointerDown(PointerEventData _eventData)
    {
        OnClicked?.Invoke(cardObject);
    }
}
