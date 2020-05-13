using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> dialogueQueue;
    private PlayerMovement playerMovement;
    public GameObject playerDialogueBox;
    private TMPro.TextMeshProUGUI textMeshPro;
    internal bool dialogueInitiated;

    #region Init Functions

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        dialogueQueue = new Queue<string>();
        textMeshPro = playerDialogueBox.GetComponent<TMPro.TextMeshProUGUI>();
        dialogueInitiated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueInitiated && Input.GetKeyDown(KeyCode.Space)) // indicate on the UI that player has to press space to progress in the dialogue
        {
            DisplayNextDialogue();
        }
    }
    #endregion

    #region Internal Functions
    
    internal void StartQuestDialogue(Dialogue dialogue)
    {
        dialogueQueue.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            dialogueQueue.Enqueue(sentence);
        }
        dialogueQueue.Enqueue(dialogue.questSentence);
        TogglePlayerMovement(false);
        DisplayNextDialogue();
        dialogueInitiated = true;
    }

    internal void StartIdleDialogue(string idleSentence)
    {
        dialogueQueue.Clear();
        TogglePlayerMovement(false);
        dialogueQueue.Enqueue(idleSentence);
        DisplayNextDialogue();

        dialogueInitiated = true;
    }

    internal void DisplayInstruction(bool activate, bool setInstruction)
    {
        if (setInstruction)
            textMeshPro.SetText("Press R to talk");

        playerDialogueBox.SetActive(activate);
    }
    #endregion

    #region Private Functions

    private void DisplayNextDialogue()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = dialogueQueue.Dequeue();
        textMeshPro.SetText(sentence);
        //Debug.Log($"sentence: {sentence}");
    }

    private void EndDialogue()
    {
        dialogueInitiated = false;
        Debug.Log("End of dialogueQueue");
        DisplayInstruction(false, false);
        TogglePlayerMovement(true);
    }

    private void TogglePlayerMovement(bool activate)
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = activate;
        }
    }
    #endregion

}
