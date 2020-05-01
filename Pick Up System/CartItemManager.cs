using System;
using UnityEngine;

public class CartItemManager: MonoBehaviour
{
    private Products productScript;
    private object [] productStorage;
    private bool storageFilledUp = false;

    #region Init Functions

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

    #endregion

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

    #region CheckProduct Functions

    internal void CheckProduct(string productName, bool check)
    {
        foreach (ProductInformation item in productStorage)
        {
            if (productName.Equals(item.productName, StringComparison.OrdinalIgnoreCase))
            {
                item.hasProduct = check;
               // Debug.Log($"Product was (un)checked: {productName}");
            }
        }
    }

    internal void CheckProduct( bool checkAll)
    {
        foreach (ProductInformation item in productStorage)
        {
            item.hasProduct = checkAll;
        }
    }
    #endregion

    #region IsProductChecked Functions

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

    internal bool IsAnyProductChecked()
    {
        bool anyProductIsPresent = false;

        foreach (ProductInformation item in productStorage)
        {
            if (item.hasProduct)
            {
                anyProductIsPresent = true;
                break;
            }
        }

        return anyProductIsPresent;
    }
    #endregion
}

class ProductInformation
{
     internal string productName = string.Empty;
     internal bool hasProduct = false;
}
