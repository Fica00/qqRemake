using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaneAbilityManager : MonoBehaviour
{
    public static LaneAbilityManager Instance;

    List<LaneAbility> allLaneAbilities;

    private void Awake()
    {
        Instance = this;
        allLaneAbilities = Resources.LoadAll<LaneAbility>("LaneAbilities").ToList();
    }

    public LaneAbility GetLaneAbility()
    {
        LaneAbility _selectedAbility = allLaneAbilities[Random.Range(0, allLaneAbilities.Count)];
        LaneAbility _laneAbility = CreateLaneAbility(_selectedAbility);
        allLaneAbilities.Remove(_selectedAbility);

        return _laneAbility;
    }

    LaneAbility CreateLaneAbility(LaneAbility _laneAbility)
    {
        return Instantiate(_laneAbility, transform);
    }

}
