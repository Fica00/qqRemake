using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AccessCodeValidator : MonoBehaviour
{
    private const string HAS_VERIFIED = "hasVerified";
    [SerializeField] private GameObject holder;
    [SerializeField] private InputField accessCode;
    [SerializeField] private string validAccessCode;
    [SerializeField] private Button validate;
    [SerializeField] private GameObject wrongAccessCode;

    private void Start()
    {
        if (PlayerPrefs.HasKey(HAS_VERIFIED))
        {
            SceneManager.Instance.LoadDataCollector();
        }
        else
        {
            holder.SetActive(true);
        }
    }

    private void OnEnable()
    {
        validate.onClick.AddListener(Validate);
    }

    private void OnDisable()
    {
        validate.onClick.RemoveListener(Validate);
    }

    private void Validate()
    {
        if (String.Equals(accessCode.text, validAccessCode, StringComparison.CurrentCultureIgnoreCase))
        {
            PlayerPrefs.SetInt(HAS_VERIFIED,1);
            SceneManager.Instance.LoadDataCollector();
        }
        else
        {
            StartCoroutine(ShowWrong());
        }
    }

    private IEnumerator ShowWrong()
    {
        wrongAccessCode.SetActive(true);
        yield return new WaitForSeconds(1);
        wrongAccessCode.SetActive(false);
    }
}
