using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class Dialogue: MonoBehaviour
{
    public string charName;

    [TextArea(3,10)]
    public string[] sentences; //The sentences written on the editor for specific NPC
    internal string questSentence;
    private DialogueManager dialogueManager;
    private QuestManager questManager;
    public ProductNeeded [] characterProductList = new ProductNeeded[8];
    List<string> productsRequired;
    HashSet<int> previousItems = new HashSet<int>();
    private bool questDeliveredToPlayer = false;
    private bool questDeliveredByPlayer = false;
    private IdleDialogue idleDialogue;
    private int questID;
    private int playerLieCounter;

    #region Init Functions
    private void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        questManager = FindObjectOfType<QuestManager>();
        AssignIdleDialogue();
    }

    private void Start()
    {
        productsRequired = AssignProductsRequired();
        questID = questManager.SetNewQuest(charName, productsRequired);
        Debug.Log($"NPC name: <color=blue>{charName}</color> questID: {questID}");
    }

    private void Update()
    {
        if (questDeliveredByPlayer)
        {
            ResetValues();
        }
    }
    #endregion

    #region OnTrigger Functions

    private void OnTriggerEnter(Collider player)
    {
        if (IsPlayerTrigger(player))
        {
            dialogueManager.DisplayInstruction(true, true);
            ToggleNPCMovement(false);

            if (questDeliveredToPlayer)
            {
                dialogueManager.SetCurrentNecessaryValues(idleDialogue.otherSentences, questID);
            }
        }
        Debug.Log($"questDeliveredToPlayer: {questDeliveredToPlayer}");
    }

    private void OnTriggerStay(Collider player)
    {
        Debug.Log($"<color=cyan>dialogueInitiated</color>: {dialogueManager.dialogueInitiated}");

        if (IsPlayerTrigger(player))
        {
            if (Input.GetKeyDown(KeyCode.R) && !dialogueManager.dialogueInitiated)
            {
                if (!questDeliveredToPlayer)
                {
                    questSentence = SentenceForNeededProducts();
                    ActivateQuestItems(productsRequired, true);
                    dialogueManager.StartQuestDialogue(this);
                    questDeliveredToPlayer = true;
                }
                else
                {
                    StartIdleDialogue(idleDialogue.GetRandomIdleSentence(), true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider player)
    {
        if (IsPlayerTrigger(player))
        {
            dialogueManager.DisplayInstruction(false, false);
            ToggleNPCMovement(true);
            RetrieveValues();
        }
    }
    #endregion

    #region Private Functions

    private string SentenceForNeededProducts()
    {
        string initialSentence = "I will just need some ";
        string itemsSentence = string.Empty;
        string finalSentence = string.Empty;
        
        int counter = 0;

        //Debug.Log($"productsRequired amount of items: {productsRequired.Count}");

        foreach (string item in productsRequired)
        {
            if (counter == productsRequired.Count - 2)
            {
                itemsSentence += $"{item} and some ";
            }
            else if (counter == productsRequired.Count - 1)
            {
                itemsSentence += $"{item}.";
            }
            else
            {
                itemsSentence += $"{item}, ";
            }

            //Debug.Log($"product: {item}, counter: {counter}");
            counter++;
        }

        finalSentence = initialSentence + itemsSentence;

        return finalSentence;
    }

    private List<string> AssignProductsRequired()
    {
        List<string> productsRequired = new List<string>();

        foreach (var product in characterProductList)
        {
            if (product.requiresItem)
            {
                productsRequired.Add(product.item.name);
                idleDialogue.SetQuestItem(product.itemName, true); //idleDialogue Quest items are assigned here, is this the best place?
                // Debug.Log($"{product.itemName} is quest item");
            } else if (!product.requiresItem && productsRequired.Contains(product.itemName)) //TODO: List.Contains is not O(1), make it be?
            {
                productsRequired.Remove(product.itemName);
            }
        }

        idleDialogue.FillUpIDList();
        return productsRequired;
    }

    private bool IsPlayerTrigger(Collider isPlayer)
    {
        GameObject playerGameObject; 

        if (isPlayer.transform.parent !=null) //implement Try Catch instead?
        {
            playerGameObject = isPlayer.transform.parent.gameObject;
            if (playerGameObject.TryGetComponent(out PlayerMovement playerScript))
            {
              //  Debug.Log($"playerGameObject name: {playerGameObject.name}");
              //  Debug.Log("IsPlayerTrigger: true");
                return true;
            }
            
        }
       // Debug.Log($"isPlayer Collider name: {isPlayer.name}");
       // Debug.Log("IsPlayerTrigger: false");
        return false;
    }

    private void ToggleNPCMovement( bool toggle)
    {
        transform.parent.GetComponent<WanderingAI>().enabled = toggle; //check for null results later? Change to Event System ThingaMaging?
        transform.parent.GetComponent<NavMeshAgent>().enabled = toggle;
    }

    private void AssignIdleDialogue()
    {
        if (gameObject.TryGetComponent(out IdleDialogue _idleDialogue))
        {
            idleDialogue = _idleDialogue;
        }
        else
        {
            idleDialogue = null;
            Debug.Log("idleDialogue could not be found");
        }
    }

    private void StartIdleDialogue(string idleSentence, bool activateIdleDialogue)
    {
        if (activateIdleDialogue)
        {
            dialogueManager.StartIdleDialogue(idleSentence, true);
        }
        else
        {
            dialogueManager.StartIdleDialogue(idleSentence, false);
        }
        
    }

    private void ActivateQuestItems(List<string> requiredProducts, bool activate)
    {
        foreach (string product in requiredProducts)
        {
            questManager.ActivateQuestProduct(activate, product);
        }

       // Debug.Log("Quest Items activated");
    }

    private void ResetQuestItems(List<string> requiredProducts)
    {
        foreach (string product in requiredProducts)
        {
            questManager.ResetQuestProduct(product);
        }
    }

    private void RetrieveValues()
    {
        playerLieCounter = questManager.GetLieCounter(questID);
        questDeliveredByPlayer = questManager.HasQuestBeenDeliveredByPlayer(questID);
    }

    private void AssignNewQuestItems()
    {
        System.Random random = new System.Random();
        int counter = 0;
        bool listLengthSet = false;
        int listLength = characterProductList.Length;
        int randomNumberOfQuestItems;
        int itemID;

        Debug.Log($"Initial characterProductList length: {listLength}");

        do
        {
            if ((characterProductList[listLength-1].itemName != string.Empty))
            {
                listLengthSet = true;
               // Debug.Log($"<color=red>itemFound</color> during list length operation: {characterProductList[listLength - 1].itemName}");
               // Debug.Log($"<color=red> itemFound.gameObject =</color> {characterProductList[listLength - 1].item}");
               // Debug.Log($"<color=red> itemFound</color> at position: {listLength-1}");
            }
            else
            {
               // Debug.Log("<color=red> Reached listLength-- </color>");
                listLength--;
            }
        }
        while (!listLengthSet);

        Debug.Log($"characterProductList updated length: {listLength}");

        for (int i=0; i < listLength; i++)
        {
            if (characterProductList[i].requiresItem)
            {
                previousItems.Add(counter);
                characterProductList[i].requiresItem = false;
            }
            counter++;
        }

        randomNumberOfQuestItems = random.Next(1, (listLength-previousItems.Count)+1);
        Debug.Log($"randomNumberOfQuestItems: {randomNumberOfQuestItems}, available number of items to choose from: { listLength - previousItems.Count}. Exclusive Upper bound: {(listLength - previousItems.Count)+1}");

        for (int i = 0; i < randomNumberOfQuestItems; i++)
        {
            do
            {
                itemID = random.Next(0, listLength);
            }
            while (previousItems.Contains(itemID) && characterProductList[itemID].requiresItem);

            characterProductList[itemID].requiresItem = true;
            Debug.Log($"<color=yellow>New Quest Product</color>: {characterProductList[itemID].itemName}");
        }

        previousItems.Clear();
        Debug.Log($"<color=yellow>Number of items</color> for new quest: {randomNumberOfQuestItems}");
    }

    private void ResetValues()
    {
        questManager.RemoveQuest(questID);
        ResetQuestItems(productsRequired); //might be unnecessary?
        questDeliveredToPlayer = false;
        questDeliveredByPlayer = false;
        playerLieCounter = 0;

        AssignNewQuestItems();
        productsRequired = AssignProductsRequired();
        questID = questManager.SetNewQuest(charName, productsRequired);
        Debug.Log($"NPC name: <color=blue>{charName}</color> new questID: {questID}");
        Debug.Log("<color=purple>Resetting Values</color>.");
    }

    #endregion
}

[Serializable]
public class ProductNeeded
{
    [SerializeField]
    internal string itemName = string.Empty; //change it so the name is displayed based on assigned object?
    public GameObject item;
    public bool requiresItem;
}