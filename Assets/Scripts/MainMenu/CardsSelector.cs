using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardsSelector : MonoBehaviour
{
    public static CardsSelector Instance { get; private set; }

    [SerializeField] private Transform cardsPanel, selectedPanel;
    [SerializeField] private MenuCard menuCardPrefab;
    [SerializeField] private Transform contentHolder;

    private MenuCard selectedAvailableCard, selectedDeckCard;
    public bool useButtonClicked { get; private set; }

    private List<Tween> selectedCardsTweens;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Clear();
        DisplayCards();
        useButtonClicked = false;
    }

    private void DisplayCards()
    {
        List<Unit> unlocked, locked;
        MainMenuManager.Instance.ArenasHolder.GetUnlockedAndLockedUnits(out unlocked, out locked);


        var selected = SavesManager.SelectedUtins;
        selectedCardsTweens = new();
        foreach(var unit in selected)
        {
            Instantiate(menuCardPrefab, selectedPanel).Setup(unit);
            selectedCardsTweens.Add(null);
        }
        foreach(var unit in unlocked.Except(selected))
        {
            Instantiate(menuCardPrefab,cardsPanel).Setup(unit);
        }
        
        foreach (var unit in locked.Except(selected))
            Instantiate(menuCardPrefab, cardsPanel).Setup(unit);
    }

    private void Clear()
    {
        var list = cardsPanel.Cast<Transform>().ToList();
        list.AddRange(selectedPanel.Cast<Transform>().ToList());
        foreach (var card in list)
            Destroy(card.gameObject);
    }

    public void OnUseButtonClick(MenuCard card)
    {
        if (card == null)
        {
            selectedAvailableCard = null;
            useButtonClicked = false;
            KillTweens();
            return;
        }

        selectedAvailableCard = card;
        useButtonClicked = true;
        KillTweens();
        contentHolder.DOMoveY(-1500, 0);
        for (int i = 0; i < selectedCardsTweens.Count; i++)
        {
            int direction = Random.Range(0, 2) == 1 ? 1 : -1;
            selectedCardsTweens[i] = selectedPanel.GetChild(i).DORotate(new(0, 0, 2 * direction), 0.085f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void KillTweens()
    {
        for (int i = 0; i < selectedCardsTweens.Count; i++)
        {
            selectedCardsTweens[i]?.Kill();
            selectedPanel.GetChild(i).rotation = Quaternion.identity;
        }
    }

    public void OnDeckCardChoose(MenuCard card)
    {
        if (SavesManager.SelectedUtins.Contains(card.GetUnit()) && selectedAvailableCard != null && useButtonClicked)
        {
            selectedDeckCard = card;
            UseSelectedCard();
        }
    }

    public void UseSelectedCard()
    {
        Unit availableUnit = selectedAvailableCard.GetUnit();
        Unit deckUnit = selectedDeckCard.GetUnit();

        int deckIndex = SavesManager.SelectedUtins.IndexOf(deckUnit);
        SavesManager.Instance.SetSelectedCardsSave(deckIndex, availableUnit);

        Clear();
        DisplayCards();

        selectedAvailableCard = null;
        selectedDeckCard = null;
        useButtonClicked = false;
    }
}
