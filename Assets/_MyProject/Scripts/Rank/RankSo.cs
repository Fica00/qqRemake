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
    [field: SerializeField] public int RequirementPerSubRank { get; private set; }
    [field: SerializeField] public int AmountOfSubRanks { get; private set; }

    private static List<RankSo> allRanks;

    public static void Init()
    {
        allRanks = Resources.LoadAll<RankSo>("Ranks").ToList();
        allRanks = allRanks.OrderBy(_rank => _rank.Id).ToList();
    }

    public static RankData GetRankData(int _points)
    {
        RankData _rankData = new RankData();
        foreach (var _rank in allRanks)
        {
            _rankData.RankSo = _rank;
            _rankData.SubRank = 1;
            for (int _index = 0; _index < _rank.AmountOfSubRanks; _index++)
            {
                if (_points<_rank.RequirementPerSubRank)
                {
                    _rankData.PointsOnRank = _points;
                    return _rankData;
                }

                _rankData.SubRank += 1;
                _points -= _rank.RequirementPerSubRank;
            }
        }

        _rankData.PointsOnRank = _points;
        return _rankData;
    }
}
