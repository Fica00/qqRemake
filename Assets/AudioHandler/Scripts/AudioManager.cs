using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public const string CONFIRM = "Confirm";
    public const string BACK = "Back";
    public const string PLAY = "Play";
    public const string MAIN_MENU = "MainMenu";
    public const string MATCHMAKING = "Matchmaking";
    public const string GAME = "Game";
    public const string SUMMON = "Summon";
    public const string WIN = "Win";
    public const string DRAW = "Draw";
    public const string LOSE = "Lose";
    public const string CARD_SOUND = "CardSound";
    public const string DRAW_CARD = "DrawCard";
    public const string NEW_TURN = "NewTurn";
    public const string REVEAL = "Reveal";
    public const string WIN_LOCATION = "WinLocation";
    public const string DOUBLE_INITIATED = "DoubleInitiated";
    public const string DOUBLE_RESOLVED = "DoubleResolved";

    [SerializeField] private List<AudioSound> audios;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        ChangeBackgroundMusic(MAIN_MENU);
        PlayBackgroundMusic();
        PlayerData.UpdatedBackgroundMusic += PlayBackgroundMusic;
    }

    private void OnDisable()
    {
        PlayerData.UpdatedBackgroundMusic -= PlayBackgroundMusic;
    }

    private void PlayBackgroundMusic()
    {
        if (DataManager.Instance.PlayerData.PlayBackgroundMusic)
        {
            if (audioSource.isPlaying)
            {
                return;
            }
            audioSource.volume = 0;
            audioSource.Play();
            audioSource.DOFade(1, 1);
        }
        else
        {
            audioSource.DOFade(0, 1).OnComplete((() =>
            {
                audioSource.Stop();
                audioSource.volume = 1;
            }));
        }
    }

    public void ChangeBackgroundMusic(string _key)
    {
        AudioSound _audio = GetAudioSound(_key);
        if (_audio.AudioClip == audioSource.clip)
        {
            return;
        }
        
        audioSource.DOFade(0, 0.5f).OnComplete(() =>
        {
            audioSource.clip = _audio.AudioClip;
            audioSource.DOFade(_audio.Volume, 0.5f).OnComplete(() =>
            {
                if (!audioSource.isPlaying && DataManager.Instance.PlayerData.PlayBackgroundMusic)
                {
                    audioSource.Play();
                }
            });
        });
    }

    public void PlaySoundEffect(string _key)
    {
        if (!DataManager.Instance.PlayerData.PlaySoundEffects)
        {
            return;
        }

        AudioSound _audio = GetAudioSound(_key);

        audioSource.PlayOneShot(_audio.AudioClip, _audio.Volume);
    }

    private AudioSound GetAudioSound(string _key)
    {
        AudioSound _audio = audios.Find(_element => _element.Key == _key);
        if (_audio == null)
        {
            throw new Exception("Cant find audio inside audios list for key: " + _key);
        }

        return _audio;
    }
    
    private void OnApplicationFocus(bool _hasFocus)
    {
        if (DataManager.Instance != default && DataManager.Instance.PlayerData != default )
        {
            if (_hasFocus&& DataManager.Instance.PlayerData.PlayBackgroundMusic)
            {
                audioSource.volume = 1;
            }
            else
            {
                audioSource.volume = 0;
            }
            
            return;
        }

        audioSource.volume = _hasFocus ? 1 : 0;
    }
}