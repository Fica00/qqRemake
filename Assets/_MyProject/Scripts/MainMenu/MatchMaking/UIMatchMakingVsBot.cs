using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMatchMakingVsBot : MonoBehaviour
{
    [SerializeField] private Button cancelButton;
    [SerializeField] private MatchMakingPlayerDisplay myPlayer;
    [SerializeField] private MatchMakingPlayerDisplay opponentPlayer;
    [SerializeField] private GameObject matchingLabel;
    [SerializeField] private TextMeshProUGUI header;
    
    public void Setup(float _waitTime)
    {
        matchingLabel.SetActive(true);
        opponentPlayer.gameObject.SetActive(false);
        ManageInteractables(true);
        myPlayer.Setup(DataManager.Instance.PlayerData.Name, DataManager.Instance.PlayerData.GetSelectedDeck().Name);
        header.text = "Searching for opponent";
        gameObject.SetActive(true);

        StartCoroutine(BringBot(_waitTime));
    }

    private IEnumerator BringBot(float _waitTime)
    {
        yield return new WaitForSeconds(1);
        AudioManager.Instance.ChangeBackgroundMusic(AudioManager.MATCHMAKING);
        yield return new WaitForSeconds(_waitTime);
        BotPlayer.GenerateNewData();
        opponentPlayer.Setup(BotPlayer.Name, BotPlayer.DeckName);
        opponentPlayer.gameObject.SetActive(true);
        header.text = "Opponent found!";
        yield return new WaitForSeconds(2);
        UIMainMenu.Instance.ShowSceneTransition(() => { SceneManager.Instance.LoadAIGameplay(false);});
    }

    private void OnEnable()
    {
        ManageInteractables(true);

        cancelButton.onClick.AddListener(Cancel);

    }

    private void OnDisable()
    {
        cancelButton.onClick.RemoveListener(Cancel);
    }

    private void Cancel()
    {
        ManageInteractables(false);
        StopAllCoroutines();
        gameObject.SetActive(false);
        AudioManager.Instance.ChangeBackgroundMusic(AudioManager.MAIN_MENU);
    }

    private void ManageInteractables(bool _status)
    {
        cancelButton.interactable = _status;
    }
}
