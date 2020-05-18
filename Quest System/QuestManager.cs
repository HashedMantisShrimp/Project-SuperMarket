using System.Collections.Generic;
using System;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Products productScript;
    private Dictionary<string, QuestProducts> questProductList = new Dictionary<string, QuestProducts>();
    private Dictionary<int, Quest> quests = new Dictionary<int, Quest>();
    internal int numberOfQuests = 0;
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

    internal void ActivateQuestProduct(bool isRequired, string productName) //make it true only?
    {
        string _productName = productName.ToLowerInvariant();

        if (questProductList.ContainsKey(_productName))
        {
            questProductList[_productName].requiresProduct = isRequired;
        }
    }

    internal void ResetQuestProduct(string productName)
    {
        string _productName = productName.ToLowerInvariant();

        if (questProductList.ContainsKey(_productName))
        {
            questProductList[_productName].requiresProduct = false;
            questProductList[_productName].playerHasItem = false;
        }
    }

    internal bool IsQuestproduct(string productName) 
    {
        string _productName = productName.ToLowerInvariant();

        if (questProductList.ContainsKey(_productName))
        {
            return questProductList[_productName].requiresProduct;
        }

        Debug.Log($"The product {productName} wasn't found. And this function will return false");
        return false;
    }

    internal void PlayerAcquiredProduct(bool hasBeenAcquired, string productName)
    {
        string _productName = productName.ToLowerInvariant();

        if (questProductList.ContainsKey(_productName))
        {
            //Debug.Log($"productPassedByCashier: {productName}, playerAcquiredProduct: {hasBeenAcquired}");
            questProductList[_productName].playerHasItem = hasBeenAcquired;
        }
    }

    internal bool HasPlayerAcquiredProduct(string productName)
    {
        string _productName = productName.ToLowerInvariant();

        if (questProductList.ContainsKey(_productName))
        {
            return questProductList[_productName].playerHasItem;
        }
        else
        {
            Debug.Log($"<color=red> questProductList doesnt contain:</color> {productName}");
        }

        Debug.Log($"The product {productName} <color=red>wasn't found</color>. And this function will return false");
        return false;
    }

    internal int SetNewQuest(string charName, List<string> productsNeeded)
    {
        bool questAlreadyExists = false;

        Quest newQuest = new Quest
        {
            npcName = charName,
            questItems = productsNeeded,
            questDelivered = false,
            questHasBeenCompleted = false
        };

        if (quests.Count > 0)
        {
            for (int i = 0; i < quests.Count; i++)
            {
                if (quests[i].npcName.Equals(newQuest.npcName))
                {
                    questAlreadyExists = true;
                    break;
                }
            }
        }
        

        if (!questAlreadyExists)
        {
            int nextID = IDGenerator();
            quests.Add(nextID, newQuest);
            
            numberOfQuests = quests.Count;
            return nextID;
        }

        Debug.Log("<color=red>Quest already exists</color> therefore it shall not be created, returning -1.");
        return -1;
    }

    internal void RemoveQuest(int ID)
    {
        if (quests[ID] != null)
        {
            quests.Remove(ID);
            Debug.Log($"Quest with ID: {ID} <color=green>removed successfully</color>");
            Debug.Log($"Checking if quests contains the recently deleted quest: {quests.ContainsKey(ID)}");
        }
    }

    internal bool IsQuestComplete(int questID)
    {
        List<string> questItemList = new List<string>();
        bool questComplete = false;
        int itemsAcquired = 0;

        if (quests[questID] != null)
        {
            questItemList = quests[questID].questItems;

            foreach (string item in questItemList)
            {
                string _item = item.ToLowerInvariant();

                if (HasPlayerAcquiredProduct(_item))
                    itemsAcquired++;
            }

            if (itemsAcquired == questItemList.Count)
            {
                Debug.Log("<color=LightBlue> Quest is complete </color>");
                questComplete = true;
            }
            else
            {
                Debug.Log("<color=DarkBlue> Quest is not complete </color>");
               // Debug.Log($"itemsAcquired: {itemsAcquired}, questItemListCount: {questItemList.Count}");
            }
        }

        return questComplete;
    }

    private void FillUpList()
    {
        if (productScript.products[0] != null)
        {
            foreach (GameObject product in productScript.products)
            {
                QuestProducts questProduct = new QuestProducts
                {
                    itemName = product.name.ToLowerInvariant(), 
                    item = product,
                    requiresProduct = false,
                    playerHasItem = false
                };
                questProductList.Add(questProduct.itemName, questProduct); //TODO: Check for whether the key (itemName) already exists
            }
            ListIsFull = true;
        }
        
    }

    private int IDGenerator()
    {
        int counter = 0;
        bool hasIDBeenUsed = true;

        do
        {
            if (quests.ContainsKey(counter))
            {
                counter++;
            }
            else
            {
                hasIDBeenUsed = false;
            }
        }
        while (hasIDBeenUsed);

        return counter;
    }
}

public class QuestProducts
{
    public string itemName;
    public GameObject item;
    public bool requiresProduct;
    public bool playerHasItem;
}

public class Quest
{
    public string npcName;
    internal List<string> questItems = new List<string>();
    internal bool questDelivered;
    internal bool questHasBeenCompleted;
}
