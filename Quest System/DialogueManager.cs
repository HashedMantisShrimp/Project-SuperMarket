using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> dialogueQueue;
    private PlayerMovement playerMovement;
    public GameObject playerDialogueBox;
    private TMPro.TextMeshProUGUI textMeshPro;
    private QuestInquisitionSentences questInquisitionSentences;
    internal bool dialogueInitiated;
    private bool questInquisition = false;
    private bool isCurrentQuestComplete;
    private bool hasCurrentQuestBeenDelivered = false;
    private int currentLieCounter;

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
        HandleDialogue();
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

    internal void StartIdleDialogue(string idleSentence, bool askAboutQuest)
    {
        dialogueQueue.Clear();
        TogglePlayerMovement(false);
        dialogueQueue.Enqueue(idleSentence);

        if (askAboutQuest)
        {
            dialogueQueue.Enqueue(questInquisitionSentences.HasPlayerCompletedQuest);
            questInquisition = true;
        }
            
        DisplayNextDialogue();
        dialogueInitiated = true;
    }

    internal void DisplayInstruction(bool activate, bool setInstruction)
    {
        if (setInstruction)
            textMeshPro.SetText("Press R to talk");

        playerDialogueBox.SetActive(activate);
    }

    internal void SetCurrentNecessaryValues(QuestInquisitionSentences otherSentences, bool isQuestComplete)
    {
        SetQuestInquisitionSentences(otherSentences);
        ToggleCurrentQuest(isQuestComplete);
    }

    internal int ReturnLieCounter()
    {
        return currentLieCounter;
    }

    internal bool QuestDeliveredByPlayer()
    {
        return hasCurrentQuestBeenDelivered;
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
        Debug.Log($"sentence dequeued: {sentence}");
        Debug.Log($"<color=blue> enqueuedOtherSentences</color>: {questInquisition}, <color=blue>dialogueQueueCount</color>: {dialogueQueue.Count}");
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

    private void ToggleCurrentQuest(bool isQuestComplete)
    {
        isCurrentQuestComplete = isQuestComplete;
    }

    private void SetQuestInquisitionSentences(QuestInquisitionSentences _questInquisitionSentences)
    {
        questInquisitionSentences = _questInquisitionSentences;
    }

    private void HandleDialogue()
    {
        if (questInquisition && dialogueQueue.Count == 0)
        {
            if (questInquisition == true && dialogueQueue.Count == 0)
            {
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    Debug.Log("<color=lightblue>Player said yes</color>");

                    if (isCurrentQuestComplete)
                    {
                        StartIdleDialogue(questInquisitionSentences.ThankPlayerForItems, false);
                        hasCurrentQuestBeenDelivered = true;
                        questInquisition = false;
                    }
                    else
                    {
                        hasCurrentQuestBeenDelivered = false;

                        if (currentLieCounter > 0)
                        {
                            StartIdleDialogue(questInquisitionSentences.WhyYouAlwaysLying, false);
                        }
                        else if (currentLieCounter == 0)
                        {
                            StartIdleDialogue(questInquisitionSentences.WhyDidYouLie, false);
                        }

                        currentLieCounter++;
                        questInquisition = false;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.N))
                {
                    Debug.Log("<color=darkblue>Player said no</color>");
                    StartIdleDialogue(questInquisitionSentences.AlrightGoodLuck, false);
                    hasCurrentQuestBeenDelivered = false;
                    questInquisition = false;
                }
            }
        }
        else if (dialogueInitiated && Input.GetKeyDown(KeyCode.Space)) // indicate on the UI that player has to press space to progress in the dialogue
        {
            DisplayNextDialogue();
        }
    }
    #endregion

}
