using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static Action<MissionProgress> OnClaimed;
    public static Action OnGeneratedNewChallenges;
    public static MissionManager Instance;
    private bool isSubscribed;
    private bool isSetup;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnEnable()
    {
        MissionDisplay.OnClaimPressed += ClaimMission;
    }

    private void OnDisable()
    {
        MissionDisplay.OnClaimPressed -= ClaimMission;
        UnsubscribeEvents();
    }

    private void ClaimMission(MissionProgress _missionProgress)
    {
        if (_missionProgress.Claimed)
        {
            return;
        }

        if (!_missionProgress.Completed)
        {
            return;
        }
        
        MissionData _missionData = DataManager.Instance.GameData.GetMission(_missionProgress.Id);
        _missionProgress.Claimed = true;
        MissionTaskData _missionTask = _missionProgress.IsHard ? _missionData.Hard : _missionData.Normal;
        switch (_missionTask.RewardType)
        {
            case ItemType.None:
                break;
            case ItemType.Qoomon:
                DataManager.Instance.PlayerData.AddQoomon(_missionTask.RewardAmount);
                break;
            case ItemType.Exp:
                DataManager.Instance.PlayerData.Exp += _missionTask.RewardAmount;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        DataManager.Instance.PlayerData.Statistics.NoteCheckPoint("Claimed mission reward", _checkForExisting:false);
        OnClaimed?.Invoke(_missionProgress);
    }

    public void Setup()
    {
        if (isSetup)
        {
            return;
        }

        isSetup = true;
        if (DataManager.Instance.PlayerData.MissionsProgress.Any())
        {
            SubscribeEvents();
            StartCheckForReset();
        }
        else
        {
            GenerateChallenges();
        }
    }

    private void GenerateChallenges()
    {
        GenerateNewChallenges(StartCheckForReset);
    }

    private void StartCheckForReset()
    {
        StartCoroutine(CheckForReset());
    }

    private IEnumerator CheckForReset()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (DateTime.UtcNow > DataManager.Instance.PlayerData.NextDailyChallenges)
            {
                GenerateNewChallenges(StartCheckForReset);
                yield break;
            }
        }
    }

    public TimeSpan GetResetTime()
    {
        return DataManager.Instance.PlayerData.NextDailyChallenges-DateTime.UtcNow;
    }
    
    private void GenerateNewChallenges(Action _callBack)
    {
        UnsubscribeEvents();
        DataManager.Instance.PlayerData.MissionsProgress.Clear();
        List<MissionData> _availableMission = DataManager.Instance.GameData.Missions.ToList();
        AddMissions(false,4);
        AddMissions(true,2);
        DataManager.Instance.PlayerData.NextDailyChallenges = DateTime.UtcNow.Date.AddDays(1);
        SubscribeEvents();
        OnGeneratedNewChallenges?.Invoke();
        _callBack?.Invoke();
        
        void AddMissions(bool _isHard, int _amount)
        {
            for (int _i = 0; _i < _amount; _i++)
            {
                MissionData _mission = _availableMission[UnityEngine.Random.Range(0, _availableMission.Count)];
                _availableMission.Remove(_mission);
                
                MissionProgress _progress = new MissionProgress { Id = _mission.Id, Value = 0, Claimed = false, IsHard = _isHard};
                DataManager.Instance.PlayerData.MissionsProgress.Add(_progress);
            }
        }
    }

    private void SubscribeEvents()
    {
        if (isSubscribed)
        {
            return;
        }

        isSubscribed = true;
        foreach (var _missionProgress in DataManager.Instance.PlayerData.MissionsProgress)
        {
            if (_missionProgress.Completed)
            {
                continue;
            }

            MissionData _missionData = DataManager.Instance.GameData.GetMission(_missionProgress.Id);
            switch (_missionData.Type)
            {
                case MissionType.DrawCard:
                    EventsManager.DrawCard += _missionProgress.IncreaseAmount;
                    break;
                case MissionType.PlayCard:
                    EventsManager.PlayCard += _missionProgress.IncreaseAmount;
                    break;
                case MissionType.WinALocationWithPowerLessThan100:
                    EventsManager.WinALocationWithPowerLess100 += _missionProgress.IncreaseAmount;
                    break;
                case MissionType.WinMatch:
                    EventsManager.WinMatch += _missionProgress.IncreaseAmount;
                    break;
                case MissionType.PlayCardCost1:
                    EventsManager.PlayCardCost1 += _missionProgress.IncreaseAmount;
                    break;
                case MissionType.PlayCardCost2:
                    EventsManager.PlayCardCost2 += _missionProgress.IncreaseAmount;
                    break;
                case MissionType.PlayCardCost3:
                    EventsManager.PlayCardCost3 += _missionProgress.IncreaseAmount;
                    break;
                case MissionType.PlayCardCost4:
                    EventsManager.PlayCardCost4 += _missionProgress.IncreaseAmount;
                    break;
                case MissionType.PlayCardCost5:
                    EventsManager.PlayCardCost5 += _missionProgress.IncreaseAmount;
                    break;
                case MissionType.PlayCardCost6:
                    EventsManager.PlayCardCost6 += _missionProgress.IncreaseAmount;
                    break;
                case MissionType.WinALocationWithPowerMoreThan200:
                    EventsManager.WinALocationWithPowerMore200 += _missionProgress.IncreaseAmount;
                    break;
                case MissionType.WinALocationWith1Card:
                    EventsManager.WinALocationWith1Card += _missionProgress.IncreaseAmount;
                    break;
                case MissionType.WinALocationWith4Card:
                    EventsManager.WinALocationWith4Card += _missionProgress.IncreaseAmount;
                    break;
                case MissionType.PlayCardsOfPowerWorth:
                    EventsManager.PlayCardsOfPowerWorth += _missionProgress.IncreaseAmount;
                    break;         
                case MissionType.WinMatchWithADouble:
                    EventsManager.WinMatchWithADouble += _missionProgress.IncreaseAmount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    private void UnsubscribeEvents()
    {
        if (!isSubscribed)
        {
            return;
        }

        isSubscribed = false;
        foreach (var _missionProgress in DataManager.Instance.PlayerData.MissionsProgress)
        {
            if (_missionProgress.Completed)
            {
                continue;
            }

            MissionData _missionData = DataManager.Instance.GameData.GetMission(_missionProgress.Id);
            switch (_missionData.Type)
            {
                case MissionType.DrawCard:
                    EventsManager.DrawCard -= _missionProgress.IncreaseAmount;
                    break;
                case MissionType.PlayCard:
                    EventsManager.PlayCard -= _missionProgress.IncreaseAmount;
                    break;
                case MissionType.WinALocationWithPowerLessThan100:
                    EventsManager.WinALocationWithPowerLess100 -= _missionProgress.IncreaseAmount;
                    break;
                case MissionType.WinMatch:
                    EventsManager.WinMatch -= _missionProgress.IncreaseAmount;
                    break;
                case MissionType.PlayCardCost1:
                    EventsManager.PlayCardCost1 -= _missionProgress.IncreaseAmount;
                    break;
                case MissionType.PlayCardCost2:
                    EventsManager.PlayCardCost2 -= _missionProgress.IncreaseAmount;
                    break;
                case MissionType.PlayCardCost3:
                    EventsManager.PlayCardCost3 -= _missionProgress.IncreaseAmount;
                    break;
                case MissionType.PlayCardCost4:
                    EventsManager.PlayCardCost4 -= _missionProgress.IncreaseAmount;
                    break;
                case MissionType.PlayCardCost5:
                    EventsManager.PlayCardCost5 -= _missionProgress.IncreaseAmount;
                    break;
                case MissionType.PlayCardCost6:
                    EventsManager.PlayCardCost6 -= _missionProgress.IncreaseAmount;
                    break;
                case MissionType.WinALocationWithPowerMoreThan200:
                    EventsManager.WinALocationWithPowerMore200 -= _missionProgress.IncreaseAmount;
                    break;
                case MissionType.WinALocationWith1Card:
                    EventsManager.WinALocationWith1Card -= _missionProgress.IncreaseAmount;
                    break;
                case MissionType.WinALocationWith4Card:
                    EventsManager.WinALocationWith4Card -= _missionProgress.IncreaseAmount;
                    break;
                case MissionType.PlayCardsOfPowerWorth:
                    EventsManager.PlayCardsOfPowerWorth -= _missionProgress.IncreaseAmount;
                    break;
                case MissionType.WinMatchWithADouble:
                    EventsManager.WinMatchWithADouble -= _missionProgress.IncreaseAmount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
