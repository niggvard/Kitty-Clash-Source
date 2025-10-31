using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStats : MonoBehaviour
{
    public static float hpModifier;
    public static float damageModifier;
    public static float cooldownModifier;

    private void Start()
    {
        hpModifier = 0;
        damageModifier = 0;
        cooldownModifier = 0;
    }
}
