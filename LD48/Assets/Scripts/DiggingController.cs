using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggingController : MonoBehaviour
{
    public float rayCastLenght;
    // Update is called once per frame
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //raycast to mouse direction
            var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z));
            mousePosition.z = 0;
            var vectorToNormalize = mousePosition - transform.position;
            vectorToNormalize.Normalize();
            vectorToNormalize *= rayCastLenght;
            var hit = Physics2D.Raycast(transform.position, vectorToNormalize, rayCastLenght);
            if (hit)
            {
                Debug.DrawLine(transform.position, hit.point, Color.red, 100);
                var miningController = hit.collider.gameObject.GetComponentInParent<MiningController>();
                if (miningController == null)
                {
                    Debug.LogWarning("MiningController is null");
                    return;
                }
                miningController.Mine(hit.collider);
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position + vectorToNormalize, Color.green, 100);
            }
        }
    }
}
