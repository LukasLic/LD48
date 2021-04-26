using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInShopController : MonoBehaviour
{
    public GameObject gameController;
    public GameObject player;

    private bool isPlayerInShop;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInShop)
        {
            var contractController = gameController.GetComponent<ContractController>();
            if(contractController == null)
            {
                Debug.LogWarning("Contract controller is null in Player in shop controller.");
            }
            contractController.SetShopWindowVisibility(true);
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
            Debug.Log("Player is null in Player in shop controller");
            return;
        }
        if (collider.gameObject == player)
        {
            isPlayerInShop = false;
            var contractController = gameController.GetComponent<ContractController>();
            if (contractController == null)
            {
                Debug.LogWarning("Contract controller is null in Player in shop controller.");
            }
            contractController.SetShopWindowVisibility(false);

        }
    }
}
