using UnityEngine;
using UnityEngine.UI;

public class EnergyDisplayHandler : MonoBehaviour
{
    [SerializeField] Image[] energyDisplays;
    [SerializeField] Color availableColor;
    [SerializeField] Color notAvailableColor;
    GameplayPlayer gameplayPlayer;

    public void Setup(GameplayPlayer _player)
    {
        gameplayPlayer = _player;

        gameplayPlayer.UpdatedEnergy += ShowEnergy;
        ShowEnergy();

    }

    private void OnDisable()
    {
        gameplayPlayer.UpdatedEnergy -= ShowEnergy;
    }

    void ShowEnergy()
    {
        for (int i = 0; i < GameplayManager.Instance.CurrentRound; i++)
        {
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
    }
}
