using System.Collections.Generic;
using System;
using UnityEngine;


public class IdleDialogue : MonoBehaviour
{//TODO: Impelement function to check if previous item and/or sentence has been used before;
    public IdleSentences[] idleSentences = new IdleSentences[6];
    public QuestInquisitionSentences otherSentences = new QuestInquisitionSentences();
    internal List<int> questItemIDList = new List<int>();
    private int randomSentenceID = -1; //no need to be -1
    private int randomItemID = -1;
    private int prevQuestItemsCount = 0;
    private int currentQuestitemsCount = 0;
    

    internal string GetRandomIdleSentence()
    {
        int randomQuestItemID = GetRandomQuestItem();
        string randomIdleSentence = string.Empty;

        if (randomQuestItemID != -1)
        {
            System.Random r = new System.Random();
            IdleSentences idleSentenceClass = new IdleSentences();

            idleSentenceClass = idleSentences[randomQuestItemID];
           // Debug.Log($"<color=red>idleSentenceClass item</color>: {idleSentenceClass.itemName}");
            int totalSentences = idleSentenceClass.randomSentences.Length;

            if (totalSentences > 1)
            {
                randomSentenceID = r.Next(0, totalSentences);
              //  Debug.Log($"totalSentences: {totalSentences}, randomSentenceID: {randomSentenceID}");
            }
            else if (totalSentences == 1)
            {
                randomSentenceID = 0;
              //  Debug.Log($"totalSentences (should be 1): {totalSentences}, randomSentenceID (should be 0): {randomSentenceID}");
            }
            
            randomIdleSentence = idleSentenceClass.randomSentences[randomSentenceID];

            return randomIdleSentence;
        }

        Debug.Log("ItemIDList has zero elements, couldn't get a sentence.");
        return string.Empty;
    }

    private int GetRandomQuestItem()
    {
        if (questItemIDList.Count > 0)
        {
            System.Random r = new System.Random();
            int temporaryItemID = -1;

           // Debug.Log($"Number of Idle Quest Items: {questItemIDList.Count}");

            do
            {
                temporaryItemID = r.Next(0, questItemIDList.Count );
            }
            while (temporaryItemID == randomItemID);

            randomItemID = temporaryItemID;

           // Debug.Log($"<color=yellow>Random quest position number</color>: {randomItemID}");
           // Debug.Log($"<color=green>final value</color> <color=yellow>Random quest item number</color>: {questItemIDList[randomItemID]}");
            return questItemIDList[randomItemID];
        }

        Debug.Log("ItemIDS list has 0 elements");
        return -1;
    }

    internal void FillUpIDList()
    { //Inserts the IDs or position of quest Items in array
        int counter = 0;
        foreach (IdleSentences item in idleSentences)
        {
            if (item.isQuestItem)
            {
                questItemIDList.Add(counter);
              //  Debug.Log($"<color=red>position of item</color> in idleSentences: {counter}");
            }
                
            counter++;
        }
        prevQuestItemsCount = currentQuestitemsCount;
       // Debug.Log($" prevQuestItemsCount: { prevQuestItemsCount}");
    }

    internal void SetQuestItem(string productName, bool activate)
    {
        //Debug.Log($" prevQuestItemsCount (from SetQuestItem): { prevQuestItemsCount}");

        foreach (IdleSentences item in idleSentences)
        {
            if (item.itemName.Equals(productName, StringComparison.OrdinalIgnoreCase))
            {
                item.isQuestItem = activate;
               // Debug.Log($"{item.itemName} is idle dialogue quest item");
                if (activate)
                {
                    currentQuestitemsCount++;
                }
                else
                {
                    currentQuestitemsCount--;
                }
                //Debug.Log($"currenQuestItemsCount (from SetQuestItem): {currentQuestitemsCount}");
                return;
            }
        }
    }
}

[Serializable]
public class IdleSentences
{
    public string itemName;
    internal bool isQuestItem;

    [TextArea(1, 5)]
    public string[] randomSentences = new string[2];
}

[Serializable]
public class QuestInquisitionSentences
{
    [TextArea(1, 5)]
    public string HasPlayerCompletedQuest = "Do you have my things?";

    [TextArea(1, 5)]
    public string ThankPlayerForItems = "Thank you for these! It must have been a hassle.";

    [TextArea(1, 5)]
    public string WhyDidYouLie = "Seems like you don't have them actually... Come back when you have them.";

    [TextArea(1, 5)]
    public string WhyYouAlwaysLying = "Why are you always lying? You dont have the items, it's easier to tell the truth, y'know?";

    [TextArea(1, 5)]
    public string AlrightGoodLuck = "Okay then, good luck on your journey!";
}