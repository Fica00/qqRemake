using System;

[Serializable]
public class RankData
{
    public RankSo RankSo;
    public int SubRank;
    public int PointsOnRank;
    
    public float Percentage
    {
        get
        {
            if (PointsOnRank==0)
            {
                return 0;
            }
            
            return (float)PointsOnRank / RankSo.RequirementPerSubRank;
        }
    }
}
