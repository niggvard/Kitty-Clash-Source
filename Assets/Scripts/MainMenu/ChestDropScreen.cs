using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestDropScreen : MonoBehaviour
{
    public static ChestDropScreen Instance { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI cardCountText;
    [SerializeField] private Sprite moneySprite;
    [SerializeField] private AudioSource dropSound;

    private List<DropInfo> dropQueue;
    private int moneyDrop;
    private Vector3 smallSize = new(0.001f, 0.001f, 0.001f);
    private int currentIndex;
    private bool canSkip;

    private void Awake()
    {
        Instance = this;
        dropQueue = new();
    }

    public void Skip()
    {
        if (!canSkip) return;

        if (currentIndex >= dropQueue.Count)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            dropQueue = new();

            return;
        }

        dropSound.Stop();
        dropSound.Play();
        StartCoroutine(ShowReward());
    }

    public void AddDrop(Sprite image, int count)
    {
        DropInfo drop = new()
        {
            unitImage = image,
            count = count
        };
        dropQueue.Add(drop);
    }

    public void AddMoneyDrop(int money)
    {
        AddDrop(moneySprite, money);
    }

    public void Show()
    {
        currentIndex = 0;
        canSkip = true;

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        Skip();
    }

    private IEnumerator ShowReward()
    {
        canSkip = false;

        cardImage.sprite = dropQueue[currentIndex].unitImage;
        cardCountText.text = dropQueue[currentIndex].count.ToString() +"x";

        cardImage.transform.localScale = smallSize;
        cardCountText.DOFade(0, 0);
        yield return cardImage.transform.DOScale(1, 0.2f).SetEase(Ease.Flash).WaitForCompletion();
        cardCountText.DOFade(1, 0);

        currentIndex++;

        canSkip = true;
    }

    private struct DropInfo
    {
        public Sprite unitImage;
        public int count;
    }
}
