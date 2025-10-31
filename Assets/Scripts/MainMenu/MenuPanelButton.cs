using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanelButton : MonoBehaviour
{
    private static MenuPanelButton selectedButton;
    private static Color originalColor;
    private static float originalSize;

    [SerializeField] private bool startPanel;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        originalColor = button.image.color;
        originalSize = button.transform.localScale.x;

        button.onClick.AddListener(() => Select());

        if (startPanel)
            Select();
    }

    public void Select()
    {
        ClearSelectedButton();

        transform.SetSiblingIndex(transform.parent.childCount);

        transform.DOScale(1.7f, 0.05f).OnComplete(delegate {
            transform.DOScale(2.3f, 0.1f).SetEase(Ease.Flash);
        });

        button.image.color = Color.yellow;
        selectedButton = this;
    }

    private void ClearSelectedButton()
    {
        if (selectedButton != null)
        {
            selectedButton.button.image.color = originalColor;
            selectedButton.transform.DOKill();
            selectedButton.transform.DOScale(originalSize, 0);
        }
    }
}
