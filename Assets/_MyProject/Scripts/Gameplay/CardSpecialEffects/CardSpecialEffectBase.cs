using System.Collections;
using UnityEngine;

public class CardSpecialEffectBase : MonoBehaviour
{
    protected CardObject cardObject;

    public void Setup(CardObject _cardObject)
    {
        cardObject = _cardObject;
    }

    public virtual void Subscribe()
    {
        throw new System.Exception("Subscribe virtual method is not being implemented");
    }

    public virtual IEnumerator Use()
    {
        throw new System.Exception("Use virtual methos is not being implemented");
    }
}
