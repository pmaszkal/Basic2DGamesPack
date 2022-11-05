using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGameAssets : MonoBehaviour
{
    public static SnakeGameAssets instance;

    private void Awake()
    {
        instance = this;
    }

    public Sprite HeadSnakeSprite;
    public Sprite BodySnakeSprite;
    public Sprite Food;
}