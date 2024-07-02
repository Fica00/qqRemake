using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingAnimation : MonoBehaviour
{
    [SerializeField] private Image loadingDisplay;
    [SerializeField] private Sprite[] animationSprites;
    
    [SerializeField] private float timeBetweenAnimationFrames;

    private IEnumerator Start()
    {
        while (gameObject.activeSelf)
        {
            foreach (var _animationSprite in animationSprites)
            {
                loadingDisplay.sprite = _animationSprite;
                yield return new WaitForSeconds(timeBetweenAnimationFrames);
            }
        }
    }
}
