using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class BGM : Singleton<BGM>
{
    private const float DEFAULT_VOLUME = 0.5f;

    public enum BGMTypes
    {
        Playing
    }

    [SerializeField] private AudioSource audioSource;

    private static Dictionary<BGMTypes, string> soundMap = new ()
    {
        { BGMTypes.Playing, "Sound/BGM/Platform Action LOOP" }
    };

    private Dictionary<BGMTypes, AudioClip> cachedSounds = new ();

    public void PlaySound(BGMTypes type)
    {
        if (soundMap.TryGetValue(type, out var path))
        {
            AudioClip audioClip;
            if (!cachedSounds.TryGetValue(type, out var sound))
            {
                audioClip = Resources.Load<AudioClip>(path);
                cachedSounds.Add(type, audioClip);
            }
            else
            {
                audioClip = sound;
            }

            audioSource.volume = DEFAULT_VOLUME;
            audioSource.loop = true;
            audioSource.clip = audioClip;
            audioSource.Play(0);
        }
        else
        {
            Debug.LogFormat("No sound is found:{0}", type);
        }
    }

    public async UniTask FadeInSound(float duration = 1.0f)
    {
        audioSource.Play(0);
        await audioSource.DOFade(0.5f, duration);
    }
    
    public async UniTask FadeOutSound(float duration = 1.0f)
    {
        await audioSource.DOFade(0.0f, duration);
        audioSource.Stop();
    }
    
    public async UniTask TurnUpVolume(float targetVolume = 0.5f, float duration = 1.0f)
    {
        await audioSource.DOFade(targetVolume, duration);
    }
    
    public async UniTask TurnDownVolume(float targetVolume = 0.1f, float duration = 1.0f)
    {
        await audioSource.DOFade(targetVolume, duration);
    }
}