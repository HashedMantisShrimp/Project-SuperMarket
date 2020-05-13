using System.Collections;
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
    private bool questDelivered = false;
    private IdleDialogue idleDialogue;

    #region Init Functions
    private void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        questManager = FindObjectOfType<QuestManager>();
        AssignIdleDialogue();
    }

    private void Start()
    {
        productsRequired = ProductsRequired();
    }
    #endregion

    #region OnTrigger Functions

    private void OnTriggerEnter(Collider player)
    {
        if (IsPlayerTrigger(player))
        {
            dialogueManager.DisplayInstruction(true, true);
            ToggleNPCMovement(false);
        }
    }

    private void OnTriggerStay(Collider player)
    {
        if (IsPlayerTrigger(player))
        {
            if (Input.GetKeyDown(KeyCode.R) && !dialogueManager.dialogueInitiated)
            {
                if (!questDelivered)
                {
                    questSentence = SentenceForNeededProducts();
                    ActivateQuestItems(productsRequired);
                    dialogueManager.StartQuestDialogue(this);
                    questDelivered = true;
                }
                else
                {
                    StartIdleDialogue();
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

    private List<string> ProductsRequired()
    {
        List<string> productsRequired = new List<string>();

        foreach (var product in characterProductList)
        {
            if (product.requiresItem)
            {
                productsRequired.Add(product.item.name);
                idleDialogue.SetQuestItem(product.itemName, true); //idleDialogue Quest items are assigned here, is this the best place?
               // Debug.Log($"{product.itemName} is quest item");
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

    private void StartIdleDialogue()
    {
        string idleSentence = idleDialogue.GetRandomIdleSentence(); 

        dialogueManager.StartIdleDialogue(idleSentence);

    }

    private void ActivateQuestItems(List<string> requiredProducts)
    {
        foreach (string product in requiredProducts)
        {
            questManager.ActivateQuestProduct(true, product);
        }

        Debug.Log("Quest Items activated");
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