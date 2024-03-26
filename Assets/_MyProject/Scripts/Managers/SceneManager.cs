public static class SceneManager
{
    private const string MAIN_MENU = "MainMenu";
    private const string GAMEPLAYPVP = "GameplayPVP";
    private const string GAMEPLAYAI = "GameplayAI";
    private const string DATA_COLLECTOR = "DataCollector";
    private const string ALPHA_CODE = "AlphaCode";

    public static bool IsAIScene => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == GAMEPLAYAI;

    public static void LoadMainMenu()
    {
        LoadScene(MAIN_MENU);
    }

    public static void LoadAlphaCode()
    {
        LoadScene(ALPHA_CODE);
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
    
    public static void ReloadScene()
    {
        LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private static void LoadScene(string _key)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_key);
    }
}
