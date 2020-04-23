using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteraction : MonoBehaviour
{
    public bool isQuestNPC = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!this.GetComponent<Collider>()) {
            this.gameObject.AddComponent<BoxCollider>();
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void QuestMsgSystem(string npcTitle, string relationPlayerToNpc, int nItems) {
         string npcName = npcTitle;
        string quest = "Hi there my dear " + relationPlayerToNpc + ". I hope you are doing well given this terrible times and I am sorry to bother you but could you perhaps get" +
           " a few stuff in the store for me? I just need " + nItems + " things";
        string acceptedRequest = "Thank you very much my " + relationPlayerToNpc + " stay safe and be sure to avoid other people, lest u get the virus!";

    }

}
