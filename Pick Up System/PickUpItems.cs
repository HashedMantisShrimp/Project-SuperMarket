using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    public Canvas instructions;
    public PlayerMovement playerScript;
    public CartItemManager cartM;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //OnCollisionTrigger If player has a cart and if he presses R' the item should instantiate in the kart
    }

    private void OnTriggerEnter(Collider objectCollided)
    {
        instructions.gameObject.SetActive(true);
        //if what the player collided against is indeed a object that can be picked up and bought, then maybe have a sound pop up to say that u can pick it up
        //Also on collisionEnter, activate a canvas that shows msg press R to pick up object

    }

    private void OnTriggerStay(Collider objectCollided)
    {
        GameObject objC = objectCollided.gameObject;
        if (objectCollided.GetComponent<NpcItem>() && playerScript.hasKart) {
            if (objectCollided.GetComponent<NpcItem>().isNPCItem) {
                if (Input.GetKeyDown(KeyCode.R)) {
                    instructions.gameObject.SetActive(false);
                    cartM = objectCollided.GetComponentInChildren<CartItemManager>();
                   // cartM.objC
                    // disable the canvas window
                    //activate Empty game object with  correspondent child/item
                }
            }
        }
    }

    private void OnTriggerExit(Collider objectCollided)
    {
        instructions.gameObject.SetActive(false);
    }
}
