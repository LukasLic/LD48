﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggingController : MonoBehaviour
{
    public int digs;
    public int criticalPointDigs;

    public float rayCastLenght;
    public LayerMask hitLayers;

    public PlayerAnimationController animationController;

    private bool isMining = false;

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMining)
        {
            animationController.SetMining(true);
            isMining = true;

            //raycast to mouse direction
            var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z));
            mousePosition.z = 0;
            var vectorToNormalize = mousePosition - transform.position;
            vectorToNormalize.Normalize();
            vectorToNormalize *= rayCastLenght;
            var hit = Physics2D.Raycast(transform.position, vectorToNormalize, rayCastLenght, hitLayers.value);

            if (hit)
            {
                Debug.DrawLine(transform.position, hit.point, Color.red, 100f);
                var miningController = hit.collider.gameObject.GetComponentInParent<MiningControllerBase>();
                if (miningController == null)
                {
                    Debug.LogWarning($"MiningController is null on {hit.collider.gameObject.name}");
                    return;
                }

                bool isCriticalPointHit = CriticalPointManager.Instance.IsCriticalPointHit(hit.collider, hit.point);
                CriticalPointManager.Instance.SetCriticalPoint(hit.collider);
                miningController.Mine(hit.collider, isCriticalPointHit ? criticalPointDigs : digs);
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position + vectorToNormalize, Color.green, 100f);
            }
        }
    }

    // Set from animation
    public void Animation_StopMining()
    {
        isMining = false;
    }
}
