using System;
using UnityEngine;

public class Cashier : MonoBehaviour
{
    private SoundPlayer soundPlayer;
    private CartItemManager cartItemManager = null;
    private PickUpItems cartPickUpSystem = null;

    void Start()
    {
        soundPlayer = FindObjectOfType<SoundPlayer>();
    }

    #region OnTrigger Functions

    private void OnTriggerEnter(Collider cart)
    {
        if (IsTargetCollider(cart))
        {
            cartItemManager = cartPickUpSystem.itemManager;

            if(cartItemManager.IsAnyProductChecked())
            cartPickUpSystem.canvasBuyItems.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider cart)
    {
        if (IsTargetCollider(cart))
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                cartPickUpSystem.canvasBuyItems.SetActive(false);
                cartItemManager.CheckProduct(false);
                cartPickUpSystem.SpawmAnimationProduct("goods-top", true);
                cartPickUpSystem.SetChildProductActive(false, "goods");
                soundPlayer.PlaySoundClip("Writting"); // delete this or find a way to further increase its volume
                soundPlayer.PlaySoundClip("Cashier");
            }
        }
    }

    private void OnTriggerExit(Collider cart)
    {
        if (IsTargetCollider(cart))
        {
            cartPickUpSystem.canvasBuyItems.SetActive(false);
        }
    }
    #endregion

    private bool IsTargetCollider(Collider target)
    {
        bool isTarget = false;
        GameObject targetObj = target.gameObject;

        if (targetObj.name.Equals("PickUpSystem", StringComparison.OrdinalIgnoreCase))
        {
            if (targetObj.TryGetComponent(out PickUpItems cartPickUp))
            {
                cartPickUpSystem = cartPickUp;
                isTarget = true;
            }
        }

        return isTarget;
    }
}
