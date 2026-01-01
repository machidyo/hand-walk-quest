using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private const float DEFAULT_VOLUME = 0.5f;

    public enum SoundNames
    {
        Click,
        Cancel,
        Error,
        PopUp,
        Close,
        CountDown,
        Coin,
        Win,
        Lose
    }

    private List<Sound> sounds = new ()
    {
        new Sound(SoundNames.Click,  "Sound/SE/Click  Back (7)"),
        new Sound(SoundNames.Cancel, "Sound/SE/Click (7)"),
        //new Sound(SoundNames.Error,  "Sound/SE/SE_Error"),
        new Sound(SoundNames.PopUp, "Sound/SE/Special Pop (5)"),
        // new Sound(SoundNames.Close, "Sound/SE/Special Pop (5)"),
        new Sound(SoundNames.CountDown, "Sound/SE/Countdown (2)"),
        new Sound(SoundNames.Coin, "Sound/SE/Coins (2)"),
        new Sound(SoundNames.Win, "Sound/SE/You Win (5)"),
        //new Sound(SoundNames.Lose, "Sound/SE/Countdown (2)"),
    };
    
    private AudioSource audioSource;

    // PlaySound が実行される前に AudioSource を設定するために Awake
    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = DEFAULT_VOLUME;
    }

    public void PlaySound(SoundNames soundName)
    {
        if (sounds.Any(s => s.Name == soundName))
        {
            var sound = sounds.First(s => s.Name == soundName);
            if (!sound.HasCash)
            {
                sound.Clip = Resources.Load<AudioClip>(sound.Path);
            }

            audioSource.PlayOneShot(sound.Clip);
        }
        else
        {
            Debug.LogError($"{soundName} is not found.");
        }
    }

    public void PlaySound(int typeNum)
    {
        PlaySound((SoundNames)typeNum);
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void StopSound()
    {
        audioSource.Stop();
    }

    public void PauseSound()
    {
        audioSource.Pause();
    }

    public void UnpauseSound()
    {
        audioSource.UnPause();
    }

    private class Sound
    {
        public SoundNames Name { get; }
        public string Path { get; }
        public AudioClip Clip;

        public bool HasCash => Clip != null;

        public Sound(SoundNames soundName, string audioPath)
        {
            Name = soundName;
            Path = audioPath;
        }
    }
}