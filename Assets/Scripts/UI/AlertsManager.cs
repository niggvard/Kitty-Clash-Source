using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertsManager : MonoBehaviour
{
    [SerializeField] private GameObject chestAlert;
    [SerializeField] private GameObject treeUpgradeAlert;

    void Start()
    {
        Chest.ChestOpen += OnChestOpen;
        TowerUpgradesManager.CurrencyChanged += CheckForTreeUpgrades;
        OnChestOpen();
        CheckForTreeUpgrades();
    }

    private void OnDisable()
    {
        Chest.ChestOpen -= OnChestOpen;
        TowerUpgradesManager.CurrencyChanged -= CheckForTreeUpgrades;
    }

    private void OnChestOpen(ChestSettings settings = null)
    {
        if (ChestManager.GetChestCount() > 0)
            chestAlert.SetActive(true);
        else
            chestAlert.SetActive(false);
    }

    private void CheckForTreeUpgrades(int newValue = 0)
    {
        if (TowerUpgradesManager.CurrentCurrency > 0)
            treeUpgradeAlert.SetActive(true);
        else
            treeUpgradeAlert.SetActive(false);
    }
}
