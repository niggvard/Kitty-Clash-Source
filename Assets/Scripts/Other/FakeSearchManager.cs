using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FakeSearchManager : MonoBehaviour
{
    [Header("Player Info Panel")]
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerTrophiesText;
    [SerializeField] private Image playerAvatarImage;

    [Header("Enemy Info Panel")]
    [SerializeField] private GameObject enemyInfoPanel;
    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private TextMeshProUGUI enemyTrophiesText;
    [SerializeField] private Image enemyAvatarImage;

    [Header("Avatars")]
    [SerializeField] private Sprite[] avatarSprites;

    [Header("Search Settings")]
    [SerializeField] private GameObject searchPanel;
    [SerializeField] private Transform animationPanel;
    [SerializeField] private int trophyRange;
    [SerializeField] private Button playButton;

    [Header("Search Animation")]
    [SerializeField] private GameObject dotHolder;
    [SerializeField] private Transform animationDot1, animationDot2, animationDot3;
    [SerializeField] private GameObject stopButton;

    [Header("Puzzle")]
    [SerializeField] private PuzzleMap[] puzzlePrefabs;
    [SerializeField] private Image puzzleImage;

    [Header("Sounds")]
    [SerializeField] private AudioSource enemyReadySound;
    [SerializeField] private AudioSource spinSound;

    private string playerName;
    private int playerTrophies;
    private int playerAvatarIndex;

    private void Start()
    {
        playerName = SavesManager.Nickname;
        playerTrophies = ArenasHolder.CurrentTrophy;
        playerAvatarIndex = SavesManager.AvatarId;

        UpdateInfo();

        playButton.onClick.AddListener(StartSearch);
    }

    private void UpdateInfo()
    {
        playerNameText.text = playerName;
        playerTrophiesText.text = playerTrophies.ToString();
        playerAvatarImage.sprite = avatarSprites[playerAvatarIndex];
    }

    public void StartSearch()
    {
        stopButton.SetActive(true);
        searchPanel.SetActive(true);

        animationPanel.DOScale(0.001f, 0);
        animationPanel.DOScale(1, 0.25f).SetEase(Ease.Flash).OnComplete(delegate {
            dotHolder.SetActive(true);
            StartCoroutine(DotsAnimation());
        });

        enemyInfoPanel.SetActive(false);
        StartCoroutine(SearchEnemy());
    }

    private IEnumerator SearchEnemy()
    {
        yield return new WaitForSeconds(Random.Range(3, 7));
        VibrationManager.Vibrate(0.9f);
        stopButton.SetActive(false);
        dotHolder.SetActive(false);
        enemyReadySound.Play();

        string enemyName = NameProvider.GetRandomName();
        int enemyTrophies = Random.Range(playerTrophies - trophyRange, playerTrophies + trophyRange);

        if (enemyTrophies < ArenasHolder.CurrentArenaInfo.TrophyRequirement)
            enemyTrophies = ArenasHolder.CurrentArenaInfo.TrophyRequirement;

        Sprite enemyAvatar = avatarSprites[Random.Range(0, avatarSprites.Length)];

        UpdateEnemyInfo(enemyName, enemyTrophies, enemyAvatar);

        enemyInfoPanel.SetActive(true);
        enemyInfoPanel.transform.DOScale(0.001f, 0);
        enemyInfoPanel.transform.DOScale(1f, 0.25f).SetEase(Ease.Flash);

        LevelLoader.puzzlePrefab = Randomizer.GetRandomFromList(puzzlePrefabs);
        puzzleImage.gameObject.SetActive(true);
        spinSound.Play();
        for (int i = 0; i < 20; i++)
        {
            puzzleImage.sprite = null;
            puzzleImage.sprite = Randomizer.GetRandomFromList(puzzlePrefabs).puzzleImage;
            yield return new WaitForSeconds(0.1f);
        }
        spinSound.Stop();
        VibrationManager.Vibrate(0.2f);
        puzzleImage.sprite = LevelLoader.puzzlePrefab.puzzleImage;
        puzzleImage.transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(1);

        MainMenuManager.Instance.LoadScene(0);
    }

    private IEnumerator DotsAnimation()
    {
        while (dotHolder.activeInHierarchy)
        {
            yield return animationDot1.DOScale(1.2f, 0.25f).SetEase(Ease.Linear).WaitForCompletion();
            animationDot1.DOScale(1f, 0.25f).SetEase(Ease.Linear);

            yield return animationDot2.DOScale(1.2f, 0.25f).SetEase(Ease.Linear).WaitForCompletion();
            animationDot2.DOScale(1f, 0.25f).SetEase(Ease.Linear);

            yield return animationDot3.DOScale(1.2f, 0.25f).SetEase(Ease.Linear).WaitForCompletion();
            animationDot3.DOScale(1f, 0.25f).SetEase(Ease.Linear);
        }
    }

    private void UpdateEnemyInfo(string enemyName, int enemyTrophies, Sprite enemyAvatar)
    {
        enemyNameText.text = enemyName;
        enemyTrophiesText.text = enemyTrophies.ToString();
        enemyAvatarImage.sprite = enemyAvatar;
    }

    public void Cancel()
    {
        StopAllCoroutines();
        dotHolder.SetActive(false);
        animationPanel.DOScale(0.001f, 0.25f).OnComplete(() => searchPanel.SetActive(false));
    }
}
