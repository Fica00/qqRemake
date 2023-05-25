using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RoundAnimationPanel : MonoBehaviour
{
    [SerializeField] GameObject panelHolder;
    [SerializeField] GameObject turnHolder;
    [SerializeField] Sprite[] turnImages;
    [SerializeField] Image turnDisplay;
    float startPositionX;

    private void OnEnable()
    {
        startPositionX = turnHolder.transform.localPosition.x;
        GameplayManager.UpdatedRound += ShowTurn;
    }

    private void OnDisable()
    {
        GameplayManager.UpdatedRound -= ShowTurn;
    }

    void ShowTurn()
    {
        int _index = GameplayManager.Instance.CurrentRound - 1;
        if (_index < 0)
        {
            return;
        }
        panelHolder.SetActive(true);
        turnDisplay.sprite = turnImages[_index];
        Sequence _sequence = DOTween.Sequence();

        _sequence.Append(turnHolder.transform.DOLocalMove(Vector3.zero, 0.2f));
        Vector3 _endPosition = turnHolder.transform.localPosition;
        _endPosition.x *= -1;
        _sequence.Append(turnHolder.transform.DOLocalMove(_endPosition, 0.2f).SetDelay(0.5f));
        _sequence.OnComplete(() =>
        {
            turnHolder.transform.localPosition = new Vector3(startPositionX, turnHolder.transform.localPosition.y, turnHolder.transform.localPosition.z);
            panelHolder.SetActive(false);
        });
        _sequence.Play();
    }
}
