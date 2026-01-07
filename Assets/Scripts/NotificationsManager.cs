using UnityEngine;
using Unity.Notifications.Android;
using System;

public class NotificationsManager : MonoBehaviour
{
    void Start()
    {
        //var channel = new AndroidNotificationChannel
        //{
        //    Id = "default_channel",
        //    Name = "Powiadomienia",
        //    Importance = Importance.High,
        //    Description = "Powiadomienia aplikacji",
        //};
        //AndroidNotificationCenter.RegisterNotificationChannel(channel);

        //var notification = new AndroidNotification
        //{
        //    Title = "Hej!",
        //    Text = "To dzia³a w tle :)",
        //    //RepeatInterval = TimeSpan.FromHours(6)
        //};

        //AndroidNotificationCenter.SendNotification(notification, "default_channel");
    }
}
