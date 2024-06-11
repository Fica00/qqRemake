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
    private Spine.AnimationState state;

    public bool HasAnimations => cardObject != null;
    private bool isRevealAnimationDone;
    private string currentAnimation;
    
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

        state = animator.AnimationState;
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

    private void CheckPower(ChangeStatus _status)
    {
        switch (_status)
        {
            case ChangeStatus.Same:
                break;
            case ChangeStatus.Increased:
                state.SetAnimation(0,increasedPowerKey, false);
                break;
            case ChangeStatus.Decreased:
                state.SetAnimation(0,decreasedPowerKey, false);
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
            isRevealAnimationDone = true;
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
