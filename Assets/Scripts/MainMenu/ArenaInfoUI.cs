using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArenaInfoUI : MonoBehaviour
{
    [SerializeField] private Image arenaImage;
    [SerializeField] private Transform cardsParent;
    [SerializeField] private TextMeshProUGUI nameText, requirementsText;
    [SerializeField] private MenuCard cardPrefab;

    public ArenaInfoUI Setup(Arena arena)
    {
        arenaImage.sprite = arena.arenaImage;
        nameText.text = arena.Name;
        requirementsText.text = requirementsText.text.Replace("XXX", arena.TrophyRequirement.ToString());

        foreach (var unit in arena.UnlockableUnits)
        {
            var card = Instantiate(cardPrefab, cardsParent).Setup(unit);
            Destroy(card.GetComponent<Button>());
        }

        return this;
    }
}
