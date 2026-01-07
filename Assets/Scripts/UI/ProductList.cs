using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class ProductList : MonoBehaviour
{
    [System.Serializable]
    private struct ItemInfoEditable
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private GameObject buttonTab;

        public TMP_Text Text { get { return text; } }
        public GameObject ButtonTab { get { return buttonTab; } }
    }
    public static ProductList Instance;
    [SerializeField] private Transform itemList;
    [SerializeField] private GameObject mainTab, editTab;
    [SerializeField] private ProductItem currentProduct, listItemPrefab;
    [SerializeField] private List<ProductItem> products;
    [SerializeField] private TMP_Text itemNameText, storageDateText, expirationDate, changeNameMessage, scanButtonText, changeExpDateMessage;
    [SerializeField] private ItemInfoEditable openingDate;
    [SerializeField] private TMP_InputField newNameField, dayField, monthField, yearField;
    private delegate void OperationToConfirm();
    private OperationToConfirm operationToConfirm;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        List<Product> _products = SavedData.GetProductsList();
        if(ProductData.Instance.SaveCurrentProduct)
        {
            _products.Add(ProductData.Instance.CurrentProduct);
            ProductData.Instance.SaveCurrentProduct = false;
        }
        products = new List<ProductItem>();
        foreach (var product in _products)
        {
            ProductItem item = Instantiate(listItemPrefab);
            item.transform.parent = itemList;
            item.ProductName = product.Name;
            item.IsOpen = product.IsOpen;
            item.HasExpirationDate = product.HasExpirationDate;
            item.DateOfOppenning = product.DateOfOppenning;
            item.ExpirationDate = product.ExpirationDate;
            products.Add(item);
        }

        SaveProducts(); // debug
    }
    public void openMainTab()
    {
        mainTab.SetActive(true);
        editTab.SetActive(false);
    }

    public void openEditTab(ProductItem productItem)
    {
        currentProduct = productItem;
        ProductData.Instance.CurrentProduct = new Product
        {
            Name = currentProduct.ProductName,
            DateOfStorage = currentProduct.DateOfStorage,
            DateOfOppenning = currentProduct.DateOfOppenning,
            ExpirationDate = currentProduct.ExpirationDate,
            HasExpirationDate = currentProduct.HasExpirationDate,
            IsOpen = currentProduct.IsOpen
        };
        UpdateEditTab();
        editTab.SetActive(true);
        mainTab.SetActive(false);
    }

    public void UpdateEditTab()
    {
        itemNameText.text = currentProduct.ProductName;
        string storageDateString = currentProduct.DateOfStorage.day.ToString() + '.' + currentProduct.DateOfStorage.month.ToString() + '.' + currentProduct.DateOfStorage.year.ToString();
        storageDateText.text = storageDateString;
        if(currentProduct.HasExpirationDate)
        {
            string expirationDateString = currentProduct.ExpirationDate.day.ToString() + '.' + currentProduct.ExpirationDate.month.ToString() + '.' + currentProduct.ExpirationDate.year.ToString();
            expirationDate.text = expirationDateString;
            scanButtonText.text = "Scan again";
        }
        else
        {
            expirationDate.text = "-";
            scanButtonText.text = "Scan";
        }
        if (currentProduct.IsOpen)
        {
            string OpeningDateString = currentProduct.DateOfOppenning.day.ToString() + '.' + currentProduct.DateOfOppenning.month.ToString() + '.' + currentProduct.DateOfOppenning.year.ToString();
            openingDate.Text.text = OpeningDateString;
            openingDate.Text.gameObject.SetActive(true);
            openingDate.ButtonTab.gameObject.SetActive(false);
        }
        else
        {
            openingDate.Text.gameObject.SetActive(false);
            openingDate.ButtonTab.gameObject.SetActive(true);
        }
    }

    public void OpenButtonAction()
    {
        operationToConfirm = () =>
        {
            OpenProduct();
        };
    }

    public void OpenProduct()
    {
        currentProduct.IsOpen = true;
        currentProduct.DateOfOppenning = SerializableDate.Today();
        UpdateEditTab();
        SaveProducts();
    }

    public void DeleteButtonAction()
    {
        operationToConfirm = () =>
        {
            DeleteProduct();
        };
    }
    private void DeleteProduct()
    {
        products.Remove(currentProduct);
        Destroy(currentProduct.gameObject);
        openMainTab();
        SaveProducts();
    }

    public void OpenNewNameTab()
    {
        changeNameMessage.text = " ";
    }

    public void ChangeNameButtonAction()
    {
        if (newNameField.text != string.Empty)
        {
            currentProduct.ProductName = newNameField.text;
            UpdateEditTab();
            changeNameMessage.text = "New name saved";
        }
        else
        {
            changeNameMessage.text = "Name can't be empty";
        }
        SaveProducts();
    }

    public void ScanButton()
    {
        ProductData.Instance.CurrentProduct = new Product
        {
            Name = currentProduct.ProductName,
            DateOfStorage = currentProduct.DateOfStorage,
            DateOfOppenning = currentProduct.DateOfOppenning,
            ExpirationDate = currentProduct.ExpirationDate,
            HasExpirationDate = currentProduct.HasExpirationDate,
            IsOpen = currentProduct.IsOpen
        };
        SceneManager.LoadScene(3);
    }

    public void OpenEnterExpDateTab()
    {
        dayField.text = String.Empty;
        monthField.text = String.Empty;
        yearField.text = String.Empty;
        changeExpDateMessage.text = " ";
    }

    public void ChangeExpDateButtonAction()
    {
        if (!string.IsNullOrEmpty(dayField.text) &&
        !string.IsNullOrEmpty(monthField.text) &&
        !string.IsNullOrEmpty(yearField.text))
        {
            string dateString = $"{dayField.text}.{monthField.text}.{yearField.text}";
            DateTime newDate;
            if (DateTime.TryParseExact(dateString, "d.M.yyyy",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out newDate))
            {
                currentProduct.HasExpirationDate = true;
                currentProduct.ExpirationDate = new SerializableDate(newDate.Day, newDate.Month, newDate.Year, 0);
                UpdateEditTab();
                changeExpDateMessage.text = "Expiration date changed";
            }
            else
            {
                changeExpDateMessage.text = "Wrong date format";
            }
        }
        else
        {
            changeExpDateMessage.text = "All date input fields must be set";
        }
        SaveProducts();
    }


    public void ConfirmOperation()
    {
        operationToConfirm();
    }

    private void SaveProducts()
    {
        List<Product> productsData = new List<Product>();
        foreach(ProductItem product in products)
        {
            Product newProductData = new Product(product);
            productsData.Add(newProductData);
        }
        SavedData.SaveProductsList(productsData);
    }
}
