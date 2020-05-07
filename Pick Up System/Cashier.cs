using System;
using System.Collections.Generic;
using UnityEngine;

public class Cashier : MonoBehaviour
{
    private SoundPlayer soundPlayer;
    private CartItemManager cartItemManager;
    private QuestManager questManager;
    private PickUpItems cartPickUpSystem;
    private List<string> productsInCart;

    void Start()
    {
        soundPlayer = FindObjectOfType<SoundPlayer>();
        questManager = FindObjectOfType<QuestManager>();
    }

    #region OnTrigger Functions

    private void OnTriggerEnter(Collider cart)
    {
        if (IsTargetCollider(cart))
        {
            cartItemManager = cartPickUpSystem.itemManager;
            productsInCart = cartItemManager.ReturnCartProducts();
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
                CheckOutProducts();
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

    private void CheckOutProducts()
    {
        foreach (string product in productsInCart)
        {
            if (questManager.IsQuestproduct(product) && !questManager.HasPlayerAcquiredProduct(product))
                questManager.PlayerAcquiredProduct(true, product);
        }

        cartItemManager.CheckOutCartProducts();
        cartPickUpSystem.SpawmAnimationProduct("goods-top", true);
        cartPickUpSystem.SetChildProductActive(false, "goods");
        soundPlayer.PlaySoundClip("Writting"); // delete this or find a way to further increase its volume
        soundPlayer.PlaySoundClip("Cashier");
    }
}
