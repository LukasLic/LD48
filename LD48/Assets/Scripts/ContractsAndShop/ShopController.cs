using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct ItemShopControls
{
    public Text priceText;
    public Text upgradeText;
    public Text maxUpgradedText;
}

public class ShopController : MonoBehaviour
{
    public GameObject ShopParentOverlay;
    public GameObject player;

    [Header("Teleport")]
    public int numberOfTeleports;
    public int maxNumberOfTeleports;
    public int teleportPrice;
    public ItemShopControls teleportItemControls;

    [Header("Pickaxe")]
    public int pickaxeLevel;
    public int maxPickaxeLevel;
    public int pickaxeLevelPrice;
    public ItemShopControls pickaxeItemControls;

    [Header("Jetpack")]
    public int jetpackPowerLevel;
    public int maxJetpackPowerLevel;
    public int jetpackFuelPowerPrice;
    public ItemShopControls jetpackItemControls;

    private void Awake()
    {
        teleportItemControls.priceText.text = teleportPrice.ToString();
        pickaxeItemControls.priceText.text = teleportPrice.ToString();
        jetpackItemControls.priceText.text = teleportPrice.ToString();

        teleportItemControls.maxUpgradedText.enabled = false;
        pickaxeItemControls.maxUpgradedText.enabled = false;
        jetpackItemControls.maxUpgradedText.enabled = false;

        teleportItemControls.upgradeText.enabled = true;
        pickaxeItemControls.upgradeText.enabled = true;
        jetpackItemControls.upgradeText.enabled = true;
    }

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
        if (InvetoryController.Instance.HasEnoughGems(GemType.Coin, teleportPrice))
        {
            var teleportController = player.GetComponent<TeleportController>();
            if (teleportController == null)
            {
                Debug.LogWarning("Teleport controller in shop is null");
                return;
            }

            if (teleportController.numberOfAvailableTeleports < 1)
            {
                InvetoryController.Instance.Pay(GemType.Coin, teleportPrice);

                teleportController.numberOfAvailableTeleports = 1;

                teleportItemControls.maxUpgradedText.enabled = true;
                teleportItemControls.upgradeText.enabled = false;
            }
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
            diggingController.digs += pickaxeLevel + 1;
            diggingController.criticalPointDigs = pickaxeLevel + 2;
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

    public void UpdateTeleportCount(int newCount)
    {
        if(newCount < 1)
        {
            teleportItemControls.maxUpgradedText.enabled = false;
            teleportItemControls.upgradeText.enabled = true;
        }
        else
        {
            teleportItemControls.maxUpgradedText.enabled = true;
            teleportItemControls.upgradeText.enabled = false;
        }
    }

    public void SetShopWindowVisibility(bool state)
    {
        ShopParentOverlay.SetActive(state);
        var diggingController = player.GetComponent<DiggingController>();
        if (diggingController == null)
        {
            Debug.LogWarning("Digging controller is null in SetShopWindowVisibility method in ContractController");
        }
        diggingController.enabled = !state;
    }
}
