using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerUpgrade", menuName = "Upgrades/Tower Upgrade", order = 0)]
public class TowerUpgrade : ScriptableObject
{
    [field: SerializeField] public TowerModifier Category { get; private set; }
    [field: SerializeField] public int cost { get; private set; }
    [field: SerializeField] public float value { get; private set; }
    [field: SerializeField] public string description { get; private set; }
    [field: SerializeField] public Sprite icon { get; private set; }
    public bool isUnlocked
    {
        get => PlayerPrefs.GetInt(name + "TowerUpgrade", 0) == 1 ? true : false;
        set => PlayerPrefs.SetInt(name + "TowerUpgrade", value ? 1 : 0);
    }

    public enum TowerModifier
    {
        HP,
        Damage,
        Cooldown
    }
}
