using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    private const string STARTING_ANIMATION_KEY = "Part2";
    public static GameplayUI Instance;
    [field: SerializeField] public GameplayYesNo YesNoDialog { get; private set; }
    [field: SerializeField] private ResultHandler resultHandler;
    [SerializeField] private GameObject[] topHud;
    [SerializeField] private GameObject[] bottomHud;
    [SerializeField] private GameObject[] topLane;
    [SerializeField] private GameObject[] midLane;
    [SerializeField] private GameObject[] botLane;
    [SerializeField] private Animator animator;
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
        animator.gameObject.SetActive(true);
        initialAnimationCallBack = _callBack;
        animator.SetTrigger(STARTING_ANIMATION_KEY);
        StartCoroutine(AnimateRoutine());
        IEnumerator AnimateRoutine()
        {
            yield return new WaitForSeconds(2.5f);
            AnimateTopAndBotHud();
        }
    }

    private void AnimateTopAndBotHud()
    {
        animator.gameObject.SetActive(false);
        StartCoroutine(StartingAnimationsRoutine());
        IEnumerator StartingAnimationsRoutine()
        {
            float _duration = 2f;
            ScaleUpObjects(topHud,_duration);
            yield return new WaitForSeconds(_duration);
            ScaleUpObjects(bottomHud,_duration);
            yield return new WaitForSeconds(_duration);
            _duration = 1;
            ScaleUpObjects(topLane,_duration);
            yield return new WaitForSeconds(_duration);
            ScaleUpObjects(midLane,_duration);
            yield return new WaitForSeconds(_duration);
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
