using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class QoomonAnimator : MonoBehaviour
{
    [SerializeField] private SkeletonGraphic animator;
    [SerializeField] private string revealKey = "dengchang";
    [SerializeField] private string increasedPowerKey = "buff";
    [SerializeField] private string decreasedPowerKey = "debuff";
    [SerializeField] private string idleKey = "daiji";
    [SerializeField] private List<AnimationHelper> animationHelpers = new ();
    [SerializeField] private float revealDelay;
    private CardObject cardObject;

    public bool HasAnimations => cardObject != null;
    [HideInInspector]public bool IsRevealAnimationDone;
    private string currentAnimation;
    
    public void Setup(CardObject _cardObject)
    {
        if (animator==null)
        {
            Debug.Log("No animator attached"+ _cardObject.Details.Name,gameObject);
            return;
        }

        cardObject = _cardObject;
        cardObject.Stats.UpdatedPowerBasedOnPrevious += CheckPower;
        if (!cardObject.IsMy)
        {
            animator.transform.eulerAngles = Vector3.zero;
        }
    }

    private void OnDisable()
    {
        if (cardObject ==null)
        {
            return;
        }
        cardObject.Stats.UpdatedPowerBasedOnPrevious -= CheckPower;
    }

    private void CheckPower(ChangeStatus _status)
    {
        switch (_status)
        {
            case ChangeStatus.Same:
                break;
            case ChangeStatus.Increased:
                PlayWinAnimation();
                break;
            case ChangeStatus.Decreased:
                PlayLoseAnimation();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_status), _status, null);
        }
    }
    
    public IEnumerator RevealAnimation()
    {
        cardObject.Reveal.PreReveal();
        IsRevealAnimationDone = false;
        PlayAnimation(revealKey,false);
        yield return new WaitUntil(() => IsRevealAnimationDone);
        cardObject.Reveal.Finish();
        cardObject.Subscribe();
    }

    public void PlayWinAnimation()
    {
        PlayAnimation(increasedPowerKey, false);
    }

    public void PlayLoseAnimation()
    {
        PlayAnimation(decreasedPowerKey, false);
    }
    

    private void PlayAnimation(string _animationKey, bool _loop, bool _playIdleOnEnd=true)
    {
        if (animator==null)
        {
            return;
        }
        
        animator.gameObject.SetActive(true);
        AnimationHelper _animationHelper = animationHelpers.Find(_animationData => _animationData.AnimationKey == _animationKey);
        if (_animationHelper!=null)
        {
            foreach (var _object in _animationHelper.Objects)
            {
                _object.SetActive(true);
            }
        }

        StartCoroutine(DoPlayAnimation());
        IEnumerator DoPlayAnimation()
        {
            if (_animationKey == revealKey)
            {
                yield return new WaitForSeconds(revealDelay);
            }
            
            var _animation = animator.AnimationState.SetAnimation(0, _animationKey, _loop);
            if (_playIdleOnEnd)
            {
                _animation.Complete += PlayIdle;
            }
        }
    }
    
    private void PlayIdle(TrackEntry _trackEntry)
    {
        if (_trackEntry.Animation.Name==revealKey)
        {
            IsRevealAnimationDone = true;
        }
        
        animator.AnimationState.SetAnimation(0, idleKey, true);
    }

}
