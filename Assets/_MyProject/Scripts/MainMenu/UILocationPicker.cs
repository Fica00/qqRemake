using TMPro;
using UnityEngine;

namespace _MyProject.Scripts.MainMenu
{
    public class UILocationPicker: MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown chosenLocationInput;
        [SerializeField] private int locationId;
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
            ChoseLocation(0);
        }

        private void ChoseLocation(int id)
        {
            DataManager.Instance.locationsPicked[locationId] = id - 1;

        }
        
        
    }
}