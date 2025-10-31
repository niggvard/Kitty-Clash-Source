using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayTutorial : MonoBehaviour
{
    [SerializeField] private Transform tutorialMask;
    [SerializeField] private Transform tutorialHand;

    [Header("Phase 0")]
    [SerializeField] private Transform dropperMaskPos;
    [SerializeField] private Transform dropperHandPos1, dropperHandPos2;
    [SerializeField] private Transform cardMaskPos, cardHandPos1, cardHandPos2;
    [SerializeField] private Button cardButton;
    [SerializeField] private CanvasGroup cardsCanvasGroup;

    private int phase;

    private void Start()
    {
        if (!TutorialManager.IsTutorialActive)
        {
            Destroy(gameObject);
            return;
        }

        phase = TutorialManager.TutorialPhase;

        if (phase == 0)
            Phase0();
    }

    private void OnEnable()
    {
        Dropper.BallSpawned += OnBallSpawn;
    }

    private void OnDisable()
    {
        Dropper.BallSpawned -= OnBallSpawn;
    }

    #region Phase0
    private bool dropperMoved = false;
    private void Phase0()
    {
        cardsCanvasGroup.interactable = false;
        cardButton.onClick.AddListener(OnCardClick);

        Time.timeScale = 0;
        tutorialMask.gameObject.SetActive(true);
        tutorialMask.position = dropperMaskPos.position;

        tutorialHand.gameObject.SetActive(true);
        tutorialHand.DOKill();
        tutorialHand.position = dropperHandPos1.position;
        tutorialHand.DOMove(dropperHandPos2.position, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
    }

    public void OnDropperMoved()
    {
        if (dropperMoved) return;

        dropperMoved = true;
        Time.timeScale = 1;
        tutorialMask.gameObject.SetActive(false);
        tutorialHand.gameObject.SetActive(false);
    }

    private void OnBallSpawn(SpawnBall ball)
    {
        Dropper.BallSpawned -= OnBallSpawn;

        Time.timeScale = 0;
        tutorialMask.gameObject.SetActive(true);
        tutorialMask.position = cardMaskPos.position;

        tutorialHand.gameObject.SetActive(true);
        tutorialHand.DOKill();
        tutorialHand.position = cardHandPos1.position;
        tutorialHand.DOMove(cardHandPos2.position, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
        cardsCanvasGroup.interactable = true;
    }

    private void OnCardClick()
    {
        cardButton.onClick.RemoveListener(OnCardClick);

        Time.timeScale = 1;
        tutorialMask.gameObject.SetActive(false);
        tutorialHand.gameObject.SetActive(false);
    }
    #endregion
}
