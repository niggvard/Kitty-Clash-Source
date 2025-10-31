using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMap : MonoBehaviour
{
    [field: SerializeField] public Sprite puzzleImage { get; private set; }
    [field: SerializeField] public EnemySpawnChances[] chances { get; private set; }

    public int GetRandomSpawnCount()
    {
        var chance = Randomizer.GetItemByChance(chances);
        return chance.count;
    }

    [System.Serializable]
    public class EnemySpawnChances : IChanceHolder
    {
        [field: SerializeField] public int count { get; private set; }
        [field: SerializeField] public float chance { get; private set; }
    }
}
