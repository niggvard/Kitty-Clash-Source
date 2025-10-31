using System;
using UnityEngine;

public class FreeChestPanel : PanelUI<bool>
{
    [Header("Chests settings")]
    [SerializeField] private ChestSettings chestAsset;

    protected override void Start()
    {
        base.Start();

        var save = PlayerPrefs.GetString("FreeChestDate", "");
        if (DateTime.TryParse(save, out DateTime lastDate))
        {
            if ((DateTime.Now.Date - lastDate.Date).TotalDays >= 1 && ArenasHolder.Instance.CurrentArenaIndex > 0)
                ShowPanel(true);
            else
                Destroy(gameObject);
        }
        else if (ArenasHolder.Instance.CurrentArenaIndex > 0)
            ShowPanel(true);
        else
            Destroy(gameObject);
    }

    public void OpenChest()
    {
        ClosePanel();
        Chest.ForceOpen(chestAsset);
        PlayerPrefs.SetString("FreeChestDate", DateTime.Now.ToString());

        SendNotifications();
    }

    public void SendNotifications()
    {
        NotificationsManager.SetupNotification(new Notification(
            "FreeChest" + DateTime.Now.ToString(),
            "FREE CHEST",
            "Daily chest is ready to open",
            "dailyChest",
            "dailyChestOpen",
            DateTime.Now.AddDays(1)
        ));
        NotificationsManager.RemoveNotifications("3DaysReminder");
        NotificationsManager.SetupNotification(new Notification(
            "3DaysReminder" + DateTime.Now.ToString(),
            "Meow!",
            "Where are you? Free chest is waiting for you!",
            "reminder",
            "3DaysReminder",
            DateTime.Now.AddDays(3)
        ));
    }
}
