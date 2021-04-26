using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInShopController : MonoBehaviour
{
    public ContractController contractController;
    public ShopController shopController;
    public GameObject player;

    private bool isPlayerInShop;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInShop)
        {
            if(contractController != null)
            {
                contractController.SetContractWindowVisibility(isPlayerInShop);
            }

            if (shopController != null)
            {
                shopController.SetShopWindowVisibility(isPlayerInShop);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(player == null)
        {
            Debug.Log("Player is null in Player in shop controller");
            return;
        }
        if(collider.gameObject == player)
        {
            isPlayerInShop = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (player == null)
        {
            Debug.Log("Player is null in PlayerInShopController");
            return;
        }
        if (collider.gameObject == player)
        {
            isPlayerInShop = false;
            
            if (contractController != null)
            {
                //Debug.LogWarning("Contract controller is null in Player in shop controller.");
                contractController.SetContractWindowVisibility(isPlayerInShop);
            }

            if (shopController != null)
            {
                //Debug.LogWarning("Contract controller is null in Player in shop controller.");
                shopController.SetShopWindowVisibility(isPlayerInShop);
            }
        }
    }
}
