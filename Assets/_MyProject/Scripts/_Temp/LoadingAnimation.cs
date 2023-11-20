using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingDisplay;
    private float timeBetweenAnimationFrames = 0.35f;

    private IEnumerator Start()
    {
        int _counter = 0;
        string _text = "Loading";
        while (gameObject.activeSelf)
        {
            string _output = _text;
            for (int _i = 0; _i < _counter; _i++)
            {
                _output += ".";
            }

            loadingDisplay.text = _output;
            _counter++;
            yield return new WaitForSeconds(timeBetweenAnimationFrames);
            if (_counter>3)
            {
                _counter = 0;
            }
        }
    }
}
