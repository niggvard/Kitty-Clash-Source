using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform[] enemySpawnPositions;
    [SerializeField] private GameTimer gameTimer;
    private List<Unit> selectedUnits;

    private const float baseTimeDivider = 0.8f;
    private const float minTimeDivider = 0.6f;
    private const float maxTimeDivider = 1.8f;
    private float currentTimeDivider;


    private void Start()
    {
        if (ArenasHolder.Instance.CurrentArenaIndex == 0)
            currentTimeDivider = baseTimeDivider;
        else
            currentTimeDivider = SavesManager.CurrentTimeDivider;

        print(currentTimeDivider);

        SelectUnits();
        StartCoroutine(UnitsSpawner());
        EventManager.GameFinished += OnGameFinish;
    }

    private void SelectUnits()
    {
        selectedUnits = new();
        List<Unit> alreadySelected = new();

        List<Unit> unlocked, locked;
        ArenasHolder.Instance.GetUnlockedAndLockedUnits(out unlocked, out locked);

        for (int i = 0; i < 3; i++)
        {
            var unit = Randomizer.GetRandomFromList(unlocked.Except(alreadySelected).ToList());
            selectedUnits.Add(unit);
            alreadySelected.Add(unit);
        }

        //foreach (var e in selectedUnits)
        //{
        //    print(e.UnitName);
        //}
    }

    private IEnumerator UnitsSpawner()
    {
        var arena = ArenasHolder.Instance.CurrentArenaIndex;

        while (true)
        {
            int spawnCount = Randomizer.GetItemByChance(LevelLoader.puzzlePrefab.chances).count;
            if (arena == 0)
                spawnCount = 1;
            else if (arena == 1)
                spawnCount = Mathf.Clamp(spawnCount, 1, 3);


            Unit unit = Randomizer.GetRandomFromList(selectedUnits);
            yield return new WaitForSeconds(unit.SpawnCooldown / currentTimeDivider);
            for (int i = 0; i < spawnCount; i++)
            {
                int positionIndex = Random.Range(0, enemySpawnPositions.Length);
                var spawnPos = enemySpawnPositions[positionIndex];
                Instantiate(unit.enemyPrefab, spawnPos.position, Quaternion.identity).SetRenderOrder(positionIndex + 5);

                if(i < spawnCount - 1)
                {
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }
    }

    private void OnDisable()
    {
        EventManager.GameFinished -= OnGameFinish;
    }

    private void OnGameFinish(GameFinishStatus status)
    {
        StopAllCoroutines();

        if (status == GameFinishStatus.win)
        {
            currentTimeDivider = Mathf.Min(currentTimeDivider + 0.15f, maxTimeDivider);
        }
        else if (status == GameFinishStatus.lose)
        {
            if (currentTimeDivider > 1.2f)
                currentTimeDivider = 1.2f;
            else 
                currentTimeDivider -= 0.2f;

            currentTimeDivider = Mathf.Max(currentTimeDivider, minTimeDivider);
        }
        else if (status == GameFinishStatus.draw)
        {
            currentTimeDivider -= 0.1f;
            currentTimeDivider = Mathf.Max(currentTimeDivider, minTimeDivider);
        }

        SavesManager.CurrentTimeDivider = currentTimeDivider;

        StartCoroutine(UnitsSpawner());
    }
}