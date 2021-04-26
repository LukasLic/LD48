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
    public Vector3 playerStartPosition;
    public GameObject player;
    public GameObject winDialog;

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
    public Text remainingContractTime;
    private bool contractActive = false;
    private float currentContractSecondsLeft;

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
        SubstractDeltaFromContractTime();
    }

    private void Awake()
    {
        // TEST:
        contractActive = false;
        contractActiveText.enabled = false;
        contractNotActiveText.enabled = true;
        remainingContractTime.enabled = false;
        DisplayContract(CurrentContract);
        //DisplayContract("Test title", "This is description. \n Enjoy reading it. \n\n Your dear contractor.", 20, 90, GemType.Sapphire);
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

    public void ContractButton()
    {
        if(contractActive)
        {
            // Verify if enough gems are in the inventory to finish contract
            // substract gems from inventory and give money
            if (InvetoryController.Instance.GetValue(CurrentContract.gemType) >= CurrentContract.amount)
            {
                Debug.Log("Enough gems");
                // Complete contract
                InvetoryController.Instance.Pay(CurrentContract.gemType, CurrentContract.amount);

                var rewardAmount = currentContractSecondsLeft > 0 ? CurrentContract.reward : CurrentContract.failedReward;
                Debug.Log($"RewardAmount {rewardAmount}");
                StartCoroutine(SpawnGemInLoop(0.2f, rewardPrefab, new Vector2(.5f, .75f), rewardAmount));

                contractActive = false;
                currentContractIndex++;

                remainingContractTime.enabled = false;

                //show next contract, if there is any, otherwise show winning dialog
                Debug.Log($"CurrentContractIndex: {currentContractIndex}");
                if(currentContractIndex < contracts.Length)
                {
                    DisplayContract(CurrentContract);

                    contractActiveText.enabled = false;
                    contractNotActiveText.enabled = true;
                }
                else
                {
                    ShopParentOverlay.SetActive(false);
                    var winDialogController = winDialog.GetComponent<WinDialogController>();
                    if(winDialogController == null)
                    {
                        Debug.LogWarning("WinDialogController is null in ContractController");
                    }
                    winDialogController.ShowDialog();
                }
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
            currentContractSecondsLeft = CurrentContract.time;

            contractActiveText.enabled = true;
            contractNotActiveText.enabled = false;

            remainingContractTime.enabled = true;

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

    private void SubstractDeltaFromContractTime()
    {
        //Is this correct?
        if (contractActive && currentContractSecondsLeft > 0)
        {
            currentContractSecondsLeft -= Time.deltaTime;
            if (currentContractSecondsLeft <= 0)
            {
                currentContractSecondsLeft = 0;
                //TODO: Add sound effect
            }
            TimeSpan timeSpan = TimeSpan.FromSeconds(currentContractSecondsLeft);
            remainingContractTime.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        }
    }
}
