using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Multiplier : MonoBehaviour
{
    [SerializeField] private int multiplier;
    [SerializeField] private TextMeshPro multiplierText;

    private void Start()
    {
        multiplierText.text = "X" + multiplier;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SpawnBall unit = collision.GetComponent<SpawnBall>();
        if(unit != null && unit.multiplier != this)
            MultiplyUnit(unit);
    }

    private void MultiplyUnit(SpawnBall unit)
    {
        for(int i = 1; i< multiplier; i++)
        {
            Vector3 offset = new (0, i * -0.2f);
            var newBall = Instantiate(unit, unit.transform.position + offset, Quaternion.identity).Setup(unit.Unit);
            newBall.multiplier = this;
            newBall.Body.velocity = unit.Body.velocity;
            VibrationManager.Vibrate(0.2f);
        }
    }
}
