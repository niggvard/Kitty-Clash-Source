using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinishMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject noChestSlotText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI trophyText;
    [SerializeField] private Image chestImage;
    [SerializeField] private Image loseImage;
    [SerializeField] private AudioSource finishSound;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private Button exitButton;

    private void OnEnable()
    {
        EventManager.GameFinished += OnGameFinish;
    }

    private void OnDisable()
    {
        EventManager.GameFinished -= OnGameFinish;
    }

    private async void OnGameFinish(GameFinishStatus status)
    {
        int trophyChange = 0;

        StartCoroutine(FreezeTimer());

        switch (status)
        {
            case GameFinishStatus.win:
                trophyChange = 10 + Random.Range(0, 2);
                statusText.text = "You win!";
                TrophyAnimationManager.hasWon = true;
                break;
            case GameFinishStatus.lose:
                trophyChange = -10;
                statusText.text = "You lost!";
                break;
            case GameFinishStatus.draw:
                trophyChange = 0;
                statusText.text = "Draw!";
                break;
        }

        if (status == GameFinishStatus.lose || status == GameFinishStatus.draw)
        {
            Destroy(noChestSlotText);
            Destroy(chestImage.gameObject);
        }
        else if(status == GameFinishStatus.win && !ChestManager.HaveEmptySlot)
        {
            Destroy(loseImage.gameObject);
            Destroy(chestImage.gameObject);
        }
        else
        {
            var chest = ArenasHolder.Instance.ArenaList[ArenasHolder.Instance.CurrentArenaIndex].GetChest();
            ChestManager.chestToGive = chest;
            chestImage.sprite = chest.Image;
            Destroy(loseImage.gameObject);
            Destroy(noChestSlotText);
        }

        trophyText.text = trophyChange.ToString();

        ArenasHolder.CurrentTrophy += trophyChange;

        await menu.transform.DOScale(0.001f, 2).SetUpdate(true).AsyncWaitForCompletion();
        finishSound.clip = audioClips[(int)status];
        finishSound.Play();
        
        menu.SetActive(true);
        menu.transform.DOScale(1f, 0.5f).SetEase(Ease.Flash).SetUpdate(true);
        Analytics.OnGameFinished(status == GameFinishStatus.win, ArenasHolder.CurrentTrophy, ArenasHolder.Instance.CurrentArenaIndex);
    }


    public void DisableButton()
    {
        exitButton.interactable = false;
    }

    private IEnumerator FreezeTimer()
    {
        yield return new WaitForSeconds(1);
        Time.timeScale = 0;
    }
}
