using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ArenasListUI : MonoBehaviour
{
    [SerializeField] private Image currentArenaImage;
    [SerializeField] private ArenaInfoUI panelPrefab;
    [SerializeField] private Transform panelsParent;

    [Header("Scroll settings")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float scrollOffset;
    private RectTransform target;

    private void Start()
    {
        currentArenaImage.sprite = ArenasHolder.Instance.ArenaList[ArenasHolder.Instance.CurrentArenaIndex].arenaImage;

        var childs = panelsParent.Cast<Transform>().ToArray();
        foreach (var child in childs)
            Destroy(child);

        var currentArena = ArenasHolder.Instance.ArenaList[ArenasHolder.Instance.CurrentArenaIndex];
        foreach (var arena in ArenasHolder.Instance.ArenaList)
        {
            var panel = Instantiate(panelPrefab, panelsParent).Setup(arena);
            if (arena.Name == currentArena.Name)
                target = panel.GetComponent<RectTransform>();
        }

        GetComponent<Button>().onClick.AddListener(() => ScrollToTarget(target));
    }

    private void ScrollToTarget(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();
        Vector2 viewPortLocalPosition = scrollRect.viewport.localPosition;
        Vector2 targetLocalPosition = target.localPosition;

        Vector2 newTargetLocalPosition = new Vector2(
            0,
            -(viewPortLocalPosition.y + targetLocalPosition.y) +
            (scrollRect.viewport.rect.height / 2) -
            (target.rect.height / 2) + scrollOffset
        );

        scrollRect.content.localPosition = newTargetLocalPosition;
    }
}
