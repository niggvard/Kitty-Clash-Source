using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoPanel : MonoBehaviour
{
    public static CardInfoPanel Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private Image unitImage;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI hpText, movingSpeedText, damageText;

    private bool isClosing;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowInfo(Unit unit)
    {
        if (panel.activeSelf) return;

        isClosing = false;
        panel.transform.DOScale(0.001f, 0);

        unitImage.sprite = unit.CardImage;
        nameText.text = unit.UnitName;
        movingSpeedText.text = "Speed: " + unit.Speed.ToString();

        var currentLevel = unit.GetUpgradeStats();
        damageText.text = "Damage: " + currentLevel.damage.ToString();
        hpText.text = "HP: " + currentLevel.hp.ToString();

        panel.SetActive(true);
        panel.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
    }

    public void ShowUpgradeStats(Unit unit)
    {
        if (panel.activeSelf) return;

        isClosing = false;
        panel.transform.DOScale(0.001f, 0);

        unitImage.sprite = unit.CardImage;
        nameText.text = unit.UnitName;
        movingSpeedText.text = "Speed: " + unit.Speed.ToString();

        var currentLevel = unit.GetUpgradeStats();
        var previousLevel = unit.GetUpgradeStats(unit.CurrentLevel - 1);

        damageText.text = "Damage: " + previousLevel.damage + " -> <color=yellow>" + currentLevel.damage + "</color>";
        hpText.text = "HP: " + previousLevel.hp + " -> <color=yellow>" + currentLevel.hp + "</color>";

        panel.SetActive(true);
        panel.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
    }

    public async void Close()
    {
        if (isClosing) return;

        isClosing = true;

        await panel.transform.DOScale(0.001f, 0.25f).SetEase(Ease.Flash).AsyncWaitForCompletion();
        panel.SetActive(false);
    }
}
