using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaneAbilityManager : MonoBehaviour
{
    public static LaneAbilityManager Instance;

    private List<LaneAbility> allLaneAbilities;

    private void Awake()
    {
        Instance = this;
        allLaneAbilities = Resources.LoadAll<LaneAbility>("LaneAbilities").ToList();
    }

    public LaneAbility GetLaneAbility(List<int> _excludeAbilities)
    {
        LaneAbility _laneAbility;
        List<LaneAbility> _abilities = new List<LaneAbility>(allLaneAbilities);
        foreach (var _excludeId in _excludeAbilities)
        {
            foreach (var _ability in _abilities.ToList()) 
            {
                if (_ability.Id==_excludeId)
                {
                    _abilities.Remove(_ability);
                    break;
                }
            }
        }

        LaneAbility _selectedAbility;

        if (DataManager.Instance.locationsPicked[GameplayManager.Instance.CurrentRound-1] == -1)
        {
            _selectedAbility = _abilities[Random.Range(0, _abilities.Count)];
        }
        else
        {
            _selectedAbility = _abilities.Find(_element =>
                _element.Id == DataManager.Instance.locationsPicked[GameplayManager.Instance.CurrentRound - 1]);
        }
        
        _laneAbility = CreateLaneAbility(_selectedAbility);


        return _laneAbility;
    }
    
    private int CheckAndUpdateAbility(int ability, int location)
    {
        if (DataManager.Instance.locationsPicked[location] == -1)   return ability;//-1 is random value so it stays the same
        return DataManager.Instance.locationsPicked[location];//picked location
    }

    public LaneAbility GetLaneAbility(int _id)
    {
        foreach (var _laneAbility in allLaneAbilities)
        {
            if (_laneAbility.Id==_id)
            {
                return _laneAbility;
            }
        }

        throw new System.Exception("Cant find laneAbility with id: "+_id);
    }

    private LaneAbility CreateLaneAbility(LaneAbility _laneAbility)
    {
        return Instantiate(_laneAbility, transform);
    }

}
