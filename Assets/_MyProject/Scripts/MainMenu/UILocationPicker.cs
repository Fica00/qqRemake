using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace _MyProject.Scripts.MainMenu
{
    public class UILocationPicker : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown chosenLocationInput;
        [SerializeField] private int locationId;
        public List<LaneAbility> checkAbilities;
        private void OnEnable()
        {
            chosenLocationInput.onValueChanged.AddListener(ChoseLocation);
        }

        private void OnDisable()
        {
            chosenLocationInput.onValueChanged.AddListener(ChoseLocation);
        }

        private void Start()
        {
            RefreshList();
        }

        private void ChoseLocation(int id)
        {
            DataManager.Instance.locationsPicked[locationId] = id - 1;
        }

        private void RefreshList()
        {
            List<LaneAbility> _abilities = Resources.LoadAll<LaneAbility>("LaneAbilities").ToList();
            _abilities = _abilities.OrderBy(_ability => _ability.Id).ToList();
            //_abilities.Sort((a, b) => a.Id.CompareTo(b.Id));
            checkAbilities = _abilities;

            List<TMP_Dropdown.OptionData> _newOptionData = new List<TMP_Dropdown.OptionData>();

            foreach (var _ability in _abilities)
            {
                _newOptionData.Add(new(text: _ability.Description));
            }

            _newOptionData.Insert(0, new(text: "Random"));

            chosenLocationInput.ClearOptions();
            chosenLocationInput.AddOptions(_newOptionData);

            ChoseLocation(0);
        }
    }
}