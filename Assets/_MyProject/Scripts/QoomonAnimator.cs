using System;
using System.Collections;
using UnityEngine;

public class QoomonAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string revealKey = "dengchang";
    [SerializeField] private string increasedPowerKey = "buff";
    [SerializeField] private string decreasedPowerKey = "debuff";
    [SerializeField] private bool useNewAnimation;
    private CardObject cardObject;

    public bool HasAnimations => cardObject != null;
    private bool isRevealAnimationDone;
    
    public void Setup(CardObject _cardObject)
    {
        // if (animator==null)
        // {
        //     return;
        // }
        if (!useNewAnimation)
        {
            return;
        }

        cardObject = _cardObject;
        cardObject.Stats.UpdatedPowerBasedOnPrevious += CheckPower;
    }

    private void OnDisable()
    {
        if (cardObject ==null)
        {
            return;
        }
        cardObject.Stats.UpdatedPowerBasedOnPrevious -= CheckPower;
    }

    private void CheckForSpawnAnimation(CardObject _card)
    {
        if (_card!=cardObject)
        {
            return;
        }
        
        animator.Play(revealKey);
    }
    
    private void CheckPower(ChangeStatus _status)
    {
        switch (_status)
        {
            case ChangeStatus.Same:
                break;
            case ChangeStatus.Increased:
                animator.Play(increasedPowerKey);
                break;
            case ChangeStatus.Decreased:
                animator.Play(decreasedPowerKey);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_status), _status, null);
        }
    }

    public IEnumerator RevealAnimation()
    {
        cardObject.Reveal.PreReveal();
        isRevealAnimationDone = false;
        yield return new WaitUntil(() => isRevealAnimationDone);
        cardObject.Reveal.Finish();
        cardObject.Subscribe();
    }

    private void EndRevealAnimation()
    {
        isRevealAnimationDone = true;
    }
}
