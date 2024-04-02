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
        Mode = mode==GameMode.VsPlayer ? GameMode.VsAi : GameMode.VsPlayer;
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
