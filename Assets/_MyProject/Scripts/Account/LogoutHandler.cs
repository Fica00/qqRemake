using UnityEngine;
using UnityEngine.UI;

public class LogoutHandler : MonoBehaviour
{
    [SerializeField] private Button logoutButton;

    private void OnEnable()
    {
        logoutButton.onClick.AddListener(Logout);
    }

    private void OnDisable()
    {
        logoutButton.onClick.RemoveListener(Logout);
    }

    private void Logout()
    {
        PlayerPrefs.DeleteAll();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
       JavaScriptManager.Instance.LoadURL(JavaScriptManager.GAME_LINK);
#endif
    }
}
