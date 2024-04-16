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
        return _rankData;
    }
    
    public static RankSo GetRankById(int _id)
    {
        return allRanks.Find(_rank => _rank.Id == _id);
    }

    public static int GetRankLevel(int _points)
    {
        int _level = 1;
        while (_points >= 7)
        {
            _points -= 7;
            _level++;
        }

        return _level;
    }
}
