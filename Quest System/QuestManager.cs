using System.Collections.Generic;
using System;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Products productScript;
    private List<object> questProductList = new List<object>();
    private bool ListIsFull = false;

    private void Awake()
    {
        productScript = FindObjectOfType<Products>();
    }

    
    void Update()
    {
        if (!ListIsFull) //There is no need for this to be called all the time, find a way around this.
            FillUpList();
    }

    internal void ActivateQuestProduct(bool isRequired, string productName)
    {
        foreach (QuestProducts item in questProductList)
        {
            if (productName.Equals(item.itemName , StringComparison.OrdinalIgnoreCase))
            {
                item.requiresProduct = isRequired;
                Debug.Log($"Product {item.itemName} is quest item:{isRequired}");
            }
        }
    }

    internal void PlayerAcquiredProduct(bool hasBeenAcquired, string productName)
    {
        foreach (QuestProducts item in questProductList)
        {
            if (productName.Equals(item.itemName, StringComparison.OrdinalIgnoreCase))
            {
                item.playerHasItem = hasBeenAcquired;
                Debug.Log($"Product {item.itemName} has been acquired by user: {hasBeenAcquired}");
            }
        }
    }

    internal bool IsQuestproduct(string productName) 
    {
        foreach (QuestProducts item in questProductList)
        {
            if (productName.Equals(item.itemName, StringComparison.OrdinalIgnoreCase))
            {
                return item.requiresProduct;
            }
        }

        Debug.Log($"The product {productName} wasn't found. And this function will return false");
        return false;
    }

    internal bool HasPlayerAcquiredProduct(string productName)
    {
        foreach (QuestProducts item in questProductList)
        {
            if (productName.Equals(item.itemName, StringComparison.OrdinalIgnoreCase))
            {
                return item.playerHasItem;
            }
        }

        Debug.Log($"The product {productName} wasn't found. And this function will return false");
        return false;
    }

    private void FillUpList()
    {
        if (productScript.products[0] != null)
        {
            foreach (GameObject product in productScript.products)
            {
                QuestProducts questProduct = new QuestProducts
                {
                    itemName = product.name, 
                    item = product,
                    requiresProduct = false,
                    playerHasItem = false
                };
                questProductList.Add(questProduct);
            }
            ListIsFull = true;
        }
        
    }
}

public class QuestProducts
{
    public string itemName;
    public GameObject item;
    public bool requiresProduct;
    public bool playerHasItem;
}
