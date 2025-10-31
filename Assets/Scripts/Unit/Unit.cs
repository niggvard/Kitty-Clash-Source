using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "CustomAssets/Unit", order = 0)]
public class Unit : ScriptableObject
{
    [field: SerializeField] public Sprite spawnBallImage { get; private set; }
    [field: SerializeField] public Sprite CardImage { get; private set; }
    [field: SerializeField] public float SpawnCooldown { get; private set; }
    [field: SerializeField] public UnitObject unitPrefab { get; private set; }
    [field: SerializeField] public UnitObject enemyPrefab { get; private set; }
    [field: SerializeField] public Sprite soulSprite { get; private set; }

    [field: SerializeField] public string UnitName { get; private set; }
    [field: SerializeField] public Rarities Rarity { get; private set; }
    [field: SerializeField] public int HP { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float AttackInterval { get; protected set; }
    [field: SerializeField] public float AttackRange { get; protected set; }

    [HideInInspector] public MenuCard menuCard;

    public bool IsUnlocked => CurrentLevel > 0;
    public bool IsMaxLevel => CurrentLevel >= RaritiesSettings.Instance.UpgradeCardRequirements.Length;
    public int CardsRequirement => RaritiesSettings.Instance.UpgradeCardRequirements[CurrentLevel];
    public int MoneyRequirement => global::Rarity.GetRarity(Rarity).UpgradeMoneyRequirements[CurrentLevel];

    public int CurrentLevel
    {
        get => PlayerPrefs.GetInt(UnitName + "Level", 0);
        set
        {
            PlayerPrefs.SetInt(UnitName + "Level", value);
            Analytics.OnUpgrade(UnitName, value);
            menuCard?.UpdateCardsNumber();
        }
    }
    public int CurrentCardsOwned
    {
        get => PlayerPrefs.GetInt(UnitName + "Cards", 0);
        set
        {
            PlayerPrefs.SetInt(UnitName + "Cards", value);
            menuCard?.UpdateCardsNumber();
        }
    }

    public UpgradeStats GetUpgradeStats(int level = -1)
    {
        if (level == -1)
            level = CurrentLevel;

        var hp = HP;
        var damage = Damage;

        int percent = 0;
        for (int i = 1; i < level; i++)
        {
            percent += 5;
        }

        hp += (int)(((float)hp / 100) * percent);
        damage += (int)(((float)damage / 100) * percent);

        return new(hp, damage);
    }

    public class UpgradeStats
    {
        public readonly int hp, damage;

        public UpgradeStats(int HP, int Damage)
        {
            hp = HP;
            damage = Damage;
        }
    }
}