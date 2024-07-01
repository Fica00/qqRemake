using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Statistics;
using UnityEngine;

[Serializable]
public class PlayerStatistics
{
    public static Action<int> OnSawNewQoomon;
    private const string LAST_LOGGED_TIME = "lastTime";
    public List<Login> Logins = new ();
    public List<TimeSpent> TimeSpent = new();
    public List<CheckPoint> CheckPoints = new();
    public List<MatchPlayed> MatchesPlayed = new ();
    public List<int> SeenQoomons = new();


    private bool didCheckForTimeSpent;

    public void StartSession()
    {
        CheckForNewLogin();
        CheckForTimeSpent();
        CheckForPwa();
    }

    private void CheckForNewLogin()
    {
        Login _lastLogin = GetLastLogin();
        Login _currentLogin = new Login { Count = 0, Date = DateTime.UtcNow };
        if ((_currentLogin.Date.WithoutHours()-_lastLogin.Date.WithoutHours()).TotalDays<1)
        {
            return;
        }
        
        NoteLogin();
    }

    private void NoteLogin()
    {
        Login _lastLogin = GetLastLogin();
        Login _currentLogin = new Login { Count = 0, Date = DateTime.UtcNow };
        if ((_currentLogin.Date.WithoutHours()-_lastLogin.Date.WithoutHours()).TotalDays<=1)
        {
            _currentLogin.Count = _lastLogin.Count + 1;
        }
        
        Logins.Add(_currentLogin);
        PlayerData.UpdatedStatistics?.Invoke();
    }
    
    private Login GetLastLogin()
    {
        if (Logins.Count==0)
        {
            return new Login { Count = 1, Date = DateTime.MinValue };
        }

        Login _lastLogin = Logins[0];
        foreach (var _login in Logins)
        {
            if (_lastLogin.Date>=_login.Date)
            {
                continue;
            }

            _lastLogin = _login;
        }

        return _lastLogin;
    }

    private void CheckForTimeSpent()
    {
        if (didCheckForTimeSpent)
        {
            return;
        }

        didCheckForTimeSpent = true;
        TimeSpent _newLog = new TimeSpent { Minutes = 0, Date = DateTime.UtcNow };
        if (PlayerPrefs.HasKey(LAST_LOGGED_TIME))
        {
            TimeSpent _lastLoggedTime = JsonConvert.DeserializeObject<TimeSpent>(PlayerPrefs.GetString(LAST_LOGGED_TIME));
            if ((DateTime.UtcNow-_lastLoggedTime.Date.AddMinutes(_lastLoggedTime.Minutes)).TotalMinutes>2)
            {
                NoteTimeSpent(_lastLoggedTime);
            }
            else
            {
                _newLog = _lastLoggedTime;
            }
        }

        DataManager.Instance.StartCoroutine(NoteTimeRoutine(_newLog));
    }

    private void NoteTimeSpent(TimeSpent _timeSpent)
    {
        TimeSpent.Add(_timeSpent);
        PlayerData.UpdatedStatistics?.Invoke();
    }

    private IEnumerator NoteTimeRoutine(TimeSpent _timeSpent)
    {
        while (true)
        {
            yield return new WaitForSeconds(60);
            _timeSpent.Minutes++;
            PlayerPrefs.SetString(LAST_LOGGED_TIME, JsonConvert.SerializeObject(_timeSpent));
        }
    }

    public void NoteFirstDeckUpdate(string _change)
    {
        NoteCheckPoint("First deck update",_change);
    }

    private void CheckForPwa()
    {
        if (!JavaScriptManager.Instance.IsPwaPlatform)
        {
            return;
        }
        
        NoteCheckPoint("First time opened as pwa");
    }

    public void NoteCheckPoint(string _description, string _additionalData= null, bool _checkForExisting = true)
    {
        CheckPoint _newCheckPoint = new CheckPoint { Description = _description, Date = DateTime.UtcNow, AdditionalData = _additionalData};
        if (_checkForExisting)
        {
            if (HasCheckPoint(_description))
            {
                return;
            }
        }
        
        CheckPoints.Add(_newCheckPoint);
        PlayerData.UpdatedStatistics?.Invoke();
    }

    public bool HasCheckPoint(string _description)
    {
        return CheckPoints.Any(_checkPoint => _checkPoint.Description == _description);
    }

    public void IncreaseMatchCount()
    {
        if (MatchesPlayed.Count==0)
        {
            if (CheckPoints.Find(_checkPoint => _checkPoint.Description == "Finished first game")==null)
            {
                return;
            }
        }

        string _date = DateTime.UtcNow.Date.ToString(CultureInfo.InvariantCulture);
        MatchPlayed _dayData = MatchesPlayed.Find(_data => _data.Date == _date);
        if (_dayData==null)
        {
            _dayData = new MatchPlayed(){Date = _date};
            MatchesPlayed.Add(_dayData);
        }

        _dayData.Count++;
        PlayerData.UpdatedStatistics?.Invoke();
    }
    
    public void NoteSeenQoomon(int _cardId)
    {
        if (SeenQoomons.Contains(_cardId))
        {
            return;
        }
        
        SeenQoomons.Add(_cardId);
        OnSawNewQoomon?.Invoke(_cardId);
        PlayerData.UpdatedStatistics?.Invoke();
    }

}
