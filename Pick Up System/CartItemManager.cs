using System;
using UnityEngine;

public class CartItemManager: MonoBehaviour
{
    private Products productScript;
    private object [] productStorage;
    private bool storageFilledUp = false;

    private void Awake()
    {
        productScript = FindObjectOfType<Products>();
    }

    private void Start()
    {
        Array.Resize(ref productStorage, productScript.AmountOfProducts);
    }

    private void Update()
    {
        if (productScript.products[0] != null && !storageFilledUp)
            FillUpProductStorage();
    }

    private void FillUpProductStorage()
    {
        int counter = 0;

         foreach (GameObject item in productScript.products)
         {
            ProductInformation productInfo = new ProductInformation();
            productInfo.productName = item.name;
            productInfo.hasProduct = false;
            productStorage[counter] = productInfo;
             counter++;
         }
        storageFilledUp = true;
        //Debug.Log("Product Storage filled up");
    }

    internal void CheckProduct(string productName)
    {
        foreach (ProductInformation item in productStorage)
        {
            if (productName.Equals(item.productName, StringComparison.OrdinalIgnoreCase))
            {
                item.hasProduct = true;
                //Debug.Log("Product was checked: " + productName);
            }
        }
    }

    internal bool IsProductChecked(string productName)
    {
        bool productIsHere = false;

        foreach (ProductInformation item in productStorage)
        {
            if (productName.Equals(item.productName, StringComparison.OrdinalIgnoreCase))
            {
                productIsHere = item.hasProduct;
            }
        }
        return productIsHere;
    }
}

 class ProductInformation
{
     public string productName = String.Empty;
     internal bool hasProduct = false;
}
