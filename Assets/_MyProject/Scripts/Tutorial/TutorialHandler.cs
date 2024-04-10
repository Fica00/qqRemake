using Tutorial;
using UnityEngine;

public class TutorialHandler : MonoBehaviour
{
    [SerializeField] private LogoAnimation logoAnimation;
    [SerializeField] private MatchMaking matchMaking;

    private void Start()
    {
        // DataManager.Instance.PlayerData.HasFinishedTutorial = 1;
        logoAnimation.Setup(ShowMatchUp);
    }

    private void ShowMatchUp()
    {
        matchMaking.Setup(ShowTutorial);
    }

    private void ShowTutorial()
    {
        SceneManager.Instance.LoadTutorialGameplay(false);
    }
}
