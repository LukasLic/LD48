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
    public TeleportController teleport;

    [Header("Colors")]
    public Color emeraldColor;
    public Color sapphireColor;
    public Color crystalColor;


    [Header("Rest")]
    public AudioClip failedTimeOnContractAudioClip;
    public AudioSource playerAudioSource;

    public AudioClip contractDeliverySuccessAudioClip;
    public AudioSource contractAudioSource;

    public GameObject ContractParentOverlay;
    public Vector3 playerStartPosition;
    public Transform coinPopPosition;
    public GameObject player;
    public GameObject winDialog;
    public GameObject mineEntrance;

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
    private float currentContractSecondsLeft;

    [Header("Active contract info")]
    public Image activeContractGemTypeIcon;
    public Text activeContractAmount;
    public Text remainingContractTime;

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
        SetEnableAndInitActiveContractInfo(false);
        DisplayContract(CurrentContract);
        //DisplayContract("Test title", "This is description. \n Enjoy reading it. \n\n Your dear contractor.", 20, 90, GemType.Sapphire);
    }

    private void SetEnableAndInitActiveContractInfo(bool enabled)
    {
        remainingContractTime.enabled = enabled;
        activeContractAmount.enabled = enabled;
        activeContractGemTypeIcon.enabled = enabled;

        if (enabled)
        {
            currentContractSecondsLeft = CurrentContract.time;
            activeContractAmount.text = CurrentContract.amount.ToString();
            spriteConversion.SetImage(CurrentContract.gemType, activeContractGemTypeIcon);

            switch (CurrentContract.gemType)
            {
                case GemType.Emerald:
                    activeContractGemTypeIcon.color = emeraldColor;
                    gemTypeIcon.color = emeraldColor;
                    break;
                case GemType.Sapphire:
                    activeContractGemTypeIcon.color = sapphireColor;
                    gemTypeIcon.color = sapphireColor;
                    break;
                case GemType.Crystal:
                    activeContractGemTypeIcon.color = crystalColor;
                    gemTypeIcon.color = crystalColor;
                    break;
                case GemType.Coin:
                    activeContractGemTypeIcon.color = Color.yellow;
                    gemTypeIcon.color = Color.yellow;
                    break;
                default:
                    break;
            }
        }
    }

    public void SetContractWindowVisibility(bool state)
    {
        ContractParentOverlay.SetActive(state);
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
            if (true)
            {
                contractAudioSource.PlayOneShot(contractDeliverySuccessAudioClip);
                Debug.Log("Enough gems");
                // Complete contract
                

                //var rewardAmount = currentContractSecondsLeft > 0 ? CurrentContract.reward : CurrentContract.failedReward;
                //InvetoryController.Instance.GetValue(CurrentContract.gemType) >= CurrentContract.amount

                // amount AND time
                //var rewardAmount = InvetoryController.Instance.GetValue(CurrentContract.gemType) >= CurrentContract.amount ?
                //                   (currentContractSecondsLeft > 0 ? CurrentContract.reward : CurrentContract.failedReward)
                //                   : CurrentContract.failedReward;

                var rewardAmount = CurrentContract.failedReward;

                if (InvetoryController.Instance.GetValue(CurrentContract.gemType) >= CurrentContract.amount)
                {
                    Debug.Log("rewardAmount ok");
                    if(currentContractSecondsLeft > 0)
                    {
                        Debug.Log("time ok");

                        rewardAmount = CurrentContract.reward;
                    }
                }

                InvetoryController.Instance.Pay(CurrentContract.gemType, CurrentContract.amount);

                Debug.Log($"RewardAmount {rewardAmount}");
                StartCoroutine(SpawnGemInLoop(0.2f, rewardPrefab, new Vector2(-.5f, .75f), coinPopPosition.position, rewardAmount));

                contractActive = false;
                currentContractIndex++;

                mineEntrance.SetActive(true);

                SetEnableAndInitActiveContractInfo(false);
                ContractParentOverlay.SetActive(false);

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
                    ContractParentOverlay.SetActive(false);
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
            mineEntrance.SetActive(false);

            contractActiveText.enabled = true;
            contractNotActiveText.enabled = false;

            SetEnableAndInitActiveContractInfo(true);
            ContractParentOverlay.SetActive(false);
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

        //spriteConversion.SetImage(contract.gemType, gemTypeIcon);

        switch (CurrentContract.gemType)
        {
            case GemType.Emerald:
                activeContractGemTypeIcon.color = emeraldColor;
                gemTypeIcon.color = emeraldColor;
                break;
            case GemType.Sapphire:
                activeContractGemTypeIcon.color = sapphireColor;
                gemTypeIcon.color = sapphireColor;
                break;
            case GemType.Crystal:
                activeContractGemTypeIcon.color = crystalColor;
                gemTypeIcon.color = crystalColor;
                break;
            case GemType.Coin:
                activeContractGemTypeIcon.color = Color.yellow;
                gemTypeIcon.color = Color.yellow;
                break;
            default:
                break;
        }
    }

    IEnumerator SpawnGemInLoop(float interval, GameObject prefab, Vector2 popForce, Vector3 position, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var gem = Instantiate(prefab, position, Quaternion.identity).GetComponent<Gem>();
            gem.transform.position = position;
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

                teleport.TeleportToStartPosition();
                mineEntrance.SetActive(true);

                playerAudioSource.PlayOneShot(failedTimeOnContractAudioClip);
            }
            TimeSpan timeSpan = TimeSpan.FromSeconds(currentContractSecondsLeft);
            remainingContractTime.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        }
    }
}
