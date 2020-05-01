using System.Collections;
using UnityEngine;

public class ShopKart : MonoBehaviour
{
    public PlayerMovement playerScript;
    public Canvas instructions;
    private SoundPlayer soundPlayer;

    #region Init Functions
    void Start()
    {
        soundPlayer = FindObjectOfType<SoundPlayer>();
        if (instructions)
        {
            instructions.gameObject.SetActive(false);
            gameObject.GetComponent<BoxCollider>().isTrigger = false;
        }
    }
    
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.F) && playerScript.hasKart && (transform.parent != null))
         {
            DetachFromParent();

            StartCoroutine(EnableHasKart(false));
            gameObject.GetComponent<BoxCollider>().enabled = true;
            //Debug.Log($"{gameObject} has detached from its previous parent.");
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
        }
    }
    #endregion

    #region OnTrigger Functions
    private void OnTriggerEnter(Collider player)
    {
        instructions.gameObject.SetActive(true);
        //Debug.Log("Enabled shud have been called");
        soundPlayer.PlaySoundClip("Pop-Up");
    }

    private void OnTriggerStay(Collider player)
    {
        GameObject grandParent = player.transform.parent.gameObject;

        if (grandParent != null)
        {
            if (grandParent.GetComponent<PlayerMovement>())
            {
                playerScript = grandParent.GetComponent<PlayerMovement>();

                if (Input.GetKeyDown(KeyCode.F) && !playerScript.hasKart)
                {
                    SetParent(player.gameObject);   //Sets the player's gameObject as parent
                    soundPlayer.PlaySoundClip("Pick-Up-Trolley");
                    instructions.gameObject.SetActive(false);
                    gameObject.GetComponent<BoxCollider>().enabled = false;
                    StartCoroutine(EnableHasKart(true));
                }
            }
        }
    }

    private void OnTriggerExit(Collider player)
    {
        instructions.gameObject.SetActive(false);
    }
    #endregion

    #region Miscellaneous Functions

    private IEnumerator EnableHasKart(bool cartIsPresent) {
        yield return new WaitForSeconds(.1f);
        playerScript.hasKart = cartIsPresent;
    }
    
    
    public void SetParent(GameObject newParent)
    {
        gameObject.transform.parent = newParent.transform;

        // Debug.Log($"Player's Parent: {gameObject.transform.parent.name}");
    }

    public void DetachFromParent()
    {
        transform.parent = null;
    }
    #endregion
}
