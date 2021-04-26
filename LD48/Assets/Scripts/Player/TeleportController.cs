using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public Vector3 startPosition;
    public int numberOfAvailableTeleports;

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
            var cameraPosition = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(startPosition.x, startPosition.y, cameraPosition.z);
            --numberOfAvailableTeleports;
        }
    }
}