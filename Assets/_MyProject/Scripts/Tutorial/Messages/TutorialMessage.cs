using UnityEngine;

namespace Tutorial
{
    public class TutorialMessage : MonoBehaviour
    {
        public virtual void Setup()
        {
            gameObject.SetActive(true);
        }
    }
}