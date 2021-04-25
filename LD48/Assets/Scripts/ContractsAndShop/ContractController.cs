using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

[Serializable]
public struct Contract
{
    public int amount;
    public GemType gemType;
    public int time;

    public string title;
    public string description;

    public int reward;
    public int failedReward;
}

public class ContractController : MonoBehaviour
{
    public GameObject ShopParentOverlay;

    [Header("Contract")]
    public GameObject rewardPrefab;
    public Text contractNotActiveText;
    public Text contractActiveText;
    public Text contractDescription;
    public Text contractTitle;
    public Text contractTime;
    public Text contractAmount;
    public Image gemTypeIcon;
    public GemTypeToSpriteConversion spriteConversion;
    private bool contractActive = false;

    [Header("ContractList")]
    public Contract[] contracts;
    private int currentContractIndex = 0;

    private Contract CurrentContract
    {
        get
        {
            return contracts[currentContractIndex];
        }
    }

    // TODO: List of contract structs.

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            SetShopWindowVisibility(true);
        }
    }

    private void Awake()
    {
        // TEST:
        contractActive = false;
        contractActiveText.enabled = false;
        contractNotActiveText.enabled = true;
        DisplayContract(CurrentContract);
        //DisplayContract("Test title", "This is description. \n Enjoy reading it. \n\n Your dear contractor.", 20, 90, GemType.Sapphire);
    }

    public void SetShopWindowVisibility(bool state)
    {
        ShopParentOverlay.SetActive(state);
    }

    public void ContractButton()
    {
        if(contractActive)
        {
            // Verify if enough gems are in the inventory to finish contract
            // substract gems from inventory and give money
            if(InvetoryController.Instance.GetValue(CurrentContract.gemType) >= CurrentContract.amount)
            {
                // Complete contract
                InvetoryController.Instance.Pay(CurrentContract.gemType, CurrentContract.amount);

                // TODO TIME var rewardAmount = 
                StartCoroutine(SpawnGemInLoop(0.2f, rewardPrefab, new Vector2(.5f, .75f), CurrentContract.amount));

                contractActive = false;
                currentContractIndex++;

                DisplayContract(CurrentContract);

                contractActiveText.enabled = false;
                contractNotActiveText.enabled = true;
            }
            else
            {
                // TODO: Play sound or effect
            }

        }
        else
        {
            // Activate the contract
            contractActive = true;

            contractActiveText.enabled = true;
            contractNotActiveText.enabled = false;

            // TODO: Effect
        }
    }

    private void DisplayContract(Contract contract)
    {
        contractTitle.text = contract.title;
        contractDescription.text = contract.description.Replace("\\n", "\n");

        contractAmount.text = contract.amount.ToString();
        var seconds = (contract.time % 60 == 0) ? "00" : (contract.time % 60).ToString();
        contractTime.text = $"{contract.time / 60}:{seconds}";

        spriteConversion.SetImage(contract.gemType, gemTypeIcon);
    }

    IEnumerator SpawnGemInLoop(float interval, GameObject prefab, Vector2 popForce, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var gem = Instantiate(prefab, Vector3.zero, Quaternion.identity).GetComponent<Gem>();
            gem.Init(popForce, InvetoryController.Instance);

            yield return new WaitForSeconds(interval);
        }
    }
}
