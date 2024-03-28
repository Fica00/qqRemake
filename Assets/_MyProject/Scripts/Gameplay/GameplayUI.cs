using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    public static GameplayUI Instance;
    [field: SerializeField] public GameplayYesNo YesNoDialog { get; private set; }
    [field: SerializeField] private ResultHandler resultHandler;
    [SerializeField] private GameObject[] topHud;
    [SerializeField] private GameObject[] bottomHud;
    [SerializeField] private GameObject[] topLane;
    [SerializeField] private GameObject[] midLane;
    [SerializeField] private GameObject[] botLane;
    [SerializeField] private TransitionAnimation transitionAnimation;

    private Action initialAnimationCallBack;
    

    private void Awake()
    {
        Instance = this;
        MinimizeTopAndBottomHudElements();
    }

    private void MinimizeTopAndBottomHudElements()
    {
        MinimizeHud(topHud);
        MinimizeHud(bottomHud);
        MinimizeHud(topLane);
        MinimizeHud(botLane);
        MinimizeHud(midLane);
    
        void MinimizeHud(GameObject[] _objects)
        {
            foreach (var _gameObject in _objects)
            {
                _gameObject.transform.localScale = Vector3.zero;
            }
        }
    }

    private void OnEnable()
    {
        GameplayManager.GameEnded += ShowResult;
    }

    private void OnDisable()
    {
        GameplayManager.GameEnded -= ShowResult;
    }

    private void ShowResult(GameResult _result)
    {
        StartCoroutine(ShowResultRoutine(_result));
    }

    private IEnumerator ShowResultRoutine(GameResult _result)
    {
        yield return new WaitForSeconds(0.2f);
        resultHandler.Show(_result);
    }

    public void StartingAnimations(Action _callBack)
    {
        transitionAnimation.EndTransition(AnimateTopAndBotHud);
        initialAnimationCallBack = _callBack;
    }
    
    public void ClosingAnimation(Action _callBack)
    {
        transitionAnimation.StartTransition(_callBack);
    }

    private void AnimateTopAndBotHud()
    {
        StartCoroutine(StartingAnimationsRoutine());
        IEnumerator StartingAnimationsRoutine()
        {
            float _duration = 1;
            ScaleUpObjects(topHud,_duration);
            yield return new WaitForSeconds(_duration);
            ScaleUpObjects(bottomHud,_duration);
            yield return new WaitForSeconds(_duration);
            ScaleUpObjects(topLane,_duration);
            ScaleUpObjects(midLane,_duration);
            ScaleUpObjects(botLane,_duration);
            yield return new WaitForSeconds(_duration);
            initialAnimationCallBack?.Invoke();
        }

        void ScaleUpObjects(GameObject[] _objects,float _duration)
        {
            foreach (var _gameObject in _objects)
            {
                _gameObject.transform.DOScale(Vector3.one, _duration);
            }
        }
    }
}
