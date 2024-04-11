using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

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
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
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
}