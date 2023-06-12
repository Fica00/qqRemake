using System.Collections;
using UnityEngine;

public class LowEnergyPanel : MonoBehaviour
{
    [SerializeField] GameObject holder;

    private void OnEnable()
    {
        CardInteractions.DragStarted += CheckEnergy;
    }

    private void OnDisable()
    {
        CardInteractions.DragStarted -= CheckEnergy;
    }

    void CheckEnergy(CardObject _cardObject)
    {
        StartCoroutine(CheckEnergyRoutine());

        IEnumerator CheckEnergyRoutine()
        {
            if (_cardObject.Stats.Energy>GameplayManager.Instance.MyPlayer.Energy)
            {
                holder.SetActive(true);
                yield return new WaitForSeconds(2);
                holder.SetActive(false);
            }
        }
    }
}
