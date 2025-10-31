using System;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public static ChestManager Instance { get; private set; }
    public static ChestSettings chestToGive;

    public static bool HaveEmptySlot => GetChestCount() < 3;

    [SerializeField] private Transform chestsParent;
    [SerializeField] private Chest chestPrefab;

    private void Awake()
    {
        Instance = this;
        Chest.ChestStartOpening += UpdateChestSave;
        LoadChests();

        if (chestToGive != null)
        {
            GiveChest(chestToGive);
            chestToGive = null;
        }
    }

    private void OnDisable()
    {
        Chest.ChestStartOpening -= UpdateChestSave;
    }

    private void UpdateChestSave(Chest chest)
    {
        var saveData = chest.GetSaveData();
        PlayerPrefs.SetString("Chest" + chest.ChestNumber, saveData);
    }

    public void GiveRandomChest()
    {
        var chestAsset = ArenasHolder.Instance.ArenaList[ArenasHolder.Instance.CurrentArenaIndex].GetChest();
        var chestNumber = -1;
        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.GetString("Chest" + i, "") == "")
            {
                chestNumber = i;
                break;
            }
        }
        if (chestNumber == -1) return;

        var chest = Instantiate(chestPrefab, chestsParent).Setup(chestAsset, chestNumber);
        var saveData = chest.GetSaveData();
        PlayerPrefs.SetString("Chest" + chestNumber, saveData);
    }

    public void GiveChest(ChestSettings chestAsset)
    {
        var chestNumber = -1;
        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.GetString("Chest" + i, "") == "")
            {
                chestNumber = i;
                break;
            }
        }
        if (chestNumber == -1) return;

        var chest = Instantiate(chestPrefab, chestsParent).Setup(chestAsset, chestNumber);
        var saveData = chest.GetSaveData();
        PlayerPrefs.SetString("Chest" + chestNumber, saveData);
    }

    private void LoadChests()
    {
        for (int i = 0; i < 3; i++)
        {
            var data = PlayerPrefs.GetString("Chest" + i, "");
            if (data == "") continue;

            var chest = Instantiate(chestPrefab, chestsParent).LoadSaveData(data);
            chest.Setup(chest.chestAsset, i);
        }
    }

    public static int GetChestCount()
    {
        int count = 0;

        for (int i = 0; i < 3; i++)
        {
            var data = PlayerPrefs.GetString("Chest" + i, "");
            if (data != "")
                count++;
        }

        return count;
    }
}
