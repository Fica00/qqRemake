public static class SceneManager
{
    const string MAIN_MENU = "MainMenu";
    const string GAMEPLAYPVP = "GameplayPVP";
    const string GAMEPLAYAI = "GameplayAI";

    public static void LoadMainMenu()
    {
        LoadScene(MAIN_MENU);
    }

    public static void LoadPVPGameplay()
    {
        LoadScene(GAMEPLAYPVP);
    }

    public static void LoadAIGameplay()
    {
        LoadScene(GAMEPLAYAI);
    }

    static void LoadScene(string _key)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_key);
    }
}
