using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarSelectionManager : MonoBehaviour
{
    [SerializeField] private Image currentAvatar;
    [SerializeField] private Sprite[] avatarSprites;
    [SerializeField] private Button[] avatarButtons;

    private void Start()
    {
        for (int i = 0; i < avatarButtons.Length; i++)
        {
            int avatarID = i;
            Image buttonImage = avatarButtons[i].GetComponent<Image>();
            buttonImage.sprite = avatarSprites[i];

            avatarButtons[i].onClick.AddListener(() => SelectAvatar(avatarID));
        }

        int savedAvatarID = SavesManager.AvatarId;
        currentAvatar.sprite = avatarSprites[savedAvatarID];
    }

    public void SelectAvatar(int avatarID)
    {
        SavesManager.AvatarId = avatarID;
        currentAvatar.sprite = avatarSprites[avatarID];
    }
}
