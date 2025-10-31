using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingUnit : UnitObject
{
    [SerializeField] private float yOffset;

    protected override void Start()
    {
        base.Start();
        Vector3 position = transform.position;
        position.y += yOffset;
        transform.position = position;
    }
}
