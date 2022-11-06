using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SnakeSoundManager
{
    public enum Sound
    {
        SnakeMove,
        SnakeDie,
        SnakeEat
    }

    public static void PlaySound(Sound sound)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (SnakeGameAssets.SoundAudioClip soundAudioClip in SnakeGameAssets.Instance.soundAudioClipsArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError($"Sound {sound} not found!");
        return null;
    }
}