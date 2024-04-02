using SceneManagement;

namespace Demo
{
    public class SceneManager : SceneLoader
    {
        public static SceneManager Instance;
        private const string DEMO2 = "Demo2";

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

        private void Start()
        {
            CapLoadingScenePercentage = 50;
        }

        public void LoadNextScene()
        {
            LoadScene(DEMO2,true);
        }
    }
}