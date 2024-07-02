using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPVPPanel : MonoBehaviour
{
    [SerializeField] private Button cancelButton;
    [SerializeField] private MatchMakingPlayerDisplay myPlayer;
    [SerializeField] private MatchMakingPlayerDisplay opponentPlayer;
    [SerializeField] private GameObject matchingLabel;
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private UIPlayPanel playPanel;
    private IEnumerator botRoutine;
    
    public void Setup()
    {
        AudioManager.Instance.ChangeBackgroundMusic(AudioManager.MATCHMAKING);
        matchingLabel.SetActive(true);
        opponentPlayer.gameObject.SetActive(false);
        ManageInteractables(true);
        myPlayer.Setup(DataManager.Instance.PlayerData.Name, DataManager.Instance.PlayerData.GetSelectedDeck().Name);
        header.text = "Searching for opponent";
        gameObject.SetActive(true);
        TryShowTransition();
    }

    
    private void StartVsBot()
    {
        ModeHandler.Instance.Mode = GameMode.VsAi;
        playPanel.BringBot();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        ManageInteractables(true);

        cancelButton.onClick.AddListener(Cancel);
    }

    private void OnDisable()
    {
        cancelButton.onClick.RemoveListener(Cancel);
    }

    private void TryShowTransition()
    {

    }

    private void Cancel()
    {
        ManageInteractables(false);
    }

    private void Close()
    {
        AudioManager.Instance.ChangeBackgroundMusic(AudioManager.MAIN_MENU);
        if (botRoutine!=default)
        {
            StopCoroutine(botRoutine);
            botRoutine = default;
        }
        gameObject.SetActive(false);
    }

    private void OpponentJoined()
    {
        ManageInteractables(false);
        ShowOpponent();
        LoadGameplay();
    }

    private void ShowOpponent()
    {
        opponentPlayer.gameObject.SetActive(true);
        header.text = "Opponent found!";
    }

    private void LoadGameplay()
    {
        
    }

    private void ManageInteractables(bool _status)
    {
        cancelButton.interactable = _status;
    }
    
}
