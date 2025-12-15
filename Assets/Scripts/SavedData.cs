using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public struct Product
{
    public string Name;
    public SerializableDate ExpirationDate, DateOfStorage, DateOfOppenning;
    public bool HasExpirationDate, IsOpen;
    public Product(ProductItem productItem)
    {
        ExpirationDate = productItem.ExpirationDate;
        DateOfStorage = productItem.DateOfStorage;
        DateOfOppenning = productItem.DateOfOppenning;
        IsOpen = productItem.IsOpen;
        Name = productItem.ProductName;
        HasExpirationDate = productItem.HasExpirationDate;
    }

}
public struct ProductListWrapper
{
    public List<Product> List;
    public ProductListWrapper(List<Product> list)
    {
        List = list;
    }
}
public static class SavedData
{
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="productsList"></param>
    public static void SaveProductsList(List<Product> productsList)
    {
        ProductListWrapper list = new ProductListWrapper(productsList);
        string profileJSON = JsonUtility.ToJson(list);
        string filePath = Path.Combine(Application.persistentDataPath, "products.json");
        Debug.Log(filePath);

        string directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        System.IO.File.WriteAllText(filePath, profileJSON);
        Debug.Log("Saving finished");
    }

    public static bool CheckIfSavedDataExist()
    {
        string[] filesInDirectory = Directory.GetFiles(Application.persistentDataPath);
        for (int i = 0; i < filesInDirectory.Length; i++)
        {
            filesInDirectory[i] = filesInDirectory[i].Remove(0, Application.persistentDataPath.Length + 1);
            if (filesInDirectory[i] == "products.json")
            {
                return true;
            }
        }
        return false;
    }

    public static List<Product> GetProductsList()
    {
        string filePath = Application.persistentDataPath + "/products.json";
        string directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        if (File.Exists(filePath))
        {
            string fileContent = System.IO.File.ReadAllText(filePath);
            return JsonUtility.FromJson<ProductListWrapper>(fileContent).List;
        }
        return new List<Product>();
    }
}
