using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public int numberOfTeleports;
    public int pickaxeLevel;
    public int jetpackFuelLevel;

    public int maxNumberOfTeleports;
    public int maxPickaxeLevel;
    public int maxJetpackFuelLevel;

    public int teleportPrice;
    public int pickaxeLevelPrice;
    public int jetpackFuelLevelPrice;

    public GameObject player;

    // Update is called once per frame
    public void Update()
    {
        // TEST, REMOVE
        if (Input.GetKeyDown(KeyCode.J))
        {
            BuyTeleport();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            BuyPickaxeLevel();     
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            BuyJetpackFuel();
        }
    }

    public void BuyTeleport()
    {
        if (InvetoryController.Instance.HasEnoughCoins(teleportPrice) 
            && numberOfTeleports < maxNumberOfTeleports)
        {
            Debug.Log("Can buy teleport");
            InvetoryController.Instance.PayCoins(teleportPrice);
            numberOfTeleports++;
            var teleportController = player.GetComponent<TeleportController>();
            if (teleportController == null)
            {
                Debug.LogWarning("Teleport controller in shop is null");
                return;
            }
            teleportController.numberOfAvailableTeleports += 1;
        }
    }

    public void BuyPickaxeLevel()
    {
        if (InvetoryController.Instance.HasEnoughCoins(pickaxeLevelPrice)
            && pickaxeLevel < maxPickaxeLevel)
        {
            Debug.Log("Can buy pickaxe");
            InvetoryController.Instance.PayCoins(pickaxeLevelPrice);
            pickaxeLevel++;
            //increase pickaxe level
            var diggingController = player.GetComponent<DiggingController>();
            if(diggingController == null)
            {
                Debug.LogWarning("Digging controller in shop is null");
                return;
            }
            diggingController.digs += 1;
            diggingController.criticalPointDigs += 2;
        }
    }

    public void BuyJetpackFuel()
    {
        if (InvetoryController.Instance.HasEnoughCoins(jetpackFuelLevelPrice)
            && jetpackFuelLevel < maxJetpackFuelLevel)
        {
            Debug.Log("Can buy fuel");
            InvetoryController.Instance.PayCoins(jetpackFuelLevelPrice);
            jetpackFuelLevel++;
            var movementController = player.GetComponent<PlayerMovement>();
            if (movementController == null)
            {
                Debug.LogWarning("Movement controller in shop is null");
                return;
            }

            movementController.maxJetpackFuel *= 2; 
        }
    }
}
