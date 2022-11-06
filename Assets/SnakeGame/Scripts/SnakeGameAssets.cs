using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGameAssets : MonoBehaviour
{
    public static SnakeGameAssets Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public SoundAudioClip[] soundAudioClipsArray;

    public Sprite HeadSnakeSprite;
    public Sprite BodySnakeSprite;
    public Sprite Food;

    [Serializable]
    public class SoundAudioClip
    {
        public SnakeSoundManager.Sound sound;
        public AudioClip audioClip;
    }
}