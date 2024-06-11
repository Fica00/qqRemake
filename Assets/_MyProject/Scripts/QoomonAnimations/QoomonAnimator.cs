using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
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
    [SerializeField] private bool useNewAnimation;
    [SerializeField] private List<AnimationHelper> animationHelpers = new ();
    private CardObject cardObject;

    public bool HasAnimations => cardObject != null;
    [HideInInspector]public bool IsRevealAnimationDone;
    private string currentAnimation;
    
    public void Setup(CardObject _cardObject)
    {
        // if (animator==null)
        // {
        //     return;
        // }
        if (!useNewAnimation)
        {
            animator.gameObject.SetActive(false);
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
                PlayAnimation(increasedPowerKey, false);
                break;
            case ChangeStatus.Decreased:
                PlayAnimation(decreasedPowerKey, false);
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



    [Button()]
    private void TestReveal()
    {
        PlayAnimation(revealKey, false);
    }

    [Button()]
    private void TestBuff()
    {
        PlayAnimation(increasedPowerKey, false);
    }
    
    [Button()]
    private void TestDeBuff()
    {
        PlayAnimation(decreasedPowerKey, false);
    }
    
    [Button()]
    private void TestIdle()
    {
        PlayAnimation(idleKey, false,false);
    }

    private void PlayAnimation(string _animationKey, bool _loop, bool _playIdleOnEnd=true)
    {
        animator.gameObject.SetActive(true);
        AnimationHelper _animationHelper = animationHelpers.Find(_animationData => _animationData.AnimationKey == _animationKey);
        if (_animationHelper!=null)
        {
            foreach (var _object in _animationHelper.Objects)
            {
                _object.SetActive(true);
            }
        }

        var _animation = animator.AnimationState.SetAnimation(0, _animationKey, _loop);
        if (_playIdleOnEnd)
        {
            _animation.Complete += PlayIdle;
            _animation.Interrupt += TryToDisableObjects;

        }
        else
        {
            _animation.Interrupt += TryToDisableObjects;
        }


    }
    
    private void PlayIdle(TrackEntry _trackEntry)
    {
        TryToDisableObjects(_trackEntry);
        animator.AnimationState.SetAnimation(0, idleKey, true);
    }
    
    private void TryToDisableObjects(TrackEntry _trackEntry)
    {
        if (_trackEntry.Animation.Name==revealKey)
        {
            IsRevealAnimationDone = true;
        }
        
        AnimationHelper _animationHelper = animationHelpers.Find(_animationData => _animationData.AnimationKey == _trackEntry.Animation.Name);
        if (_animationHelper == null)
        {
            return;
        }
        foreach (var _object in _animationHelper.Objects)
        {
            _object.SetActive(false);
        }
    }

}
