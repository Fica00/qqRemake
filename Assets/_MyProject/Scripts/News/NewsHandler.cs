using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsHandler : MonoBehaviour
{
    [SerializeField] private List<NewsData> news = new ();
    [SerializeField] private NewsDisplay newsPrefab;
    [SerializeField] private Transform newsHolder;
    private List<NewsDisplay> shownDisplays = new();
    private int newsIndex;
    private static bool hasSeenNews;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        if (DataManager.Instance.PlayerData.HasFinishedTutorial != 1)
        {
            yield break;
        }

        if (!DataManager.Instance.PlayerData.HasPlayedFirstGame)
        {
            yield break;
        }

        if (!DataManager.Instance.CanShowPwaOverlay)
        {
            yield break;
        }

        if (hasSeenNews)
        {
            yield break;
        }

        ShowNews();
    }

    public void ShowNews()
    {
        hasSeenNews = true;
            
        SpawnDisplays();
        if (shownDisplays.Count>0)
        {
            shownDisplays[0].Show();
        }
    }

    private void SpawnDisplays()
    {
        foreach (var _newsData in news)
        {
            NewsDisplay _display = Instantiate(newsPrefab, newsHolder);
            _display.Setup(_newsData);
            _display.gameObject.SetActive(false);
            shownDisplays.Add(_display);
        }
    }

    private void OnEnable()
    {
        NewsDisplay.OnClicked += ShowNext;
    }

    private void OnDisable()
    {
        NewsDisplay.OnClicked -= ShowNext;
    }

    private void ShowNext()
    {
        newsIndex++;
        if (newsIndex>=shownDisplays.Count)
        {
            return;
        }
        
        shownDisplays[newsIndex].Show();
    }
}
