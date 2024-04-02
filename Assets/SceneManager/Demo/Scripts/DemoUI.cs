using UnityEngine;
using UnityEngine.UI;

namespace Demo
{
    public class DemoUI : MonoBehaviour
    {
        [SerializeField] private Button nextScene;

        private void OnEnable()
        {
            nextScene.onClick.AddListener(LoadNextScene);
        }

        private void OnDisable()
        {
            nextScene.onClick.RemoveListener(LoadNextScene);
        }

        private void LoadNextScene()
        {
            SceneManager.Instance.LoadNextScene();
        }
    }

}
