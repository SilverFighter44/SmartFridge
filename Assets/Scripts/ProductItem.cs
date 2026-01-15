using System;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct SerializableDate
{

    public int day, month, year, hour;

    public SerializableDate(DateTime date)
    {
        this.day = date.Day;
        this.month = date.Month;
        this.year = date.Year;
        this.hour = date.Hour;
    }
    public SerializableDate(int day, int month, int year, int hour)
    {
        this.day = day;
        this.month = month;
        this.year = year;
        this.hour = hour;
    }
    public static int CompareDays(SerializableDate d1, SerializableDate d2)
    {
        DateTime dt1 = new DateTime(d1.year, d1.month, d1.day, d1.hour, 0, 0), dt2 = new DateTime(d2.year, d2.month, d2.day, d2.hour, 0, 0);
        TimeSpan timeSpan = dt1 - dt2;
        return timeSpan.Days;
    }

    public static SerializableDate Today()
    {
        DateTime now = DateTime.Now;
        return new SerializableDate(now.Day, now.Month, now.Year, now.Hour);
    }
}
public class ProductItem : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText, stateText;
    [SerializeField] private string productName = "name";
    [SerializeField] private SerializableDate expirationDate, dateOfStorage = SerializableDate.Today(), dateOfOppenning;
    [SerializeField] private bool hasExpirationDate, isOpen;

    public string ProductName
    {
        get { return productName; }
        set { productName = value; }
    }

    public SerializableDate DateOfStorage
    {
        get { return dateOfStorage; }
        set { dateOfStorage = value; }
    }
    public SerializableDate DateOfOppenning
    {
        get { return dateOfOppenning; }
        set { dateOfOppenning = value; }
    }
    public SerializableDate ExpirationDate
    {
        get { return expirationDate; }
        set { expirationDate = value; }
    }

    public bool HasExpirationDate
    {
        get { return hasExpirationDate; }
        set { hasExpirationDate = value; }
    }

    public bool IsOpen
    {
        get { return isOpen; }
        set { isOpen = value; } 
    }

    private void Start()
    {
        UpdateItemInfo();
    }

    public void UpdateItemInfo()
    {
        nameText.text = productName;
        SerializableDate currentDate = SerializableDate.Today();
        if (!isOpen) // Product closed
        {
            if (hasExpirationDate) // Product has expiration date
            {
                int daysAfterExpiration = SerializableDate.CompareDays(currentDate, expirationDate);
                if (daysAfterExpiration > 0) // Product expired
                {
                    stateText.text = daysAfterExpiration + " " + ((daysAfterExpiration == 1) ? "day" : "days") + " after expitation date";
                    stateText.color = Color.red;
                }
                else // Product not expired
                {
                    int dayUntilExpiration = Math.Abs(daysAfterExpiration);
                    stateText.text = dayUntilExpiration + " " + ((dayUntilExpiration == 1) ? "day" : "days") + " until expitation date";
                    stateText.color = Color.green; // to do later: change color to yellow when expiration is close =====================================================
                }
            }
            else // Product doesn't have expiration date
            {
                int daysInFridge = SerializableDate.CompareDays(currentDate, dateOfStorage);
                stateText.text = daysInFridge + " " + ((daysInFridge == 1) ? "day" : "days") + " in fridge";
                stateText.color = Color.orange; // to do later: change color depending of product and days in fridge ====================================================
            }
        }
        else
        {
            int daysAfterExpiration = 0, daysOpen = SerializableDate.CompareDays(currentDate, dateOfOppenning);
            if (hasExpirationDate)
            {
                daysAfterExpiration = SerializableDate.CompareDays(currentDate, expirationDate);
            }
            if (daysAfterExpiration > 0) // Product open and expired
            {
                stateText.text = daysAfterExpiration + " " + ((daysAfterExpiration == 1) ? "day" : "days") + " expired and " + daysOpen + " days open";
                stateText.color = Color.red;
            }
            else // Product open
            {
                stateText.text = daysOpen + " " + ((daysOpen == 1) ? "day" : "days") + " open";
                stateText.color = Color.orange; // to do later: change color depending of product and days in fridge =====================================================
            }
        }
    }

    public void OpenEditTab()
    {
        ProductList.Instance.openEditTab(this);
    }


}
