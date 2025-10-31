using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private UnitObject[] enemies;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnRandomEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    //я знаю что будем переписывать по умнее делать спавнер, это на первое время

    private void SpawnRandomEnemy()
    {
        int randomIndex = Random.Range(0, enemies.Length);
        UnitObject selectedEnemy = enemies[randomIndex];
        Instantiate(selectedEnemy, spawnPoint.position, Quaternion.identity);
    }
}
