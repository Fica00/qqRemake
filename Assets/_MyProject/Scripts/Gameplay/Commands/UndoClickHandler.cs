using UnityEngine;
using UnityEngine.UI;

public class UndoClickHandler : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(HandleClick);
    }

    private void HandleClick()
    {
        switch (GameplayManager.Instance.GameplayState)
        {
            case GameplayState.ResolvingBeginingOfRound:
                break;
            case GameplayState.Playing:
                GameplayManager.Instance.MyPlayer.CancelAllCommands();
                break;
            case GameplayState.Waiting:
                GameplayManager.Instance.ReturnToWaitingState();
                break;
            case GameplayState.ResolvingEndOfRound:
                break;
            case GameplayState.StartingAnimation:
                break;
            default:
                throw new System.Exception("Dont know how to handle state: " + GameplayManager.Instance.GameplayState);
        }
    }
}
