using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProductScannButtons : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    public void SaveProduct()
    {
        if (!string.IsNullOrWhiteSpace(nameInput.text))
        {
            ProductData.Instance.CurrentProduct = new Product
            {
                Name = nameInput.text,
                IsOpen = false,
                HasExpirationDate = false,
                DateOfStorage = SerializableDate.Today()
            };
            ProductData.Instance.SaveCurrentProduct = true;
            SceneManager.LoadScene(2);
        }
    }

    public void ScanDate()
    {
        if (!string.IsNullOrWhiteSpace(nameInput.text))
        {
            ProductData.Instance.CurrentProduct = new Product
            {
                Name = nameInput.text,
                IsOpen = false,
                HasExpirationDate = true,
                DateOfStorage = SerializableDate.Today()
            };
            SceneManager.LoadScene(1);
        }
    }
}
