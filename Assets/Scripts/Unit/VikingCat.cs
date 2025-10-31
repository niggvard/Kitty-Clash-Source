using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingCat : UnitObject
{
    [SerializeField] private float splashRadius;

    protected override IEnumerator AttackEnemy()
    {
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
                var enemy = GetNearestEnemy();
                if (enemy != null)
                {
                    targetEnemy = enemy;
                    continue;
                }

                isAttacking = false;
                targetEnemy = null;
                canWalk = true;

                attackCoroutine = null;
                yield break;
            }
            SplashAttack(targetEnemy.transform.position);
        }
    }

    private void SplashAttack(Vector3 attackPoint)
    {
        targetEnemy.TakeDamage(damage);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, splashRadius);

        foreach (Collider2D collider in hitEnemies)
        {
            UnitObject enemy = collider.GetComponent<UnitObject>();
            if (enemy != null && enemy.isEnemy != isEnemy && enemy != targetEnemy)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
