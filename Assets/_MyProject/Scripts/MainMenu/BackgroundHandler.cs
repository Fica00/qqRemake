using UnityEngine;

public class BackgroundHandler : MonoBehaviour
{
    public const string DISCORD_CLICK = "Discord click";
    [SerializeField] private GameObject discordBG;
    [SerializeField] private GameObject qoomonBG;

    private void OnEnable()
    {
        PlayerData.UpdatedStatistics += Check;
    }

    private void OnDisable()
    {
        PlayerData.UpdatedStatistics -= Check;
    }
    
    private void Start()
    {
        Check();   
    }

    private void Check()
    {
        discordBG.SetActive(false);
        qoomonBG.SetActive(false);
        if (DataManager.Instance.PlayerData.Statistics.HasCheckPoint(DISCORD_CLICK))
        {
            qoomonBG.SetActive(true);
        }
        else
        {
            discordBG.SetActive(true);
        }
    }
}
