using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LockPlatform : MonoBehaviour
{
    [SerializeField] private int lockCount;
    [SerializeField] private TextMeshPro counterText;

    private void Start()
    {
        counterText.text = lockCount.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SpawnBall ball;
        if (ball = collision.GetComponent<SpawnBall>())
        {
            Destroy(ball.gameObject);
            lockCount--;
            counterText.text = lockCount.ToString();

            if (lockCount <= 0)
                Destroy(gameObject);
        }
    }
}
