using NaughtyAttributes;
using UnityEngine;

public class QoomonUnlockTemp : MonoBehaviour
{
    [field: SerializeField] private int qoomonId;

    [Button()]
    private void UnlockQoomon()
    {
        DataManager.Instance.PlayerData.AddQoomon(qoomonId);
        SceneManager.Instance.ReloadScene();
    }
}
