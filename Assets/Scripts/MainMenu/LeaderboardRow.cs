using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardRow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI trophiesText;
    [SerializeField] private TextMeshProUGUI fakePlayerNickName;
    [SerializeField] private Image avatarSprite;

    public LeaderboardRow Setup(PlayerData playerData)
    {
        trophiesText.text = playerData.Trophies.ToString();
        fakePlayerNickName.text = playerData.Name;
        avatarSprite.sprite = playerData.Avatar;

        return this;
    }

    private void Start()
    {
        if(fakePlayerNickName.text == "You")
        {
            transform.GetComponent<Image>().color = Color.yellow;
            fakePlayerNickName.color = Color.black;
        }
    }
}
