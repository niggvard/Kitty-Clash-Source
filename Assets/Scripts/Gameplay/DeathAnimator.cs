using DG.Tweening;
using UnityEngine;

public class DeathAnimator : MonoBehaviour
{
    public static DeathAnimator Instance { get; private set; }

    [SerializeField] private SpriteRenderer soulPrefab;
    [SerializeField] private Color playerColor, enemyColor;
    [SerializeField] private float animationDuration;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayDeathAnimation(UnitObject unit)
    {
        var soul = Instantiate(soulPrefab, unit.transform.position, Quaternion.identity);
        soul.sortingOrder = 50;
        soul.sprite = unit.Unit.soulSprite;
        soul.color = unit.isEnemy ? enemyColor : playerColor;

        soul.DOFade(0, animationDuration).SetEase(Ease.Linear).SetUpdate(true);
        soul.transform.DOMoveY(soul.transform.position.y + 3, animationDuration)
            .SetEase(Ease.Linear)
            .SetUpdate(true)
            .OnComplete(() => Destroy(soul.gameObject));
    }
}
