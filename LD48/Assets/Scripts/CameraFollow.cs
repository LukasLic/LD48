using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public KeyCode zoomOutKey = KeyCode.F;

    public float smoothSpeed = 0.125f;
    private Vector3 offset;

    public float zoomedOutFov = 120f;
    public float zoomOutSpeed = 60f;
    private float startingFOV;

    private void Awake()
    {
        startingFOV = Camera.main.fieldOfView;
        offset = transform.position - target.transform.position;
    }

    private void FixedUpdate()
    {
        var targetPosition = target.position + offset;
        var smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;        
    }

    private void Update()
    {
        if (Input.GetKey(zoomOutKey))
        {
            Camera.main.fieldOfView = Mathf.Min(zoomedOutFov, Camera.main.fieldOfView + zoomOutSpeed * Time.deltaTime);
        }
        else if (Camera.main.fieldOfView > startingFOV)
        {
            Camera.main.fieldOfView = Mathf.Max(startingFOV, Camera.main.fieldOfView - zoomOutSpeed * Time.deltaTime);
        }
    }
}
