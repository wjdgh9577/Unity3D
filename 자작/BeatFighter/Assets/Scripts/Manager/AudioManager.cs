using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public static Action OnEffectSoundChange;

    [Header("Menu Clip")]
    [SerializeField]
    private AudioClip[] main;
    [Header("Battle Clip")]
    [SerializeField]
    private AudioClip[] battle;
    [Header("Boss Room Clip")]
    [SerializeField]
    private AudioClip boss;
    [Header("Reward Clip")]
    [SerializeField]
    private AudioClip victory;
    [SerializeField]
    private AudioClip gameover;

    private float musicSoundDegree;
    private float effectSoundDegree;

    private AudioSource _audioSource;
    public AudioSource audioSource
    {
        get
        {
            if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
            return _audioSource;
        }
        private set
        {

        }
    }

    public void Setup()
    {
        musicSoundDegree = PlayerData.musicSoundDegree;
        effectSoundDegree = PlayerData.effectSoundDegree;
    }

    #region Audio Control
    private void PlayOneShot(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.loop = false;
        audioSource.volume = musicSoundDegree;
        audioSource.Play();
    }

    private void PlayLoop(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.volume = musicSoundDegree;
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }
    #endregion

    #region Sound Volume Setting
    public void SetMusicSoundDegree()
    {
        musicSoundDegree = PlayerData.musicSoundDegree;
        audioSource.volume = musicSoundDegree;
    }

    public void SetEffectSoundDegree()
    {
        effectSoundDegree = PlayerData.effectSoundDegree;
        OnEffectSoundChange?.Invoke();
    }

    public float GetEffectSoundDegree()
    {
        return effectSoundDegree;
    }
    #endregion

    #region Game Music
    public void PlayMain()
    {
        int index = UnityEngine.Random.Range(0, main.Length);
        PlayLoop(main[index]);
    }

    public void PlayBattle(int fieldID)
    {
        int index = UnityEngine.Random.Range(0, battle.Length);
        PlayLoop(battle[index]);
    }

    public void PlayBoss(int fieldID)
    {
        PlayLoop(boss);
    }

    public void PlayVictory()
    {
        PlayOneShot(victory);
    }

    public void PlayGameover()
    {
        PlayOneShot(gameover);
    }
    #endregion
}
