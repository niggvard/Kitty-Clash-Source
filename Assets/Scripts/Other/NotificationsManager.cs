using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.iOS;

#if UNITY_IOS
using Unity.Notifications.iOS;
#endif
using System;

public class NotificationsManager
{
    public static void SetupNotification(Notification notification)
    {
#if UNITY_IOS
        var newNotification = new iOSNotification
        {
            Identifier = notification.identifier,
            Title = notification.title,
            Body = notification.bodyText,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = notification.categoryIdentifier,
            ThreadIdentifier = notification.threadIdentifier,
            Trigger = new iOSNotificationTimeIntervalTrigger
            {
                TimeInterval = notification.activationTime - DateTime.Now,
                Repeats = false
            }
        };
        iOSNotificationCenter.ScheduleNotification(newNotification);
        notification.Save();
#endif
    }

    public static void RemoveNotifications(string textInName)
    {
#if UNITY_IOS
        var scheduledNotifications = iOSNotificationCenter.GetScheduledNotifications();
        foreach (var notification in scheduledNotifications)
        {
            if (notification.Identifier.Contains(textInName))
            {
                iOSNotificationCenter.RemoveScheduledNotification(notification.Identifier);
            }
        }
#endif
    }
}

public class Notification
{
    public static List<Notification> notifications;

    public readonly string identifier, title, bodyText, categoryIdentifier, threadIdentifier;
    public readonly DateTime activationTime;

    public Notification(string identifier, string title, string bodyText, string categoryIdentifier, string threadIdentifier, DateTime activationTime)
    {
        this.identifier = identifier;
        this.title = title;
        this.bodyText = bodyText;
        this.categoryIdentifier = categoryIdentifier;
        this.threadIdentifier = threadIdentifier;
        this.activationTime = activationTime;
    }

    public void Save()
    {
        if (notifications == null)
            notifications = LoadNotifications();

        if (notifications == null)
            notifications = new();

        notifications.Add(this);
        string json = JsonUtility.ToJson(notifications);
        Debug.Log(json);
        PlayerPrefs.SetString("notifications", json);
    }

    public static List<Notification> LoadNotifications()
    {
        try
        {
            string json = PlayerPrefs.GetString("notifications", "");
            notifications = JsonUtility.FromJson<List<Notification>>(json);
        }
        catch
        {
            notifications = new();
        }

        return notifications;
    }
}