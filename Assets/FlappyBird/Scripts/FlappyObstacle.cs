using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyObstacle : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (FlappyGameManager.Instance.gameState == GameState.Active)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}