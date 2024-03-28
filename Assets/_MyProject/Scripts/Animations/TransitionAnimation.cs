using System;
using System.Collections;
using UnityEngine;

public class TransitionAnimation : MonoBehaviour
{
    private const string STARTING_ANIMATION_KEY = "Part1";
    private const string ENDING_ANIMATION_KEY = "Part2";
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject ripple;

    public void StartTransition(Action _callBack)
    {
        animator.gameObject.SetActive(true);
        animator.SetTrigger(STARTING_ANIMATION_KEY);
        ripple.SetActive(false);
        StartCoroutine(CallCallBack(2.5f, _callBack));
    }

    public void EndTransition(Action _callBack)
    {
        animator.gameObject.SetActive(true);
        animator.SetTrigger(ENDING_ANIMATION_KEY);
        ripple.SetActive(false);
        StartCoroutine(CallCallBack(2.5f, () =>
        {
            gameObject.SetActive(false);
            _callBack?.Invoke();
        }));
    }

    IEnumerator CallCallBack(float _duration,Action _callBack)
    {
        yield return new WaitForSeconds(_duration);
        _callBack?.Invoke();
    }
}
