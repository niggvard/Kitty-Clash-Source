using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgradeMoneyMenu : MonoBehaviour
{
    private bool isClosing;

    private void Start()
    {
        isClosing = false;

        if (PlayerPrefs.GetInt("LastScene", 0) < ArenasHolder.Instance.CurrentArenaIndex)
        {
            PlayerPrefs.SetInt("LastScene", ArenasHolder.Instance.CurrentArenaIndex);
            TowerUpgradesManager.CurrentCurrency++;
            transform.DOScale(1, 0.5f).SetEase(Ease.OutBounce);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void Close()
    {
        if (isClosing) return;

        isClosing = true;
        await transform.DOScale(0.001f, 0.25f).SetEase(Ease.Flash).AsyncWaitForCompletion();
        Destroy(gameObject);
    }
}
