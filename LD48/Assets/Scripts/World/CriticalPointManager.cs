using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalPointManager : GenericSingleton<CriticalPointManager>
{
    public GameObject criticalPoint;
    public int secondsToRemoveCriticalPoint;

    private Collider2D tileWallColiderOfCurrentCriticalPoint;
    private float criticalPointDiameter;
    private const float colliderToCriticalPointCenterDistance = 0.05f;

    public override void Awake()
    {
        base.Awake();
        criticalPointDiameter = criticalPoint.transform.localScale.x / 2;
    }

    public bool IsCriticalPointHit(Collider2D collider, Vector2 point)
    {
        if(collider != tileWallColiderOfCurrentCriticalPoint ||
            criticalPoint == null
            || collider == null)
        {
            return false;
        }

        var renderer = criticalPoint.GetComponentInChildren<Renderer>();
        if(renderer == null)
        {
            Debug.LogWarning("Renderer is null in IsCriticalPointHit");
            return false;
        }

        var width = collider.bounds.size.x;
        var height = collider.bounds.size.y;

        if (width == 1 && Mathf.Abs(criticalPoint.transform.position.x 
            - point.x)
            < CalculateColiderBorderOfCriticalPoint(criticalPointDiameter, colliderToCriticalPointCenterDistance))
        {
            return true;
        }

        if (height == 1 && Mathf.Abs(criticalPoint.transform.position.y 
            - point.y)
            < CalculateColiderBorderOfCriticalPoint(criticalPointDiameter, colliderToCriticalPointCenterDistance))
        {
            return true;
        }

        return false;
    }

    public void SetCriticalPoint(Collider2D collider)
    {
        CancelInvoke();
        var width = collider.bounds.size.x;
        var height = collider.bounds.size.y;
        if(width == 1)
        {
            criticalPoint.transform.position = new Vector3(collider.transform.position.x + Random.Range(-0.4f, 0.4f), 
                collider.transform.position.y, 0f);
        }
        else if(height == 1)
        {
            criticalPoint.transform.position = new Vector3(collider.transform.position.x,
                collider.transform.position.y + Random.Range(-0.4f, 0.4f), 0f);
        }
        else
        {
            Debug.LogWarning("Unexpected size of tile collider");
            return;
        }
        tileWallColiderOfCurrentCriticalPoint = collider;
        criticalPoint.SetActive(true);
        Invoke(nameof(RemoveCriticalPoint), secondsToRemoveCriticalPoint);
    }

    public void TryRemoveCriticalPointFromCollider(Collider2D collider)
    {
        if(collider == tileWallColiderOfCurrentCriticalPoint)
        {
            RemoveCriticalPoint();
        }
    }

    private void RemoveCriticalPoint()
    {
        criticalPoint.SetActive(false);
        tileWallColiderOfCurrentCriticalPoint = null;
    }

    private float CalculateColiderBorderOfCriticalPoint(float criticalPointRadius,
        float colliderToCriticalPointCenterDistance)
    {
        return Mathf.Sqrt(Mathf.Pow(criticalPointRadius, 2) 
            - Mathf.Pow(colliderToCriticalPointCenterDistance, 2));
    }
}
