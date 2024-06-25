using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModeHandler : MonoBehaviour
{
    public static Action OnUpdatedMode;
    public static ModeHandler Instance;

    [SerializeField] private TextMeshProUGUI modeDisplay;
    [SerializeField] private Button changeModeButton;

    public static GameMode ModeStatic = GameMode.VsAi;

    private void Awake()
    {
        Instance = this;
    }

    public GameMode Mode
    {
        get => ModeStatic;
        set
        {
            ModeStatic = value;
            OnUpdatedMode?.Invoke();
        }
    }
    
    private void OnEnable()
    {
        changeModeButton.onClick.AddListener(ChangeMode);
        OnUpdatedMode += ShowMode;
        ShowMode();
    }

    private void OnDisable()
    {
        changeModeButton.onClick.RemoveListener(ChangeMode);
        OnUpdatedMode -= ShowMode;
    }

    private void ChangeMode()
    {
        switch (Mode)
        {
            case GameMode.VsAi:
                Mode = GameMode.VsPlayer;
                break;
            case GameMode.VsPlayer:
                Mode = GameMode.Friendly;
                break;
            case GameMode.Friendly:
                Mode = GameMode.VsAi;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ShowMode()
    {
        string _modeName = string.Empty;
        switch (Mode)
        {
            case GameMode.VsAi:
                _modeName = "vs AI";
                break;
            case GameMode.VsPlayer:
                _modeName = "Online";
                break;
            case GameMode.Friendly:
                _modeName = "Friendly";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        modeDisplay.text = _modeName;
    }
}