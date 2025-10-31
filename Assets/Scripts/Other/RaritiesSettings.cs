using UnityEngine;

public class RaritiesSettings : ScriptableObject
{
    public static RaritiesSettings Instance { get; private set; }

    [field: SerializeField] public int[] UpgradeCardRequirements { get; private set; }
    [field: SerializeField] public Rarity[] Rarities { get; private set; }

    public void CreateInstance()
    {
        Instance = this;
    }
}

[System.Serializable]
public class Rarity
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Color Color { get; private set; }
    [field: SerializeField] public int[] UpgradeMoneyRequirements { get; private set; }

    public static Rarity GetRarity(Rarities rarity)
    {
        int index = (int)rarity;

        return RaritiesSettings.Instance.Rarities[index];
    }
}

public enum Rarities
{
    common, rare, epic, legendary
}