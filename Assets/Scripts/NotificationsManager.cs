using UnityEngine;
using Unity.Notifications.Android;
using System;

public class NotificationsManager : MonoBehaviour
{
    public static NotificationsManager Instance;

    private const string CHANNEL_ID = "expiry_channel";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            RegisterChannel();
        }
        else Destroy(gameObject);
    }

    private void RegisterChannel()
    {
        var channel = new AndroidNotificationChannel
        {
            Id = CHANNEL_ID,
            Name = "Terminy wa¿noœci",
            Importance = Importance.High,
            Description = "Powiadomienia o koñcz¹cych siê produktach"
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    public void ScheduleProductNotifications(ProductItem product)
    {
        if (!product.HasExpirationDate) return;

        DateTime expDate = new DateTime(
            product.ExpirationDate.year,
            product.ExpirationDate.month,
            product.ExpirationDate.day,
            9, 0, 0); // godz. 9:00

        // 2 dni przed
        DateTime warningDate = expDate.AddDays(-2);

        if (warningDate > DateTime.Now)
        {
            var warningNotif = new AndroidNotification
            {
                Title = "Produkt wkrótce siê przeterminuje!",
                Text = $"{product.ProductName} – zosta³y 2 dni",
                FireTime = warningDate
            };
            int id1 = AndroidNotificationCenter.SendNotification(warningNotif, CHANNEL_ID);
            product.NotificationIdWarning = id1;
        }

        // w dniu przeterminowania
        if (expDate > DateTime.Now)
        {
            var expNotif = new AndroidNotification
            {
                Title = "Produkt przeterminowany!",
                Text = $"{product.ProductName} straci³ wa¿noœæ",
                FireTime = expDate
            };
            int id2 = AndroidNotificationCenter.SendNotification(expNotif, CHANNEL_ID);
            product.NotificationIdExpired = id2;
        }
    }

    public void CancelProductNotifications(ProductItem product)
    {
        AndroidNotificationCenter.CancelNotification(product.NotificationIdWarning);
        AndroidNotificationCenter.CancelNotification(product.NotificationIdExpired);
    }
}
