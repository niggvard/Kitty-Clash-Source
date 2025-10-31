using System;

public class EventManager
{
    public static event Action<GameFinishStatus> GameFinished;

    public static void InvokeGameFinish(GameFinishStatus status)
    {
        if (!LevelLoader.IsGameFinished)
            GameFinished?.Invoke(status);
    }
}

public enum GameFinishStatus
{
    win, lose, draw
}