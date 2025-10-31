using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfernoUnit : RangeUnit
{
    [Header("Inferno Settings")]
    [SerializeField] private float damageMultiplier;
    [SerializeField] private float resetDelay;
    [SerializeField] private float multiplyInterval;
    [SerializeField] private float maxDamage;

    private float currentDamage;
    private Coroutine multipliedDamageCoroutine;

    protected override IEnumerator AttackEnemy()
    {
        ResetDamage();
        multipliedDamageCoroutine = StartCoroutine(MultipliedDamage());

        while (true)
        {
            yield return new WaitForSeconds(attackInterval);
            animator.Play("attack");

            if (targetEnemy == null && GetNearestEnemy() == null)
            {
                isAttacking = false;
                targetEnemy = null;
                canWalk = true;

                attackCoroutine = null;
                yield break;
            }
            if (targetEnemy == null || targetEnemy.hp <= 0)
            {
                ResetDamage();

                var enemy = GetNearestEnemy();
                if (enemy != null)
                {
                    targetEnemy = enemy;
                    multipliedDamageCoroutine = StartCoroutine(MultipliedDamage());
                    continue;
                }

                isAttacking = false;
                targetEnemy = null;
                canWalk = true;

                attackCoroutine = null;
                yield break;
            }

            var newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            ShootBullet(newBullet.transform, targetEnemy);
        }
    }

    private IEnumerator MultipliedDamage()
    {
        while (true)
        {
            currentDamage = Mathf.Min(currentDamage * damageMultiplier, maxDamage);
            yield return new WaitForSeconds(multiplyInterval);
        }
    }

    private void ResetDamage()
    {
        if (multipliedDamageCoroutine != null)
            StopCoroutine(multipliedDamageCoroutine);

        currentDamage = damage;
        multipliedDamageCoroutine = null;
    }

    protected override void BulletCallback(UnitObject targetUnit, Transform arrow)
    {
        targetUnit.TakeDamage((int)currentDamage); 
        Destroy(arrow.gameObject);
    }

}
