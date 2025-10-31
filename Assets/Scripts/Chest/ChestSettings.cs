using UnityEngine;

[CreateAssetMenu(fileName = "ChestSettings", menuName = "CustomAssets/ChestSettings", order = 1)]
public class ChestSettings : ScriptableObject
{
    [field: SerializeField] public string chestName { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public int SecondsToOpen { get; private set; }
    [field: SerializeField] public int DropNumber { get; private set; }
    [field: SerializeField] public int MinDrop { get; private set; }
    [field: SerializeField] public int MaxDrop { get; private set; }
    [field: SerializeField] public int MinMoneyDrop { get; private set; }
    [field: SerializeField] public int MaxMoneyDrop { get; private set; }
    [field: SerializeField] public ChestRarityChance[] chances { get; private set; }
}

[System.Serializable]
public struct ChestRarityChance : IChanceHolder
{
    [field: SerializeField] public Rarities Rarity { get; private set; }
    [field: SerializeField] public float chance { get; private set; }
}