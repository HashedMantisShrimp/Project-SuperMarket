using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKart : MonoBehaviour
{
    public PlayerMovement playerScript;
    //private bool kartIsTaken = false;
    public Canvas instructions;
    public SoundPlayer soundPlayer;
    public PickUpItems itemManager;
    public CartItemManager cartMan;

    void Start()
    {
        //soundPlayer = Camera.main.gameObject.GetComponent<SoundPlayer>(); //This is returning null for whatever reason, despite the script being present in the cam.
        if (instructions) {
            instructions.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.F) && playerScript.hasKart) { //Implement mechanic to detach from the cart here.
             DetachFromParent();
            StartCoroutine(EnableHasKart(false));
             Debug.Log(gameObject + " has detached from its previous parent.");
         }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
            && playerScript.hasKart && !soundPlayer.isSourcePlaying("Empty-Obj")) //If any of the keys are pressed, trolley sound begins, otherwise it stops
        {
            soundPlayer.PlaySoundClip("Trolley", true, "Empty-Obj");
        }
        else
        if ((!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) &&
           !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E)) && soundPlayer.isSourcePlaying("Empty-Obj"))
        {
            soundPlayer.StopSoundClip("Empty-Obj");
            //Debug.Log("ClipStop function for cart was called");
        }
    }

    private void OnTriggerEnter(Collider player)
    {
        instructions.gameObject.SetActive(true);
        Debug.Log("Enabled shud have been called");
        soundPlayer.PlaySoundClip("Pop-Up");
        //insert pop up sound
    }

    private void OnTriggerStay(Collider player)
    {
        GameObject grandParent = player.transform.parent.gameObject; //GrandParent is The actual Player. 'player' is the empty object
        //Debug.Log("OnTriggerStay ");

        if (grandParent != null)
        {
            //Display the name of the grand parent of the player.
            //Debug.Log("Player's Grand parent: " + player.transform.parent.name);

            if (grandParent.GetComponent<PlayerMovement>())
            {
                playerScript = grandParent.GetComponent<PlayerMovement>();

                if (Input.GetKeyDown(KeyCode.F) && !playerScript.hasKart)
                {
                    SetParent(player.gameObject);   //Sets the player's gameObject as parent
                    soundPlayer.PlaySoundClip("Pick-Up-Trolley");
                    instructions.gameObject.SetActive(false);
                    StartCoroutine(EnableHasKart(true));
                    //itemManager = playerScript.gameObject.GetComponent<PickUpItems>();
                   // itemManager.cartM = this.cartMan;

                }


            }
        }


        
    }

    private void OnTriggerExit(Collider player)
    {
        instructions.gameObject.SetActive(false);
    }

    private IEnumerator EnableHasKart(bool cartIsPresent) {
        yield return new WaitForSeconds(.1f);
        playerScript.hasKart = cartIsPresent;
    }
    
    
    public void SetParent(GameObject newParent)
    {
        gameObject.transform.parent = newParent.transform;
        
        Debug.Log("Player's Parent: " + gameObject.transform.parent.name);
    }

    public void DetachFromParent()
    {
        transform.parent = null;
    }
}
