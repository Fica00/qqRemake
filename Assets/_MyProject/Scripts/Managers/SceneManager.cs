public static class SceneManager
{
    private const string MAIN_MENU = "MainMenu";
    private const string GAMEPLAYPVP = "GameplayPVP";
    private const string GAMEPLAYAI = "GameplayAI";
    private const string DATA_COLLECTOR = "DataCollector";

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

    public static void LoadDataCollector()
    {
        LoadScene(DATA_COLLECTOR);
    }

    private static void LoadScene(string _key)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_key);
    }
}
