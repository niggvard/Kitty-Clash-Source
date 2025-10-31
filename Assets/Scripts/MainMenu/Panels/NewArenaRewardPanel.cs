using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewArenaRewardPanel : PanelUI<Arena>
{
    [Header("New Arena Settings")]
    [SerializeField] private Image arenaImage;
    [SerializeField] private TextMeshProUGUI arenaNameText;
    [SerializeField] private MenuCard cardPrefab;
    [SerializeField] private Transform cardsParent;

    protected override void Start()
    {
        base.Start();

        int currentArenaIndex = ArenasHolder.Instance.CurrentArenaIndex;
        if (PlayerPrefs.GetInt("LastArena", 0) < currentArenaIndex)
        {
            TowerUpgradesManager.CurrentCurrency++;
            ShowPanel(ArenasHolder.CurrentArenaInfo);
            PlayerPrefs.SetInt("LastArena", currentArenaIndex);
        }
        else
            Destroy(gameObject);
    }

    public override void ShowPanel(Arena arena)
    {
        arenaImage.sprite = arena.arenaImage;
        arenaNameText.text = arena.Name;

        foreach (var unit in arena.UnlockableUnits)
        {
            Instantiate(cardPrefab, cardsParent).Setup(unit);
        }

        base.ShowPanel(arena);
    }
}
