using System;

[Serializable]
public class RankData
{
    public RankSo RankSo;
    public int PointsOnRank;
    public int Level;
    
    public float Percentage
    {
        get
        {
            if (PointsOnRank==0)
            {
                return 0;
            }
            
            return (float)PointsOnRank / RankSo.AmountOfOrbs;
        }
    }
}
