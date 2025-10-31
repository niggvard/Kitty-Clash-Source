using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public static bool IsGameFinished { get; private set; }

    public static PuzzleMap puzzlePrefab;
    [SerializeField] private SpriteRenderer backgroundImage;
    [SerializeField] private PuzzleMap defaultPuzzle;

    private void Awake()
    {
        Analytics.OnGameStart(ArenasHolder.Instance.CurrentArenaIndex);
        UnitsHolder.InitializeLists();

        var background = ArenasHolder.Instance.ArenaList[ArenasHolder.Instance.CurrentArenaIndex].GameplayBackground;
        backgroundImage.sprite = background;

        if (puzzlePrefab == null)
            puzzlePrefab = defaultPuzzle;

        Instantiate(puzzlePrefab);

        IsGameFinished = false;
        EventManager.GameFinished += FinishGame;
    }

    private void OnDisable()
    {
        EventManager.GameFinished -= FinishGame;
    }

    private void FinishGame(GameFinishStatus status)
    {
        IsGameFinished = true;
    }
}

public static class UnitsHolder
{
    private static List<UnitObject> playerUnits, enemyUnits;

    public static void InitializeLists()
    {
        playerUnits = new();
        enemyUnits = new();
    }

    public static void AddUnit(UnitObject unit)
    {
        if (unit.isEnemy)
            enemyUnits.Add(unit);
        else
            playerUnits.Add(unit);
    }

    public static void RemoveUnit(UnitObject unit)
    {
        if (unit.isEnemy)
            enemyUnits.Remove(unit);
        else
            playerUnits.Remove(unit);
    }

    public static List<UnitObject> GetList(bool isEnemy)
    {
        if (isEnemy)
            return enemyUnits;
        else
            return playerUnits;
    }
}