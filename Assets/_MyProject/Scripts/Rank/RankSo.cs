using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRank", menuName = "ScriptableObject/Rank")]
public class RankSo : ScriptableObject
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [JsonIgnore] [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public int AmountOfOrbs { get; private set; }
    [field: SerializeField] public int Level { get; private set; }

    private static List<RankSo> allRanks;

    public static void Init()
    {
        allRanks = Resources.LoadAll<RankSo>("Ranks").ToList();
        allRanks = allRanks.OrderBy(_rank => _rank.Level).ToList();
    }

    public static RankData GetRankData(int _points)
    {
        RankData _rankData = new RankData();
        _rankData.Level = GetRankLevel(_points);
        int _level=1;
        foreach (var _rank in allRanks)
        {
            _rankData.RankSo = _rank;
            RankSo _nextRank = GetRankById(_rank.Id + 1);
            if (_nextRank==default)
            {
                _rankData.PointsOnRank = 0;
                break;
            }
            for (int _index = _level; _index < _nextRank.Level; _index++)
            {
                if (_points<_rank.AmountOfOrbs)
                {
                    _rankData.PointsOnRank = _points;
                    return _rankData;
                }

                _level++;
                _points -= _rank.AmountOfOrbs;
            }
        }

        _rankData.PointsOnRank = _points;
        if (_rankData.Level>100)
        {
            _rankData.Level = 100;
            _rankData.PointsOnRank = 7;
        }
        return _rankData;
    }
    
    public static RankSo GetRankById(int _id)
    {
        return allRanks.Find(_rank => _rank.Id == _id);
    }

    public static int GetRankLevel(int _points)
    {
        int _currentLevel = 1;
        int _rankIndex = 0;

        while (_points > 0)
        {
            int _currentAmountOfOrbs = (_rankIndex < allRanks.Count && _currentLevel >= allRanks[_rankIndex].Level)
                ? allRanks[_rankIndex].AmountOfOrbs
                : allRanks[_rankIndex - 1].AmountOfOrbs;

            if (_rankIndex + 1 < allRanks.Count && _currentLevel + 1 == allRanks[_rankIndex + 1].Level)
            {
                _rankIndex++;
            }

            if (_points >= _currentAmountOfOrbs)
            {
                _points -= _currentAmountOfOrbs;
                _currentLevel++;
            }
            else
            {
                break;
            }
        }

        return _currentLevel;
    }
}
