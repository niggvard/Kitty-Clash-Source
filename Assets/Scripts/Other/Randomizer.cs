using System.Collections.Generic;
using UnityEngine;

public interface IChanceHolder
{
    public float chance { get; }
}

public static class Randomizer
{
    public static T GetItemByChance<T>(T[] items) where T : IChanceHolder
    {
        float totalChance = 0f;

        foreach (var item in items)
        {
            totalChance += item.chance;
        }

        float randomValue = Random.Range(0f, totalChance);

        float cumulativeChance = 0f;
        foreach (var item in items)
        {
            cumulativeChance += item.chance;
            if (randomValue <= cumulativeChance)
            {
                return item;
            }
        }

        return items[0];
    }

    public static T GetRandomFromList<T>(T[] array)
    {
        int index = Random.Range(0, array.Length);
        return array[index];
    }

    public static T GetRandomFromList<T>(List<T> array)
    {
        int index = Random.Range(0, array.Count);
        return array[index];
    }
}