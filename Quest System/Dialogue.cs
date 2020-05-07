using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class Dialogue: MonoBehaviour
{
    public string charName;
    [TextArea(3,10)]
    public string[] sentences;
    internal string questSentence;
    private DialogueManager manager;
    //private Products productScript;
    public ProductNeeded [] characterProductList = new ProductNeeded[8];
    List<string> productsRequired;

    private void Awake()
    {
        manager = FindObjectOfType<DialogueManager>();
    }

    private void Start()
    {
        productsRequired = ProductsRequired();
    }


    #region OnTrigger Functions

    private void OnTriggerEnter(Collider player)
    {
        if (IsPlayerTrigger(player))
        {
            manager.DisplayInstruction(true, true);
        }
    }

    private void OnTriggerStay(Collider player)
    {
        if (IsPlayerTrigger(player))
        {
            if (Input.GetKeyDown(KeyCode.R) && !manager.dialogueInitiated)
            {
                transform.parent.GetComponent<WanderingAI>().enabled = false; //check for null results later
                transform.parent.GetComponent<NavMeshAgent>().enabled = false;
                questSentence = SentenceForNeededProducts();
                manager.StartQuestDialogue(this, productsRequired);
            }
        }
    }

    private void OnTriggerExit(Collider player)
    {
        if (IsPlayerTrigger(player))
        {
            transform.parent.GetComponent<WanderingAI>().enabled = true; //check for null results later Or change to Event System ThingaMaging?
            transform.parent.GetComponent<NavMeshAgent>().enabled = true;
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

        Debug.Log($"productsRequired amount of items: {productsRequired.Count}");

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

            Debug.Log($"product: {item}, counter: {counter}");
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
            }
        }

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
