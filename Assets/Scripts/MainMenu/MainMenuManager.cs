using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Numerics;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance { get; private set; }

    [field: SerializeField] public ArenasHolder ArenasHolder { get; private set; }
    [field: SerializeField] public RaritiesSettings RaretiesSettings { get; private set; }
    [SerializeField] private TextMeshProUGUI trophiesText;
    [SerializeField] private Image menuLevelImage;
    [SerializeField] private Image fade;

    public void LoadScene(int cond)
    {
        fade.gameObject.SetActive(true);
        fade.DOKill();

        if (cond == 0)
        {
            fade.pixelsPerUnitMultiplier = 100;

            DOTween.To(() => fade.pixelsPerUnitMultiplier, x => fade.pixelsPerUnitMultiplier = x, 0.01f, 2).SetEase(Ease.InOutExpo);
            DOTween.To(() => fade.pixelsPerUnitMultiplier, x => fade.pixelsPerUnitMultiplier = x, 0.01f, 2).SetEase(Ease.InOutExpo).OnComplete(() => SceneManager.LoadScene("Game"));
        }
        else if (cond == 1)
        {
            fade.pixelsPerUnitMultiplier = 0.01f;

            DOTween.To(() => fade.pixelsPerUnitMultiplier, x => fade.pixelsPerUnitMultiplier = x, 0.01f, 0.5f).SetEase(Ease.InOutExpo);
            DOTween.To(() => fade.pixelsPerUnitMultiplier, x => fade.pixelsPerUnitMultiplier = x, 0.01f, 0.5f).SetEase(Ease.InOutExpo).OnComplete(() => SceneManager.LoadScene("Game"));
        }

        //fade.DOFade(0, 0);
        //fade.DOFade(1, 1).SetEase(Ease.Linear).OnComplete(() => SceneManager.LoadScene(1));
    }

    private void Start()
    {
        menuLevelImage.sprite = ArenasHolder.CurrentArenaInfo.GameplayBackground;
        trophiesText.text = ArenasHolder.CurrentTrophy.ToString();

        if (Application.platform == RuntimePlatform.IPhonePlayer)
            Application.targetFrameRate = 60;
        Application.runInBackground = true;
    }

    private void Awake()
    {
        Instance = this;
        ArenasHolder.CreateInstance();
        RaretiesSettings.CreateInstance();

        fade.gameObject.SetActive(true);
        fade.DOKill();
        fade.pixelsPerUnitMultiplier = 0.01f;

        DOTween.To(() => fade.pixelsPerUnitMultiplier, x => fade.pixelsPerUnitMultiplier = x, 100f, 3).SetEase(Ease.InOutExpo);
        DOTween.To(() => fade.pixelsPerUnitMultiplier, x => fade.pixelsPerUnitMultiplier = x, 100f, 3).SetEase(Ease.InOutExpo).OnComplete(() => fade.gameObject.SetActive(false));

        //fade.DOFade(1, 0);
        //fade.DOFade(0, 1).SetEase(Ease.Linear).OnComplete(() => fade.gameObject.SetActive(false));
    }
}
