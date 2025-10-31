using System;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public event Action<Collider2D> OnTriggerEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter?.Invoke(collision);
    }
}
