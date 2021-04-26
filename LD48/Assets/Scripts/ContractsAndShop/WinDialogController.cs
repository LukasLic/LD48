using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDialogController : MonoBehaviour
{
    public GameObject winDialog;
    public GameObject player;

    public void ShowDialog()
    {
        winDialog.SetActive(true);
        var playerMovement = player.GetComponent<PlayerMovement>();
        var diggingController = player.GetComponent<DiggingController>();
        if(playerMovement == null || diggingController == null)
        {
            Debug.LogWarning("Players component to be set inactive during active win dialog are null");
            return;
        }
        playerMovement.enabled = false;
        diggingController.enabled = false;
    }

    public void Restart()
    {
        HideDialog();
        GameController.Instance.CompleteRestart();
    }

    private void HideDialog()
    {
        winDialog.SetActive(false);
    }
}
