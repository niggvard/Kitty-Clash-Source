using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class UnitObject : MonoBehaviour
{
    public int hp { get; protected set; }
    public int damage { get; protected set; }
    public float speed { get; protected set; }
    public float attackInterval { get; protected set; }
    public float attackRange { get; protected set; }

    [field: SerializeField] public Unit Unit { get; private set; }
    [SerializeField] protected Transform unitTransform;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Animator animator;
    [field: SerializeField] public bool isEnemy { get; private set; }
    [field: SerializeField] public bool isFlying { get; private set; }
    [field: SerializeField] public bool isTower { get; private set; }

    [Header("Targets")]
    [SerializeField] protected bool canAttackFlying;
    [SerializeField] protected bool canAttackLand;

    protected bool canWalk;
    protected UnitObject targetEnemy;
    protected MainTower targetTower;
    protected Coroutine attackCoroutine;

    public bool isAttacking { get; protected set; }
    private bool isStunned;
    private bool isFlashing;
    protected bool isDead;

    protected virtual void Start()
    {
        var upgradeStats = Unit.GetUpgradeStats();
        hp = upgradeStats.hp;
        damage = upgradeStats.damage;

        speed = Unit.Speed;
        attackInterval = Unit.AttackInterval;
        attackRange = Unit.AttackRange;

        canWalk = true;
        isFlashing = false;
        isDead = false;

        UnitsHolder.AddUnit(this);
        EventManager.GameFinished += OnGameFinish;
    }

    protected virtual void FixedUpdate()
    {
        if (canWalk && !isAttacking)
            Walk();

        if (!isAttacking && !isDead)
            DetectEnemy();
    }

    private void Update()
    {
        Animate();
    }

    protected virtual void Walk()
    {
        if (!canWalk || isStunned)
        {
            return;
        }

        float direction = isEnemy ? -1 : 1;
        Vector3 targetLocation = new(direction * speed * Time.fixedDeltaTime * 0.02f, 0);
        transform.position += targetLocation;
    }

    protected virtual void DetectEnemy()
    {
        var unit = GetNearestEnemy();

        if (unit != null && unit != this && unit.isEnemy != isEnemy)
        {
            if (!isAttacking)
                StartAttacking(unit);
        }
    }

    protected virtual UnitObject GetNearestEnemy()
    {
        float distance = float.MaxValue;
        UnitObject nearestUnit = null;

        foreach (var unit in UnitsHolder.GetList(!isEnemy).ToArray())
        {
            if (unit == null) continue;

            var newDistance = Vector3.Distance(transform.position, unit.transform.position);
            if (newDistance < distance && newDistance <= attackRange)
            {
                if ((unit.isFlying && canAttackFlying) || (!unit.isFlying && canAttackLand) || unit.isTower)
                {
                    distance = newDistance;
                    nearestUnit = unit;
                }
            }
        }

        return nearestUnit;
    }

    protected virtual void StartAttacking(UnitObject enemy)
    {
        if (isAttacking) return;

        isAttacking = true;
        targetEnemy = enemy;
        animator.Play("idle");

        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(AttackEnemy());
        }
    }

    protected virtual IEnumerator AttackEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);

            targetEnemy.TakeDamage(damage);
            SoundsPool.Instance.PlaySoundFromPool("bites", 0.05f);
            animator.Play("attack");
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
        }
    }

    public virtual void TakeDamage(int damageAmount)
    {
        if (hp <= 0) return;

        hp -= damageAmount;
        StartCoroutine(FlashRed());
        if (hp <= 0)
            Die();
    }
    private IEnumerator FlashRed()
    {
        if (spriteRenderer == null || isFlashing) yield break;

        isFlashing = true;
        Color originalColor = spriteRenderer.color;

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        if (spriteRenderer != null && gameObject.activeInHierarchy)
        {
            spriteRenderer.color = originalColor;
        }
        isFlashing = false;
    }

    public virtual void Die()
    {
        isDead = true;
        UnitsHolder.RemoveUnit(this);

        SoundsPool.Instance.PlaySoundFromPool("meows", 0.25f);
        DeathAnimator.Instance.PlayDeathAnimation(this);
        StopAllCoroutines();
        isAttacking = false;
        canWalk = false;

        Destroy(gameObject);
    }

    public void Stun(float duration)
    {
        if (isStunned) return;
        StartCoroutine(ApplyStun(duration));
    }

    private IEnumerator ApplyStun(float duration)
    {
        isAttacking = false;
        isStunned = true;
        canWalk = false;

        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        yield return new WaitForSeconds(duration);
        isStunned = false;
        canWalk = true;
        isAttacking = false;
    }

    public void SetRenderOrder(int order)
    {
        spriteRenderer.sortingOrder = order;
    }

    protected virtual void Animate()
    {
        if (animator == null) return;

        if (!isAttacking)
        {
            animator.Play("walk");
        }
    }

    protected virtual void OnGameFinish(GameFinishStatus status)
    {
        if ((status == GameFinishStatus.win && isEnemy) || (status == GameFinishStatus.lose && !isEnemy) || status == GameFinishStatus.draw)
        {
            Die();
            return;
        }

        StopAllCoroutines();
        isDead = true;
        animator.Play("idle");
    }

    protected virtual void OnDisable()
    {
        EventManager.GameFinished -= OnGameFinish;
    }
}
