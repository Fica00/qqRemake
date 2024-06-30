using SceneManagement;
using UnityEngine;

public class SceneManager : SceneLoader
{
    public static SceneManager Instance;
    [SerializeField] private bool useAsync;
    private const string MAIN_MENU = "MainMenu";
    private const string LEVEL_PAGE = "LevelPage";
    private const string SETTINGS_PAGE = "SettingsPage";
    private const string COLLECTION_PAGE = "CollectionPage";
    private const string RANK_REWARDS_PAGE = "RankPage";
    private const string MISSIONS_PAGE = "MissionsPage";
    private const string GAMEPLAY_PVP = "GameplayPVP";
    private const string GAMEPLAY_AI = "GameplayAI";
    private const string DATA_COLLECTOR = "DataCollector";
    private const string ALPHA_CODE = "AlphaCode";
    private const string TUTORIAL = "Tutorial";
    private const string SIMPLE_TUTORIAL = "SimpleTutorial";
    private const string GAMEPLAY_TUTORIAL = "GameplayTutorial";
    private const string MAIN_MENU_TUTORIAL = "MainMenuTutorial";

    public static bool IsAIScene => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == GAMEPLAY_AI;
    public static bool IsGameplayTutorialScene => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == GAMEPLAY_TUTORIAL;
    public static bool IsMainMenuTutorialScene => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == MAIN_MENU_TUTORIAL;
    public static bool IsAuthScene => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == DATA_COLLECTOR;
   
    public static string CurrentSceneName => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadMainMenu(bool _useAsyncLoading=true)
    {
        DoLoadScene(MAIN_MENU,_useAsyncLoading);
    }    
    
    public void LoadLevelPage(bool _useAsyncLoading=true)
    {
        DoLoadScene(LEVEL_PAGE,_useAsyncLoading);
    }     
    
    public void LoadMainMenuTutorial(bool _useAsyncLoading=true)
    {
        DoLoadScene(MAIN_MENU_TUTORIAL,_useAsyncLoading);
    }   
    
    public void LoadMissionsPage(bool _useAsyncLoading=true)
    {
        DoLoadScene(MISSIONS_PAGE,_useAsyncLoading);
    }       
    
    public void LoadRankRewardsPage(bool _useAsyncLoading=true)
    {
        DoLoadScene(RANK_REWARDS_PAGE,_useAsyncLoading);
    }    
    
    public void LoadSettingsPage(bool _useAsyncLoading=true)
    {
        DoLoadScene(SETTINGS_PAGE,_useAsyncLoading);
    }    
    
    public void LoadCollectionPage(bool _useAsyncLoading=true)
    {
        DoLoadScene(COLLECTION_PAGE,_useAsyncLoading);
    }

    public void LoadAlphaCode(bool _useAsyncLoading=true)
    {
        DoLoadScene(ALPHA_CODE,_useAsyncLoading);
    }

    public void LoadPvpGameplay(bool _useAsyncLoading=true)
    {
        DoLoadScene(GAMEPLAY_PVP,_useAsyncLoading);
    }

    public void LoadAIGameplay(bool _useAsyncLoading=true)
    {
        DoLoadScene(GAMEPLAY_AI,_useAsyncLoading);
    }

    public void LoadDataCollector(bool _useAsyncLoading=true)
    {
        DoLoadScene(DATA_COLLECTOR,_useAsyncLoading);
    }

    public void LoadTutorial(bool _useAsyncLoading=true)
    {
        DoLoadScene(TUTORIAL,_useAsyncLoading);
    }    
    
    public void LoadSimpleTutorial(bool _useAsyncLoading=true)
    {
        DoLoadScene(SIMPLE_TUTORIAL,_useAsyncLoading);
    }

    public void LoadTutorialGameplay(bool _useAsyncLoading=true)
    {
        DoLoadScene(GAMEPLAY_TUTORIAL,_useAsyncLoading);
    }
    
    public void ReloadScene(bool _useAsyncLoading=true)
    {
        DoLoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,_useAsyncLoading);
    }

    private void DoLoadScene(string _key, bool _useAsyncLoading=true)
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene(_key);
        LoadScene(_key, false);
    }
}
