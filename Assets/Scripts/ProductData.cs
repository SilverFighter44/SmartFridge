using UnityEngine;

public class ProductData : MonoBehaviour
{
    public static ProductData Instance;

    public Product CurrentProduct;
    public bool SaveCurrentProduct = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
