using DG.Tweening;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuCard : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private GameObject higlightBackground;
    [SerializeField] private Unit unit;

    [Header("Text Objects")]
    [SerializeField] private TextMeshProUGUI upgradeProgressText;
    [SerializeField] private TextMeshProUGUI levelText, moneyRequirementText, timeCostText;

    [Header("Buttons")]
    [SerializeField] private Button upgradeButton;
    [field: SerializeField] public GameObject useButton { get; private set; }

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound, failSound, upgradeSound;

    public static event Action<MenuCard> Selecting;

    public bool CanUpgrade
    {
        get
        {
            if (unit.IsMaxLevel)
                return false;

            int cards = unit.CurrentCardsOwned;
            int money = SavesManager.Money;

            if (cards >= unit.CardsRequirement && money >= unit.MoneyRequirement)
                return true;

            return false;
        }
    }

    public MenuCard Setup(Unit Unit)
    {
        unit = Unit;
        unit.menuCard = this;

        timeCostText.text = unit.SpawnCooldown.ToString();
        cardImage.sprite = unit.CardImage;
        higlightBackground.SetActive(false);

        UpdateCardsNumber();

        return this;
    }

    public void UpdateCardsNumber()
    {
        if(unit == null) return;

        var cardsNumber = unit.CurrentCardsOwned;

        levelText.text = "LV. " + unit.CurrentLevel;

        if (unit.CurrentLevel > 0)
        {
            if (!unit.IsMaxLevel)
            {
                var cardsRequirement = unit.CardsRequirement;
                upgradeProgressText.text = cardsNumber + "/" + cardsRequirement;
            }
            else
                upgradeProgressText.text = "MAX";
        }
        else
            upgradeProgressText.text = "Locked";
    }

    public void OnButtonClick()
    {
        if (unit.CurrentLevel > 0)
            CardsSelector.Instance.OnUseButtonClick(this);
    }

    public void Highlight()
    {
        if (unit.CurrentLevel < 1)
        {
            PlaySound(failSound);
            VibrationManager.Vibrate(0.2f);
            return;
        }

        PlaySound(clickSound);
        useButton.SetActive(true);

        if (SavesManager.SelectedUtins.Contains(unit))
        {
            if (CardsSelector.Instance.useButtonClicked)
                CardsSelector.Instance.OnDeckCardChoose(this);
            else
            {
                Selecting?.Invoke(this);
                AnimateHighlight();
                useButton.SetActive(false);
            }
        }
        else
        {
            Selecting?.Invoke(this);
            AnimateHighlight();
        }

        if (CanUpgrade)
            upgradeButton.image.color = Color.yellow;
        else
            upgradeButton.image.color = Color.white;

        if (!unit.IsMaxLevel)
            moneyRequirementText.text = unit.MoneyRequirement + "$";
        else
            moneyRequirementText.text = "MAX";
    }

    public void Deselect()
    {
        higlightBackground.SetActive(false);
        CardsSelector.Instance.OnUseButtonClick(null);
    }

    private async void AnimateHighlight()
    {
        float defaultScale = transform.localScale.x;
        await transform.DOScaleX(0, 0.1f).AsyncWaitForCompletion();
        higlightBackground.SetActive(true);
        transform.DOScaleX(defaultScale, 0.1f);
    }

    public void GetInfo()
    {
        CardInfoPanel.Instance.ShowInfo(unit);
    }

    public void Upgrade()
    {
        if (!CanUpgrade)
        {
            PlaySound(failSound);
            return;
        }

        PlaySound(upgradeSound);
        int cards = unit.CurrentCardsOwned;
        int money = SavesManager.Money;
        money -= unit.MoneyRequirement;
        cards -= unit.CardsRequirement;

        unit.CurrentLevel++;
        SavesManager.Money = money;
        unit.CurrentCardsOwned = cards;
        UpdateCardsNumber();
        CardInfoPanel.Instance.ShowUpgradeStats(unit);

        higlightBackground.SetActive(false);
        VibrationManager.Vibrate(0.5f);
    }

    private void OnSelected(MenuCard card)
    {
        if (card == this) return;

        Deselect();
    } 

    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    public Unit GetUnit() => unit;

    private void OnEnable()
    {
        Selecting += OnSelected;
    }

    private void OnDisable()
    {
        Selecting -= OnSelected;
        higlightBackground.SetActive(false);
        unit.menuCard = null;
    }
}