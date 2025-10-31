using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;

    private void Start()
    {
        TowerUpgradesManager.CurrencyChanged += UpdateCurrencyUI;
        UpdateCurrencyUI(TowerUpgradesManager.CurrentCurrency);
    }

    private void OnDisable()
    {
        TowerUpgradesManager.CurrencyChanged -= UpdateCurrencyUI;
    }

    private void UpdateCurrencyUI(int newCurrency)
    {
        currencyText.text = $"Upgrade Points: {newCurrency}";
    }
}
