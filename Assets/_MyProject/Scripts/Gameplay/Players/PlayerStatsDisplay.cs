using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsDisplay : MonoBehaviour
{
    [SerializeField] private Button close;
    [SerializeField] private TextMeshProUGUI cardsInHand;
    [SerializeField] private TextMeshProUGUI discarded;
    [SerializeField] private TextMeshProUGUI collection;
    [SerializeField] private TextMeshProUGUI destroyed;

    private bool isOpen;

    public void Show(int _cardsInHand, int _discard, int _collection, int _destroyed)
    {
        if (isOpen)
        {
            return;
        }

        isOpen = true;
        transform.localScale=Vector3.zero;
        transform.DOScale(Vector3.one, 1).OnComplete(() =>
        {
            close.onClick.AddListener(Close);
        });
        gameObject.SetActive(true);
        cardsInHand.text = _cardsInHand.ToString();
        discarded.text = _discard.ToString();
        collection.text = _collection.ToString();
        destroyed.text = _destroyed.ToString();
    }
    
    private void Close()
    {
        close.interactable = false;
        transform.DOScale(Vector3.zero, 1).OnComplete(() =>
        {
            isOpen = false;
            gameObject.SetActive(false);
            close.interactable = true;
            close.onClick.RemoveListener(Close);
        });
    }
}
