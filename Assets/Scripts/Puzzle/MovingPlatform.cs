using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Collider2D[] colliderList;
    [SerializeField] private Rigidbody2D platformBody;
    [SerializeField] private float platformSpeed;

    private int currentIndex;
    private Vector3 direction;
    private Collider2D currentDestination;

    private void Start()
    {
        currentIndex = 0;
        currentDestination = colliderList[currentIndex];
        direction = (currentDestination.transform.position - platformBody.transform.position).normalized;
    }

    private void FixedUpdate()
    {
        platformBody.velocity = direction * platformSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != currentDestination) return;

        currentIndex++;
        if (currentIndex == colliderList.Length)
            currentIndex = 0;

        currentDestination = colliderList[currentIndex];
        direction = (currentDestination.transform.position - platformBody.transform.position).normalized;
    }
}
