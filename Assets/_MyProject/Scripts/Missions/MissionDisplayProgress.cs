using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class MissionDisplayProgress : MonoBehaviour
{
    [SerializeField] private MissionDisplaySmall displayPrefab;
    [SerializeField] private Transform missionsHolder;
    
    private static List<MissionProgress> progresses;

    private void Start()
    {
        CheckForUpdates();
        progresses = JsonConvert.DeserializeObject<List<MissionProgress>>(JsonConvert.SerializeObject(DataManager.Instance.PlayerData.MissionsProgress));
    }

    private void CheckForUpdates()
    {
        if (progresses==default)
        {
            return;
        }

        int _counter = 1;
        foreach (var _mission in DataManager.Instance.PlayerData.MissionsProgress)
        {
            MissionProgress _savedProgress = GetSavedProgress(_mission.Id);
            if (_savedProgress == default)
            {
                continue;
            }

            if (_savedProgress.Value >= _mission.Value)
            {
                continue;
            }

            MissionDisplaySmall _display = Instantiate(displayPrefab, missionsHolder);
            _display.Setup(_mission);
            StartCoroutine(DestroyShownMission(_display.gameObject, _counter));
            _counter++;
        }
    }

    private MissionProgress GetSavedProgress(int _missionId)
    {
        return progresses.Find(_mission => _mission.Id == _missionId);
    }

    private IEnumerator DestroyShownMission(GameObject _gameObject, int _counter)
    {
        yield return new WaitForSeconds(_counter);
        if (_gameObject)
        {
            Destroy(_gameObject);
        }
    }
}
