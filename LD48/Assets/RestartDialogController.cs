using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartDialogController : MonoBehaviour
{
    public GameObject restartButton;
    public GameObject restartDialog;

    public void HideDialog()
    {
        restartDialog.SetActive(false);
        restartButton.SetActive(true);
    }

    public void ShowDialog()
    {
        restartDialog.SetActive(true);
        restartButton.SetActive(false);
    }

    public void Restart()
    {
        GameController.Instance.CompleteRestart();
    }
}
