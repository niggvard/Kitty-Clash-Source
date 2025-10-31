using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenasHolder : ScriptableObject
{
    public static ArenasHolder Instance { get; private set; }
    public static int CurrentTrophy
    {
        get => PlayerPrefs.GetInt("Trophy", 0);
        set
        {
            var trophy = value;
            var minValue = Instance.ArenaList[Instance.CurrentArenaIndex].TrophyRequirement;
            if (trophy < minValue)
                trophy = minValue;
            PlayerPrefs.SetInt("Trophy", trophy);
        }
    }
    public static Arena CurrentArenaInfo
    {
        get => Instance.ArenaList[Instance.CurrentArenaIndex];
    }
    [field: SerializeField] public Arena[] ArenaList { get; private set; }
    public int CurrentArenaIndex
    {
        get
        {
            int trophy = CurrentTrophy;
            var arenas = ArenaList;
            for (int i = arenas.Length - 1; i >= 0; i--)
            {
                if (trophy >= arenas[i].TrophyRequirement)
                    return i;
            }

            return 0;
        }
    }
    public List<Unit> AllUnits
    {
        get
        {
            List<Unit> units = new();
            foreach (var arena in ArenaList)
            {
                foreach (var unit in arena.UnlockableUnits)
                    units.Add(unit);
            }
            return units;
        }
    }

    public void GetUnlockedAndLockedUnits(out List<Unit> unlocked, out List<Unit> locked)
    {
        unlocked = new();
        locked = new();

        foreach (var arena in ArenaList)
        {
            foreach (var unit in arena.UnlockableUnits)
            {
                if (unit.IsUnlocked)
                    unlocked.Add(unit);
                else
                    locked.Add(unit);
            }
        }
    }

    public List<Unit> GetAvaliableUnitsOfRarity(Rarities rarity)
    {
        List<Unit> list = new();
        for (int i = 0; i <= CurrentArenaIndex; i++)
        {
            foreach (var unit in ArenaList[i].UnlockableUnits)
            {
                if (unit.Rarity == rarity)
                    list.Add(unit);
            }
        }

        return list;
    }

    public void CreateInstance()
    {
        Instance = this;
    }
}

[System.Serializable]
public struct Arena
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int TrophyRequirement { get; private set; }
    [field: SerializeField] public Unit[] UnlockableUnits { get; private set; }
    [field: SerializeField] public ChestChance[] ChestsList { get; private set; }
    [field: SerializeField] public Sprite arenaImage { get; private set; }
    [field: SerializeField] public Sprite GameplayBackground { get; private set; }

    public ChestSettings GetChest()
    {
        var chest = Randomizer.GetItemByChance(ChestsList);
        return chest.Chest;
    }

    [System.Serializable]
    public struct ChestChance : IChanceHolder
    {
        [field: SerializeField] public ChestSettings Chest { get; private set; }
        [field: SerializeField] public float chance { get; private set; }
    }
}