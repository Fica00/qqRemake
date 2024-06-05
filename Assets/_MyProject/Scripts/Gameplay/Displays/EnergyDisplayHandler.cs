using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EnergyDisplayHandler : MonoBehaviour
{
    [SerializeField] private HorizontalLayoutGroup horizontalLayoutGroup;
    [SerializeField] private Image[] energyDisplays;
    [SerializeField] private Color availableColor;
    [SerializeField] private Color notAvailableColor;
    private GameplayPlayer gameplayPlayer;

    public void Setup(GameplayPlayer _player)
    {
        gameplayPlayer = _player;

        gameplayPlayer.UpdatedEnergy += ShowEnergy;
        GameplayManager.GameEnded += Hide;
        ShowEnergy();

    }

    private void OnDisable()
    {
        gameplayPlayer.UpdatedEnergy -= ShowEnergy;
        GameplayManager.GameEnded -= Hide;
    }

    private void Hide(GameResult _result)
    {
        gameObject.SetActive(false);
    }

    private void ShowEnergy()
    {
        for (int i = 0; i < gameplayPlayer.Energy; i++)
        {
            if (i >= energyDisplays.Length)
            {
                break;
            }
            energyDisplays[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < gameplayPlayer.Energy; i++)
        {
            if (i >= energyDisplays.Length)
            {
                break;
            }
            energyDisplays[i].color = availableColor;
        }

        for (int i = gameplayPlayer.Energy; i < energyDisplays.Length; i++)
        {
            energyDisplays[i].color = notAvailableColor;
        }

        if (gameplayPlayer.Energy>6)
        {
            horizontalLayoutGroup.childControlHeight = true;
            horizontalLayoutGroup.childControlWidth = true;
            horizontalLayoutGroup.childForceExpandHeight = true;
            horizontalLayoutGroup.childForceExpandWidth = true;
        }
    }
}
