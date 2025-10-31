using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI upgradeDescription;
    [SerializeField] private Image icon;
    [SerializeField] private AudioSource buySound;

    [Header("Upgrade Settings")]
    [SerializeField] private TowerUpgrade selectedUpgrade;

    public bool canUpgrade;

    private void Start()
    {
        UpdateButtonUI();
        upgradeButton.onClick.AddListener(PurchaseUpgrade);
        icon.sprite = selectedUpgrade.icon;

        if (!canUpgrade)
        {
            upgradeButton.interactable = false;
            if (selectedUpgrade.isUnlocked)
            {
                upgradeButton.image.color = TowerUpgradesManager.Instance.colorUnlocked;
            }
            else
                upgradeButton.image.color = TowerUpgradesManager.Instance.colorLocked;
        }
        else
            upgradeButton.image.color = TowerUpgradesManager.Instance.colorActive;
    }

    private void UpdateButtonUI()
    {
        costText.text = selectedUpgrade.cost.ToString();
        upgradeDescription.text = selectedUpgrade.description.ToString();
    }

    private void PurchaseUpgrade()
    {
        if (!canUpgrade) return;
        if (TowerUpgradesManager.Instance.PurchaseUpgrade(selectedUpgrade))
        {
            selectedUpgrade.isUnlocked = true;
            upgradeButton.image.color = TowerUpgradesManager.Instance.colorUnlocked;
            upgradeButton.interactable = false;
            TowerUpgradesManager.Instance.OnUpgrade(this);
            buySound.Play();
        }
    } 

    public void Unlock()
    {
        canUpgrade = true;
        upgradeButton.interactable = true;
        upgradeButton.image.color = TowerUpgradesManager.Instance.colorActive;
    }
}
