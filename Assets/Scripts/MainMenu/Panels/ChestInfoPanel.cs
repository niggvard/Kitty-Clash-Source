using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestInfoPanel : MonoBehaviour
{
    public static ChestInfoPanel Instance { get; private set; }

    [SerializeField] private GameObject panel, openButton;
    [SerializeField] private Image chestImage;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI openTimeText, statusText;

    private Chest chest;
    private bool isClosing;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void ShowInfo(Chest chest)
    {
        if (panel.activeSelf) return;

        StopAllCoroutines();
        openButton.SetActive(true);

        this.chest = chest;
        var asset = chest.chestAsset;

        isClosing = false;
        panel.transform.DOScale(0.001f, 0);

        chestImage.sprite = asset.Image;
        nameText.text = asset.chestName;

        if (chest.IsOpening)
        {
            openButton.SetActive(false);
            statusText.text = "Opening";
            StartCoroutine(LiveTimer());
        }
        else
        {
            statusText.text = "Closed";
            openTimeText.text = TimeSpan.FromSeconds(asset.SecondsToOpen).ToString(@"hh\:mm\:ss");
        }

        panel.SetActive(true);
        panel.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
    }

    private IEnumerator LiveTimer()
    {
        while (chest.SecondsLeft > 0)
        {
            openTimeText.text = TimeSpan.FromSeconds(chest.SecondsLeft).ToString(@"hh\:mm\:ss");
            yield return new WaitForSeconds(1);
        }
        Close();
    }

    public void Open()
    {
        chest.StartOpening();
        Close();
    }

    public async void Close()
    {
        if (isClosing) return;

        isClosing = true;
        StopAllCoroutines();

        await panel.transform.DOScale(0.001f, 0.25f).SetEase(Ease.Flash).AsyncWaitForCompletion();
        panel.SetActive(false);
    }
}
