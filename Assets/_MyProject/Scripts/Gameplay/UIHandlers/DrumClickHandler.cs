using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DrumClickHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI betDisplay;
    [SerializeField] TextMeshProUGUI nextBetDisplay;
    [SerializeField] GameObject soundWawe;

    Button button;
    int timeCanIncreaseBet = 1;
    int maxBet = 16;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(IncreaseBet);
        GameplayManager.UpdatedBet += ShowBet;
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(IncreaseBet);
        GameplayManager.UpdatedBet -= ShowBet;
    }

    private void Start()
    {
        ShowBet();
    }

    void ShowBet()
    {
        int _betAmount = GameplayManager.Instance.CurrentBet;
        betDisplay.text = _betAmount < 10 ? "0 " + _betAmount : "1" + (_betAmount - 10);
        nextBetDisplay.text = _betAmount == maxBet ? "MAX" : "Next: " + (_betAmount * 2);
    }

    void IncreaseBet()
    {
        if (timeCanIncreaseBet == 0)
        {
            return;
        }

        timeCanIncreaseBet--;
        GameplayManager.Instance.IncreaseBet();
        GameplayManager.Instance.MyPlayerDisplay.RemoveGlow();
        if (timeCanIncreaseBet==0)
        {
            soundWawe.SetActive(false);
        }
    }
}
