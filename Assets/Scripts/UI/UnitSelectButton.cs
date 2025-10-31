using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitSelectButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private Button button;
    [SerializeField] private Transform selectedBackground;

    private static UnitSelectButton currentButton;

    public void Setup(Unit unit)
    {
        cooldownText.text = unit.SpawnCooldown.ToString();
        button.image.sprite = unit.CardImage;
        button.onClick.AddListener(delegate
        {
            if(currentButton == this)
            {
                Dropper.Instance.Deselect();
                selectedBackground.position = new(0, -100000);
                currentButton = null;
                return;
            }
            Dropper.Instance.SelectUnit(unit);
            selectedBackground.position = transform.position;
            currentButton = this;
        });
    }
}
