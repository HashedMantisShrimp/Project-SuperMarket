using System.Collections;
using System;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    private GameObject instructionsCanvas;
    public GameObject productObj;
    private Products productScript;
    private Transform parentTransform;
    private SoundPlayer soundPlayer;
    private PlayerMovement playerScript;
    

    void Start()
    {
        productScript = Camera.main.GetComponent<Products>();
        soundPlayer = Camera.main.GetComponent<SoundPlayer>();
        instructionsCanvas = FindCanvas();
        playerScript = FindObjectOfType<PlayerMovement>();
    }

    
    void Update()
    {
        if (!playerScript.hasKart && instructionsCanvas.activeSelf)
        {
            //Debug.Log("Player has no cart, canvas shud be off");
            instructionsCanvas.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider objectCollided)
    {
        if (instructionsCanvas != null && IsTargetTrigger(objectCollided) && playerScript.hasKart)
        {
            instructionsCanvas.SetActive(true);

            soundPlayer.PlaySoundClip("Pop-Up");
        }
    }

    private void OnTriggerStay(Collider objectCollided)
    {
        GameObject objC = objectCollided.gameObject;

        if (IsTargetTrigger(objectCollided))
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                instructionsCanvas.SetActive(false);
                soundPlayer.PlaySoundClip("Box 1");
                objectCollided.enabled = false;
                SetChildProductActive(true, true, 1f,"goods-top", productObj.name);
                Debug.Log("Product acquired, product has name of: " + productObj.name);
            }
        }
    }

    private void OnTriggerExit(Collider objectCollided)
    {
        if (instructionsCanvas != null && instructionsCanvas.activeSelf)
            instructionsCanvas.gameObject.SetActive(false);
    }



    private bool IsTargetTrigger(Collider targetCollider) {
        productObj = productScript.GetProduct(targetCollider.name);
        
        bool val = productObj !=null ? true : false;

        return val;
    }
    
    private GameObject FindCanvas()
    {
        parentTransform = this.transform.parent;
        //Debug.Log("Parent: " + parentTransform);

        foreach (Transform child in parentTransform)
        {
            //Debug.Log("child of parent " + parentTransform.name + ": " + child.name);
            if (child.name == "Canvas-P")
            {
                //Debug.Log("Found Canvas: " + child.name);
                return child.gameObject;
            }
        }
        //Debug.Log("Couldnt find canvas");
        return null;
    }

    private void SetChildProductActive(bool activate, string targetChild, string productName) //only activates children with productName
    {
        Transform goodsTransfrom = null;

        foreach (Transform childOfGameObject in gameObject.transform)
        {
            if (childOfGameObject.name.Equals(targetChild, StringComparison.OrdinalIgnoreCase))
                goodsTransfrom = childOfGameObject;
        }

        foreach (Transform child in goodsTransfrom)
        {
            if(child.name.Equals(productName, StringComparison.OrdinalIgnoreCase))
            child.gameObject.SetActive(activate);
        }
    }

    private void SetChildProductActive(bool activate, bool deleteChild, float time, string targetChild, string productName)
    { //actiavtes children with productname and after some time, destroys it
        Transform goodsTransfrom = null;

        foreach (Transform childOfGameObject in gameObject.transform) {
            if (childOfGameObject.name.Equals(targetChild, StringComparison.OrdinalIgnoreCase))
                goodsTransfrom = childOfGameObject;
        }

        Transform childProduct = null;
        foreach (Transform childFound in goodsTransfrom)
        {
            if (childFound.name.Equals(productName, StringComparison.OrdinalIgnoreCase)) {
                childProduct = childFound;
                childProduct.gameObject.SetActive(activate);

                if(deleteChild)
                StartCoroutine(DestroyChild(childProduct, time, true));
                
            }
                
        }
    }

    private IEnumerator DestroyChild(Transform objectToDestroy, float timeToWait, bool stationaryObject)
    { //Destroys "gameObject animation" and then activates stationary gameObject
        yield return new WaitForSeconds(timeToWait);
        Destroy(objectToDestroy.gameObject);
        // Debug.Log("Child Deleted");

        if (stationaryObject)
        SetChildProductActive(true, "goods", productObj.name);
    }

}
