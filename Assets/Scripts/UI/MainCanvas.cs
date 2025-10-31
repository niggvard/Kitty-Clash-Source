using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    public static MainCanvas Instance { get; private set; }

    [field: SerializeField] public RectTransform inputSliderPosition;
    [field: SerializeField] public Slider spawnCooldownSlider;

    [Header("Fade")]
    [SerializeField] private Image fadeImage;

    private void Awake()
    {
        Instance = this;

        fadeImage.gameObject.SetActive(true);
        fadeImage.pixelsPerUnitMultiplier = 0.01f;
        fadeImage.DOKill();

        DOTween.To(() => fadeImage.pixelsPerUnitMultiplier, x => fadeImage.pixelsPerUnitMultiplier = x, 100f, 3).SetEase(Ease.InOutExpo).SetUpdate(true);
        DOTween.To(() => fadeImage.pixelsPerUnitMultiplier, x => fadeImage.pixelsPerUnitMultiplier = x, 100f, 2).SetEase(Ease.InOutExpo).OnComplete(() => fadeImage.gameObject.SetActive(false)).SetUpdate(true);

        //fadeImage.DOFade(1, 0).SetUpdate(true);
        //fadeImage.DOFade(0, 0.4f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => fadeImage.gameObject.SetActive(false));
    }

    public void LoadScene(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOKill();
        fadeImage.pixelsPerUnitMultiplier = 100;

        DOTween.To(() => fadeImage.pixelsPerUnitMultiplier, x => fadeImage.pixelsPerUnitMultiplier = x, 0.01f, 2).SetEase(Ease.InOutExpo).SetUpdate(true);
        DOTween.To(() => fadeImage.pixelsPerUnitMultiplier, x => fadeImage.pixelsPerUnitMultiplier = x, 0.01f, 3).SetEase(Ease.InOutExpo).SetUpdate(true).OnComplete(delegate { Time.timeScale = 1; SceneManager.LoadScene(sceneName); });


        //fadeImage.DOFade(0, 0);
        //fadeImage.DOFade(1, 0.4f).SetEase(Ease.Linear).OnComplete(() => SceneManager.LoadScene(sceneName));
    }
}
