using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    private Vector3 startPosition;
    public int numberOfAvailableTeleports;

    public ShopController shopController;

    private void Awake()
    {
        startPosition = transform.position;
        shopController.UpdateTeleportCount(numberOfAvailableTeleports);
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TeleportToStartPosition();
        }
    }

    public void TeleportToStartPosition()
    {
        if(numberOfAvailableTeleports > 0)
        {
            transform.position = startPosition;
            var cameraPosition = Camera.main.transform.parent.position;
            Camera.main.transform.parent.position = new Vector3(startPosition.x, startPosition.y, cameraPosition.z);
            --numberOfAvailableTeleports;

            shopController.UpdateTeleportCount(numberOfAvailableTeleports);
        }
    }
}