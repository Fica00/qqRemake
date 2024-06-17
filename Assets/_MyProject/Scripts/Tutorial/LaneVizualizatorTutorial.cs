using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaneVizualizatorTutorial : LaneVizualizator
{
    
    protected override void CheckIfLaneIsAvailable(CardObject _card)
    {
        foreach (var _myPlace in myPlaces)
        {
            if(_myPlace.Location == CheckLaneForThisRound())
            if (_myPlace.CheckIfTileIsAvailable(_card))
            {
                laneIndicator.SetActive(true);
                return;
            }
        }
        
        laneIndicator.SetActive(false);
    }

    private LaneLocation CheckLaneForThisRound()
    {
        switch (GameplayManager.Instance.CurrentRound)
        {
            case 1:
                return LaneLocation.Top;
                
            case 2:
                return LaneLocation.Mid;
                
            case 3:
                return LaneLocation.Bot;
                
            case 4:
                return LaneLocation.Mid;
                
            case 5:
                return LaneLocation.Mid;
                
            case 6:
                return LaneLocation.Bot;
                
            default:
                return LaneLocation.Top;
                
        }
    }
}
