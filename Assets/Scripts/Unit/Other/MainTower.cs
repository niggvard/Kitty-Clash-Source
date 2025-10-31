using UnityEngine;
using TMPro;
using System.Collections;

public class MainTower : RangeUnit
{
    [Header("Main Tower Settings")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private GameObject destroyEffect;

    int maxHP;

    protected override void Start()
    {
        base.Start();

        if (!isEnemy)
        {
            hp += (int)TowerStats.hpModifier;
            damage += (int)TowerStats.damageModifier;
            attackInterval += TowerStats.cooldownModifier;
        }

        maxHP = hp;
        hpText.text = hp.ToString() + "/" + maxHP;
    }

    protected override Vector3 GetBulletSpawnOffset()
    {
        return new Vector3(0, 0.5f, 0); 
    }

    public override void TakeDamage(int damageAmount)
    {
        base.TakeDamage(damageAmount);

        hpText.text = hp.ToString() + "/" + maxHP;
    }

    public override void Die()
    {
        base.Die();
        Instantiate(destroyEffect, transform.position, Quaternion.identity);

        var status = isEnemy ? GameFinishStatus.win : GameFinishStatus.lose;
        EventManager.InvokeGameFinish(status);
    }

    protected override void Walk() { }

}
