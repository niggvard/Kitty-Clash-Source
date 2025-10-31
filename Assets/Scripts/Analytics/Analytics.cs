using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Analytics 
{
    public static void OnGameStart(int levelNumber)
    {
        TinySauce.OnGameStarted(levelNumber);
    }

    public static void OnGameFinished(bool isUserCompleteLevel, int trophies, int levelNumber)
    {
        TinySauce.OnGameFinished(isUserCompleteLevel, trophies, levelNumber);
    }

    public static void OnUpgrade(string upgradeName, int upgradeLevel)
    {
        TinySauce.OnUpgradeEvent(upgradeName, upgradeLevel);
    }
}
