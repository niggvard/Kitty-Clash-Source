using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static int TutorialPhase
    {
        get => PlayerPrefs.GetInt("TutorialPhase", 0);
        set => PlayerPrefs.SetInt("TutorialPhase", value);
    }
    public static bool IsTutorialActive => TutorialPhase < 2;

    [SerializeField] private Transform tutorialMask;
    [SerializeField] private Transform tutorialHand;
    private static bool isSubscribed = false;
    private Button[] allButtons;

    [Header("Phase 0")]
    [SerializeField] private GameObject musicHolder;

    [Header("Phase 1")]
    [SerializeField] private ScrollRect scrollView;
    [SerializeField] private Button cardsPanelButton;
    [SerializeField] private Transform cardsPanelMaskPos, cardsPanelHandPos1, cardsPanelHandPos2;
    [SerializeField] private Unit unlockableUnit;
    [SerializeField] private Transform unitMaskPos, unitHandPos1, unitHandPos2;
    [SerializeField] private Transform selectedUnitsPanel;
    [SerializeField] private Transform oldUnitMaskPos, oldUnitHandPos1, oldUnitHandPos2;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Transform mainMenuMaskPos, mainMenuHandPos1, mainMenuHandPos2;
    [SerializeField] private Button playButton;
    [SerializeField] private Transform playMaskPos, playHandPos1, playHandPos2;

    private void Start()
    {
        if (!IsTutorialActive)
        {
            Destroy(gameObject);
            return;
        }

        allButtons = FindObjectsByType<Button>(FindObjectsSortMode.None);
        var phase = TutorialPhase;
        if (phase == 0)
            Phase0();
        else if (phase == 1)
            Phase1();
    }

    private static void OnGameFinished(GameFinishStatus status)
    {
        if (IsTutorialActive)
            TutorialPhase++;
    }

    private void OnEnable()
    {
        if (!isSubscribed)
        {
            EventManager.GameFinished += OnGameFinished;
            isSubscribed = true;
        }
    }

    private void HighlightButton(Button activeButton)
    {
        foreach (var button in allButtons)
            button.interactable = false;

        activeButton.interactable = true;
    }

    private void Phase0()
    {
        Destroy(musicHolder);
        MainMenuManager.Instance.LoadScene(1);
    }

    #region Phase0

    private void Phase1()
    {
        scrollView.vertical = false;

        unlockableUnit.CurrentCardsOwned = 1;
        unlockableUnit.CurrentLevel = 1;
        unlockableUnit.menuCard.UpdateCardsNumber();

        cardsPanelButton.onClick.AddListener(OnCardsPanelClicked);
        HighlightButton(cardsPanelButton);

        tutorialMask.gameObject.SetActive(true);
        tutorialHand.gameObject.SetActive(true);

        tutorialMask.position = cardsPanelMaskPos.position;
        tutorialHand.DOKill();
        tutorialHand.position = cardsPanelHandPos1.position;
        tutorialHand.DOMove(cardsPanelHandPos2.position, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnCardsPanelClicked()
    {
        cardsPanelButton.onClick.RemoveListener(OnCardsPanelClicked);

        tutorialMask.position = unitMaskPos.position;
        tutorialHand.DOKill();
        tutorialHand.position = unitHandPos1.position;
        tutorialHand.DOMove(unitHandPos2.position, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        cardButton = unlockableUnit.menuCard.transform.GetChild(0).GetComponent<Button>();
        HighlightButton(cardButton);
        cardButton.onClick.AddListener(OnCardClicked);
    }

    Button cardButton, useButton;
    private void OnCardClicked()
    {
        cardButton.onClick.RemoveListener(OnCardClicked);

        tutorialHand.gameObject.SetActive(false);

        useButton = unlockableUnit.menuCard.useButton.GetComponent<Button>();
        HighlightButton(useButton);
        useButton.onClick.AddListener(OnUseButton);

        useButton.transform.DOScale(1.8f, 0.2f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnUseButton()
    {
        scrollView.vertical = true;
        useButton.onClick.RemoveListener(OnUseButton);

        tutorialHand.gameObject.SetActive(true);
        tutorialHand.DOKill();

        tutorialMask.position = oldUnitMaskPos.position;
        tutorialHand.position = oldUnitHandPos1.position;
        tutorialHand.DOMove(oldUnitHandPos2.position, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        var changeButton = selectedUnitsPanel.GetChild(0).GetChild(0).GetComponent<Button>();
        HighlightButton(changeButton);
        changeButton.onClick.AddListener(OnChanged);
    }

    private void OnChanged()
    {
        HighlightButton(mainMenuButton);

        tutorialMask.position = mainMenuMaskPos.position;
        tutorialHand.DOKill();
        tutorialHand.position = mainMenuHandPos1.position;
        tutorialHand.DOMove(mainMenuHandPos2.position, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        mainMenuButton.onClick.AddListener(OnMainMenu);
    }

    private void OnMainMenu()
    {
        tutorialMask.position = playMaskPos.position;
        tutorialHand.DOKill();
        tutorialHand.position = playHandPos1.position;
        tutorialHand.DOMove(playHandPos2.position, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        HighlightButton(playButton);
        playButton.onClick.AddListener(OnPlay);
    }

    private void OnPlay()
    {
        playButton.onClick.RemoveListener(OnMainMenu);

        tutorialMask.gameObject.SetActive(false);
        tutorialHand.gameObject.SetActive(false);

        TutorialPhase++;
    }

    #endregion
}
