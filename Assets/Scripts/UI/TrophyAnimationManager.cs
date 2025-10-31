using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrophyAnimationManager : MonoBehaviour
{
    [SerializeField] private Transform trophyImage, parent;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private TextMeshProUGUI trophyText;
    [SerializeField] private int trophies;

    private int currentTrophyCount;
    public static bool hasWon = false;

    private void Start()
    {
        currentTrophyCount = ArenasHolder.CurrentTrophy;
        UpdateTrophyText();

        if (hasWon)
        {
            StartCoroutine(AnimateTrophy(trophies));
            hasWon = false;
        }
    }

    private IEnumerator AnimateTrophy(int trophiesToAdd)
    {
        yield return new WaitForSeconds(2);
        for(int i  = 0; i < trophiesToAdd; i++)
        {
            Vector3 offset = new(Random.Range(-50, 50), Random.Range(-50, 50));
            Transform trophy = Instantiate(trophyImage, spawnPoint.position + offset, Quaternion.identity, parent);

            trophy.DOMove(trophyText.transform.position, 0.5f).SetEase(Ease.InOutQuad).OnComplete(() => Destroy(trophy.gameObject));


            yield return new WaitForSeconds(3 / trophiesToAdd);

            UpdateTrophyText();
        }
    }

    private void UpdateTrophyText()
    {
        trophyText.text = currentTrophyCount.ToString();
        ArenasHolder.CurrentTrophy = currentTrophyCount;
    }
}
