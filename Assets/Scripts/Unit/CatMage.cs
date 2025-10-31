using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMage : RangeUnit
{
    [Header("Mage Unit Settings")]
    [SerializeField] private float splashRadius;
    [SerializeField] private GameObject effectPrefab;

    protected override void BulletCallback(UnitObject targetUnit, Transform arrow)
    {
        Instantiate(effectPrefab, arrow.position, Quaternion.identity);
        targetUnit.TakeDamage(damage);

        ApplySpalshDamage(targetUnit.transform.position);

        if (targetEnemy.hp <= 0 || targetEnemy == null)
        {
            isAttacking = false;
            targetEnemy = null;
            canWalk = true;

            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
        Destroy(arrow.gameObject);
    }

    private void ApplySpalshDamage(Vector3 impactPosition)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(impactPosition, splashRadius);

        foreach (var hitCollider in hitColliders)
        {
            UnitObject unit = hitCollider.GetComponent<UnitObject>();

            if (unit != null && unit.isEnemy != isEnemy && unit != targetEnemy)
            {
                int splashDamage = Mathf.CeilToInt(damage * 0.5f);
                unit.TakeDamage(splashDamage);
            }
        }
    }
}

