using System;
using UnityEngine;

public class ModeHandler : MonoBehaviour
{
    public static Action OnUpdatedMode;
    public static ModeHandler Instance;

    private GameMode mode;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Mode = GameMode.VsAi;
    }

    public GameMode Mode
    {
        get
        {
            return mode;
        }
        set
        {
            mode = value;
            OnUpdatedMode?.Invoke();
        }
    }

    private void OnEnable()
    {
        mode = GameMode.VsPlayer;
        OnUpdatedMode += ShowMode;
        ShowMode();
    }

    private void OnDisable()
    {
        OnUpdatedMode -= ShowMode;
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
    }
}
