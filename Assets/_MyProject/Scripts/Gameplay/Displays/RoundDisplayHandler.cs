using UnityEngine;
using TMPro;

public class RoundDisplayHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roundDisplay;

    private void OnEnable()
    {
        GameplayManager.UpdatedRound += ShowRound;
    }

    private void OnDestroy()
    {
        GameplayManager.UpdatedRound -= ShowRound;
    }

    private void ShowRound()
    {
        roundDisplay.text = GameplayManager.Instance.CurrentRound+"/"+GameplayManager.Instance.MaxAmountOfRounds;
    }
}
