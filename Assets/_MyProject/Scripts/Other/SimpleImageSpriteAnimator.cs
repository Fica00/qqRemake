using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SimpleImageSpriteAnimator : MonoBehaviour
{
    [SerializeField] float timeBetweenFrames;
    [SerializeField] Image animatedImage;
    [SerializeField] Sprite[] sprites;
    [SerializeField] bool setNativeSize;

    void Start()
    {
        StartCoroutine(AnimateRoutine());
    }

    IEnumerator AnimateRoutine()
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
