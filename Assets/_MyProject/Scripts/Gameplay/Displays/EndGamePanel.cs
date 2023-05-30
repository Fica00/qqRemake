using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI resultDisplay;
    [SerializeField] Button closeButton;
    [SerializeField] GameObject holder;

    private void OnEnable()
    {
        GameplayManager.GameEnded += ShowResult;
        closeButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        GameplayManager.GameEnded -= ShowResult;
        closeButton.onClick.RemoveListener(Close);
    }

    void ShowResult(GameResult _result)
    {
        switch (_result)
        {
            case GameResult.IWon:
                resultDisplay.text = "Congratzz!\nYou Won!";
                break;
            case GameResult.ILost:
                resultDisplay.text = "Better luck next time!\nYou Lost!";
                break;
            case GameResult.Draw:
                resultDisplay.text = "Hard one!\nTied!";
                break;
            default:
                break;
        }

        holder.SetActive(true);
    }

    void Close()
    {
        SceneManager.LoadMainMenu();
    }
}
