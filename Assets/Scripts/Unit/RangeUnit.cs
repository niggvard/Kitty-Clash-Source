using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class RangeUnit : UnitObject
{
    [Header("RangeUnitSettings")]
    [SerializeField] protected Transform bullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float arcHeight;

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

            var bulletSpawnPosition = transform.position + GetBulletSpawnOffset();
            var newBullet = Instantiate(bullet, bulletSpawnPosition, Quaternion.identity);
            ShootBullet(newBullet.transform, targetEnemy, 0.2f);
        }
    }

    protected virtual Vector3 GetBulletSpawnOffset()
    {
        return Vector3.zero;
    }
    
    protected virtual async void ShootBullet(Transform arrow, UnitObject targetUnit, float cooldown = 0)
    {
        Transform target = targetUnit.transform;
        Vector3 startPos = arrow.position;
        Vector3 targetPos = target.position;

        await Task.Delay((int)(cooldown * 1000));

        Vector3 midPoint = (startPos + targetPos) / 2;
        midPoint.y += arcHeight;

        Sequence flightSequence = DOTween.Sequence();
        flightSequence.Append(arrow.DOMove(midPoint, bulletSpeed / 2).SetEase(Ease.OutQuad));
        flightSequence.Append(arrow.DOMove(targetPos, bulletSpeed / 2).SetEase(Ease.InQuad));
        flightSequence.Join(arrow.DOLookAt(targetPos, bulletSpeed, AxisConstraint.None));
        flightSequence.onComplete = () => BulletCallback(targetUnit, arrow);
    }

    protected virtual void BulletCallback(UnitObject targetUnit, Transform arrow)
    {
        if (targetEnemy == null || targetEnemy.hp <= 0)
        {
            targetUnit = GetNearestEnemy();
            targetUnit?.TakeDamage(damage);
        }
        else
            targetUnit.TakeDamage(damage);

        if (targetEnemy.hp <= 0 || targetEnemy == null)
        {
            isAttacking = false;
            targetEnemy = null;
            canWalk = true;
        }

        Destroy(arrow.gameObject);
    }
}