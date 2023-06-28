using System.Collections;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    public static GameplayUI Instance;
    [field: SerializeField] public GameplayYesNo YesNoDialog { get; private set; }

    private void Awake()
    {
        Instance = this;
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
        string _resultText = string.Empty;
        string _fontKey = string.Empty;
        switch (_result)
        {
            case GameResult.IWon:
                _resultText = "You won!";
                _fontKey = GameplayYesNo.FONT_GREEN;
                break;
            case GameResult.ILost:
                _resultText = "You lost!";
                _fontKey = GameplayYesNo.FONT_RED;
                break;
            case GameResult.Draw:
                _resultText = "Tied!";
                _fontKey = GameplayYesNo.FONT_RED;
                break;
            case GameResult.IForefiet:
                _resultText = "Escaped";
                _fontKey = GameplayYesNo.FONT_RED;
                break;
            default:
                break;
        }

        YesNoDialog.Setup(_resultText, _fontKey);
    }
}
