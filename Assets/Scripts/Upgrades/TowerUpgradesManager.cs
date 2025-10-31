using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TowerUpgrade;

public class TowerUpgradesManager : MonoBehaviour
{
    public static TowerUpgradesManager Instance { get; private set; }

    [SerializeField] private List<UpgradeButtonUI> upgradeButtons;
    [SerializeField] private ScrollRect scrollRect;

    [field: SerializeField] public Color colorLocked { get; private set; }
    [field: SerializeField] public Color colorUnlocked { get; private set; }
    [field: SerializeField] public Color colorActive { get; private set; }

    public static event Action<int> CurrencyChanged;

    public static int CurrentCurrency
    {
        get => PlayerPrefs.GetInt("TowerUpgradeMoney", 1);
        set => PlayerPrefs.SetInt("TowerUpgradeMoney", value);
    }

    private void Awake()
    {
        Instance = this;

        int upgrade = PlayerPrefs.GetInt("TowerUpgrade", -1);

        if (upgrade < upgradeButtons.Count - 1)
        {
            upgradeButtons[upgrade + 1].canUpgrade = true;
            ScrollToTarget(upgradeButtons[upgrade + 1].GetComponent<RectTransform>());
        }
        else
            ScrollToTarget(upgradeButtons[upgrade].GetComponent<RectTransform>());
    }

    private void ScrollToTarget(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();
        Vector2 viewPortLocalPosition = scrollRect.viewport.localPosition;
        Vector2 targetLocalPosition = target.localPosition;

        Vector2 newTargetLocalPosition = new Vector2(
            scrollRect.content.localPosition.x,
            -(viewPortLocalPosition.y + targetLocalPosition.y) +
            (scrollRect.viewport.rect.height / 2) -
            (target.rect.height / 2) - 600
        );

        scrollRect.content.localPosition = newTargetLocalPosition;
    }

    public void OnUpgrade(UpgradeButtonUI button)
    {
        var index = upgradeButtons.IndexOf(button);
        PlayerPrefs.SetInt("TowerUpgrade", index);
        if (index < upgradeButtons.Count - 1)
        {
            upgradeButtons[index + 1].Unlock();
            ScrollToTarget(upgradeButtons[index + 1].GetComponent<RectTransform>());
        }
    }

    public bool PurchaseUpgrade(TowerUpgrade perk)
    {
        if (CurrentCurrency < perk.cost)
            return false;
        CurrentCurrency -= perk.cost;
        CurrencyChanged?.Invoke(CurrentCurrency);

        ApplyModifier(perk);
        return true;
    }

    private void ApplyModifier(TowerUpgrade perk)
    {
        switch (perk.Category)
        {
            case TowerModifier.HP:
                TowerStats.hpModifier += perk.value;
                break;
            case TowerModifier.Damage:
                TowerStats.damageModifier += perk.value;
                break;
            case TowerModifier.Cooldown:
                TowerStats.cooldownModifier -= perk.value;
                break;
        }
    }

    public void AddCurrency(int amount)
    {
        CurrentCurrency += amount;
        CurrencyChanged?.Invoke(CurrentCurrency);
    }
}
