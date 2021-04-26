using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public GameObject player;

    [Header("Teleport")]
    public int numberOfTeleports;
    public int maxNumberOfTeleports;
    public int teleportPrice;

    [Header("Pickaxe")]
    public int pickaxeLevel;
    public int maxPickaxeLevel;
    public int pickaxeLevelPrice;

    [Header("Jetpack")]
    public int jetpackPowerLevel;
    public int maxJetpackPowerLevel;
    public int jetpackFuelPowerPrice;

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
        if (InvetoryController.Instance.HasEnoughGems(GemType.Coin, teleportPrice)
            && numberOfTeleports < maxNumberOfTeleports)
        {
            InvetoryController.Instance.Pay(GemType.Coin, teleportPrice);
            numberOfTeleports++;
            var teleportController = player.GetComponent<TeleportController>();
            if (teleportController == null)
            {
                Debug.LogWarning("Teleport controller in shop is null");
                return;
            }
            teleportController.numberOfAvailableTeleports += 1;
        }
        else
        {
            Debug.Log("Cannot buy teleport. Not enough money or max level reached");
        }
    }

    public void BuyPickaxeLevel()
    {
        if (InvetoryController.Instance.HasEnoughGems(GemType.Coin, pickaxeLevelPrice)
            && pickaxeLevel < maxPickaxeLevel)
        {
            InvetoryController.Instance.Pay(GemType.Coin, pickaxeLevelPrice);
            pickaxeLevel++;
            //increase pickaxe level
            var diggingController = player.GetComponent<DiggingController>();
            if(diggingController == null)
            {
                Debug.LogWarning("Digging controller in shop is null");
                return;
            }
            diggingController.digs += (pickaxeLevel + 1) + 1;
            diggingController.criticalPointDigs = (pickaxeLevel + 1) + 2;
        }
        else
        {
            Debug.Log("Cannot buy pickaxe level. Not enough money or max level reached");
        }
    }

    public void BuyJetpackFuel()
    {
        if (InvetoryController.Instance.HasEnoughGems(GemType.Coin, jetpackFuelPowerPrice)
            && jetpackPowerLevel < maxJetpackPowerLevel)
        {
            InvetoryController.Instance.Pay(GemType.Coin, jetpackFuelPowerPrice);
            jetpackPowerLevel++;
            var movementController = player.GetComponent<PlayerMovement>();
            if (movementController == null)
            {
                Debug.LogWarning("Movement controller in shop is null");
                return;
            }

            movementController.maxJetpackVelocity = (jetpackPowerLevel + 1) * 5;
        }
        else
        {
            Debug.Log("Cannot buy jetpack fuel. Not enough money or max level reached");
        }
    }
}
