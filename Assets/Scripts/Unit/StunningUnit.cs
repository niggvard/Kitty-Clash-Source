using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunningUnit : RangeUnit
{
    [Header("StunningUnitSettings")]
    [SerializeField] private float stuningTime;
    [SerializeField] private GameObject effectPrefab;

    protected override void BulletCallback(UnitObject targetUnit, Transform arrow)
    {
        Instantiate(effectPrefab, arrow.position, Quaternion.identity);
        base.BulletCallback(targetUnit, arrow);
        targetUnit.Stun(0.3f);
    }
}
