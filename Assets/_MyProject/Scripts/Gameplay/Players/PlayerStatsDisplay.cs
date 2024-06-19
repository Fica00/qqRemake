using System;
using TMPro;
using UnityEngine;

public class PlayerStatsDisplay : MonoBehaviour
{
    public static Action OnPlayerStatsClose;
    public static Action OnPlayerNameClicked;
    
    [SerializeField] private TextMeshProUGUI cardsInHand;
    [SerializeField] private TextMeshProUGUI discarded;
    [SerializeField] private TextMeshProUGUI collection;
    [SerializeField] private TextMeshProUGUI destroyed;
    
    
    public bool IsOpen;

    public void Show(int _cardsInHand, int _discard, int _cardsInDeck, int _destroyed)
    {
        OnPlayerNameClicked?.Invoke();
        Debug.Log("Stats Open");
        gameObject.SetActive(true);
        cardsInHand.text = _cardsInHand.ToString();
        discarded.text = _discard.ToString();
        collection.text = _cardsInDeck.ToString();
        destroyed.text = _destroyed.ToString();
        IsOpen = true;
        
    }
    
    public void Close()
    {
        Debug.Log("Stats Close");
       
        gameObject.SetActive(false);
        IsOpen = false;
        OnPlayerStatsClose?.Invoke();
    }
}
