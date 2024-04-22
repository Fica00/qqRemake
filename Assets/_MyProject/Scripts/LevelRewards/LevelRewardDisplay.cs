using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelRewardDisplay : MonoBehaviour
{
    public static Action<LevelReward> OnClicked;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image laneImage;
    
    [SerializeField] private GameObject placeHolder;
    [SerializeField] private Image qoomonDisplay;
    [SerializeField] private GameObject lockDisplay;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private GameObject completedDisplay;
    [SerializeField] private Button claimButton;
    [SerializeField] private Transform[] leftChanges;

    [SerializeField] private Color readyToClaimBackground;
    [SerializeField] private GameObject redDot;
    [SerializeField] private Sprite cloud; 

    private LevelReward levelReward;
    
    public void Setup(LevelReward _levelReward, bool _isLeft)
    {
        level.text = _levelReward.Level.ToString();
        if (_isLeft)
        {
            foreach (var _leftChange in leftChanges)
            {
                _leftChange.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        
        levelReward = _levelReward;
        
        if (DataManager.Instance.PlayerData.Level<_levelReward.Level)
        {
            placeHolder.SetActive(true);
            level.color = FadeColor(level.color);
            backgroundImage.color = FadeColor(backgroundImage.color);
            laneImage.color = FadeColor(laneImage.color);
            return;
        }
        
        
        if (DataManager.Instance.PlayerData.HasClaimedLevelReward(_levelReward.Level))
        {
            int _qoomonId = DataManager.Instance.PlayerData.GetClaimedLevelReward(_levelReward.Level).QoomonId;
            qoomonDisplay.sprite = _qoomonId == -1 ? cloud : CardsManager.Instance.GetCardSprite(_qoomonId);
            if (_qoomonId==-1)
            {
                qoomonDisplay.SetNativeSize();
                qoomonDisplay.transform.localPosition = new Vector3(-59,7.3731f,0);
            }
            completedDisplay.SetActive(true);
        }
        else
        {
            redDot.SetActive(true);
            backgroundImage.color = readyToClaimBackground;
            lockDisplay.SetActive(true);
        }
    }

    private Color FadeColor(Color _color)
    {
        Color _newColor = _color;
        _newColor.a = 0.6f;
        return _newColor;
    }

    private void OnEnable()
    {
        claimButton.onClick.AddListener(Claim);
    }

    private void OnDisable()
    {
        claimButton.onClick.RemoveListener(Claim);
    }

    private void Claim()
    {
        OnClicked?.Invoke(levelReward);
    }
}
