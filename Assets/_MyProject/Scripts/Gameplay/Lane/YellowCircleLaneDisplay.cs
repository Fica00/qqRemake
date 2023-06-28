using UnityEngine;
using UnityEngine.UI;

public class YellowCircleLaneDisplay : MonoBehaviour
{
    [SerializeField] private bool isMy;
    private LaneLocation location;
    private Image image;

    private void Awake()
    {
        location = GetComponentInParent<LaneDisplay>().Location;
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        TableHandler.OnRevealdCard += CheckCard;
    }

    private void OnDisable()
    {
        TableHandler.OnRevealdCard -= CheckCard;
    }

    private void CheckCard(CardObject _cardObject)
    {
        if (_cardObject.LaneLocation != location)
        {
            return;
        }
        if (_cardObject.IsMy!=isMy)
        {
            return;
        }

        foreach (var _effect in CardsManager.Instance.GetCardEffects(_cardObject.Details.Id))
        {
            if (_effect is CardEffectDoublePowerOnCurrentLane)
            {
                Color _color = image.color;
                _color.a = 1;
                image.color = _color;
            }
        }
    }

}
