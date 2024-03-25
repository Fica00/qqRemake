using TMPro;
using UnityEngine;

public class PlayerStatsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardsInHand;
    [SerializeField] private TextMeshProUGUI discarded;
    [SerializeField] private TextMeshProUGUI collection;
    [SerializeField] private TextMeshProUGUI destroyed;

    public bool IsOpen;

    public void Show(int _cardsInHand, int _discard, int _collection, int _destroyed)
    {
        gameObject.SetActive(true);
        cardsInHand.text = _cardsInHand.ToString();
        discarded.text = _discard.ToString();
        collection.text = _collection.ToString();
        destroyed.text = _destroyed.ToString();
        IsOpen = true;
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
        IsOpen = false;
    }
}
