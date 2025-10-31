using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnselectedMenuPanelButton : MonoBehaviour
{
    private static float originalSize;
    private Button button;
    private Toggle toggle;

    private void Awake()
    {
        if (GetComponent<Button>() != null)
        {
            button = GetComponent<Button>();
            originalSize = button.transform.localScale.x;
            
            button.onClick.AddListener(() => Select());
        }

        if (GetComponent<Toggle>() != null)
        {
            toggle = GetComponent<Toggle>();
            originalSize = toggle.transform.localScale.x;
            
            toggle.onValueChanged.AddListener((isOn) => Select());
        }
    }

    public void Select()
    {
        transform.transform.DOKill();
        transform.transform.DOScale(originalSize, 0);

        transform.DOScale(originalSize * 0.8f, 0.05f).OnComplete(delegate {
            transform.DOScale(originalSize, 0.1f).SetEase(Ease.Flash);
        });
    }
}
