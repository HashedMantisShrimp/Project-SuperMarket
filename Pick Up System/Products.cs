using System;
using UnityEngine;

public class Products : MonoBehaviour
{
    [SerializeField]
    private GameObject goods = null; //The GameObject with all the products must be attched here
    internal int AmountOfProducts { get; set; }
    internal GameObject[] products;

    #region Init Functions

    void Awake()
    {
        AmountOfProducts = goods.transform.childCount;
    }

    void Start()
    {
        Array.Resize<GameObject>(ref products, AmountOfProducts);
        AssignProductsToArray();
    }

    #endregion

    internal GameObject GetProduct(string colliderName)
    {// returns the gameObject of the requested product

        string productName = GetUntilOrEmpty(colliderName);
        GameObject productObject = null;

        if (productName != string.Empty)
        {
            foreach (GameObject item in products)
            {
                //Debug.Log($"product names: {product.name}");
                if (productName.Equals(item.name, StringComparison.OrdinalIgnoreCase))
                {
                    productObject = item.gameObject;
                    break;
                }
            }
        }
        return productObject;
    }
    
    #region Private Functions

    private void AssignProductsToArray()
    {//fills up the products array
        Transform goodsTransfrom = goods.transform;
        int counter = 0;
        
        foreach (Transform child in goodsTransfrom) {
            products[counter] = child.gameObject;
            counter++;
        }
        // Debug.Log($"One of the children is: {products[2].name}");
    }

    private string GetUntilOrEmpty(string text, string stopAt = "-")//checks for where the '-' is and returns everything before it
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

            if (charLocation > 0)
            {
                // Debug.Log($"The found text was: {text.Substring(0, charLocation)}");
                return text.Substring(0, charLocation);
            }
        }
       // Debug.Log("Could not find the new text associated with productName");
        return string.Empty;
    }
    #endregion
}


