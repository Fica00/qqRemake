using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SimpleImageSpriteAnimator : MonoBehaviour
{
    [SerializeField] private float timeBetweenFrames;
    [SerializeField] private Image animatedImage;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private bool setNativeSize;

    private void Start()
    {
        StartCoroutine(AnimateRoutine());
    }

    private IEnumerator AnimateRoutine()
    {
        int _counter = 0;
        while (gameObject.activeSelf)
        {
            animatedImage.sprite = sprites[_counter];
            if (setNativeSize)
            {
                animatedImage.SetNativeSize();
            }
            _counter++;
            if (_counter>=sprites.Length)
            {
                _counter = 0;
            }

            yield return new WaitForSeconds(timeBetweenFrames);
        }
    }
}
