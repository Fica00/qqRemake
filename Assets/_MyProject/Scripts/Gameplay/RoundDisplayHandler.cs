using UnityEngine;
using TMPro;

public class RoundDisplayHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI roundDisplay;

    private void OnEnable()
    {
        GameplayManager.UpdatedRound += ShowRound;
    }

    private void OnDestroy()
    {
        GameplayManager.UpdatedRound -= ShowRound;
    }

    void ShowRound()
    {
        roundDisplay.text = "Turn: " + GameplayManager.Instance.CurrentRound;
    }
}
