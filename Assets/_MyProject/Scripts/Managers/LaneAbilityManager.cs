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

        LaneAbility _selectedAbility = _abilities[Random.Range(0, _abilities.Count)];
        LaneAbility _laneAbility = CreateLaneAbility(_selectedAbility);

        return _laneAbility;
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
