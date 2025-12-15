using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProductScannButtons : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;

    private void Start()
    {
        nameInput.onValidateInput = (string text, int charIndex, char addedChar) =>
        {
            return UITools.ValidateCharExclusive("\\/:*?\"<>|", addedChar);
        };
    }
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
            SceneManager.LoadScene(3);
        }
    }
}
