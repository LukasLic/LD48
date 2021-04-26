using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableMiningController : MiningControllerBase
{
    private TilesManager tilesManager;
    private (int x, int y) coordinates;
    public int numberOfDigsToMine;

    public Collider2D leftCollider;
    public Collider2D rightCollider;
    public Collider2D topCollider;
    public Collider2D bottomCollider;

    [Header("digged variants")]
    public GameObject background_only_tile;
    public GameObject none_tile;
    public GameObject left_tile;
    public GameObject right_tile;
    public GameObject left_right_tile;
    public GameObject top_tile;
    public GameObject left_top_right_tile;
    public GameObject left_top_tile;
    public GameObject right_top_tile;
    public GameObject down_tile;
    public GameObject left_down_tile;
    public GameObject right_down_tile;
    public GameObject right_left_down_tile;
    public GameObject top_down_tile;
    public GameObject top_left_down_tile;
    public GameObject top_right_down_tile;
    public GameObject top_right_left_down_tile;

    [Header("Corners")]
    public GameObject corner_top_left;
    public GameObject corner_top_right;
    public GameObject corner_bottom_left;
    public GameObject corner_bottom_right;

    public GameObject hiding_corner_top_left;
    public GameObject hiding_corner_top_right;
    public GameObject hiding_corner_bottom_left;
    public GameObject hiding_corner_bottom_right;

    public override bool IsDiggedOut { get; set; }
    public override bool IsVisible { get; set; }

    public override void Init(TilesManager tilesManager, (int x, int y) coordinates)
    {
        this.tilesManager = tilesManager;
        this.coordinates = coordinates;

        transform.position = new Vector3(coordinates.x, coordinates.y, 0f);
    }

    public override void Mine(Collider2D collider, int numberOfDigs)
    {
        //var tileMiningController = this as MiningControllerBase; //GetTileMiningControllerNextToThisTile(collider);
        //if (tileMiningController == null)
        //{
        //    Debug.LogWarning("MininingComponent is null in method Mine");
        //    return;
        //}
        var direction = GetMiningDirection(collider);

        if(direction == null)
        {
            Debug.LogWarning("Direction is null in method Mine");
            return;
        }

        DigInto(numberOfDigs);
    }

    public override void UpdateState()
    {
        SetAllTileTypesInactive();
        TryRemoveCriticalPoint();

        if (IsDiggedOut)
        {
            background_only_tile.SetActive(true);
            IsVisible = true;

            leftCollider.enabled = false;
            rightCollider.enabled = false;
            topCollider.enabled = false;
            bottomCollider.enabled = false;

            HideAllCorners();

            return;
        }

        var leftNeighbor = tilesManager.GetTile(coordinates.x - 1, coordinates.y);
        var rightNeighbor = tilesManager.GetTile(coordinates.x + 1, coordinates.y);
        var topNeighbor = tilesManager.GetTile(coordinates.x, coordinates.y + 1);
        var downNeighbor = tilesManager.GetTile(coordinates.x, coordinates.y - 1);

        var leftDiggedOut = leftNeighbor != null ? leftNeighbor.IsDiggedOut : false;
        var rightDiggedOut = rightNeighbor != null ? rightNeighbor.IsDiggedOut : false;
        var topDiggedOut = topNeighbor != null ? topNeighbor.IsDiggedOut : false;
        var downDiggedOut = downNeighbor != null ? downNeighbor.IsDiggedOut : false;

        leftCollider.enabled = leftDiggedOut;
        rightCollider.enabled = rightDiggedOut;
        topCollider.enabled = topDiggedOut;
        bottomCollider.enabled = downDiggedOut;

        //Debug.Log($"{gameObject.name}   X:{coordinates.x}, Y:{coordinates.y}");
        //Debug.Log($"leftDiggetOut:{leftDiggedOut}");
        //Debug.Log($"rightDiggedOut:{rightDiggedOut}");
        //Debug.Log($"topDiggedOut:{topDiggedOut}");
        //Debug.Log($"downDiggedOut:{downDiggedOut}");
        //Debug.Log("-----------------");

        //TryRemoveCriticalPoint(leftDiggetOut, leftCollider);
        //TryRemoveCriticalPoint(rightDiggedOut, rightCollider);
        //TryRemoveCriticalPoint(topDiggedOut, topCollider);
        //TryRemoveCriticalPoint(downDiggedOut, bottomCollider);

        IsVisible = true;

        if (leftDiggedOut && rightDiggedOut && topDiggedOut && downDiggedOut)
        {
            top_right_left_down_tile.SetActive(true);

            // No corners
            HideAllCorners();
        }
        else if (!leftDiggedOut && rightDiggedOut && topDiggedOut && downDiggedOut)
        {
            top_right_down_tile.SetActive(true);

            // No corners - U shape
            HideAllCorners();
        }
        else if (leftDiggedOut && !rightDiggedOut && topDiggedOut && downDiggedOut)
        {
            top_left_down_tile.SetActive(true);

            // No corners - U shape
            HideAllCorners();
        }
        else if (!leftDiggedOut && !rightDiggedOut && topDiggedOut && downDiggedOut)
        {
            top_down_tile.SetActive(true);

            // No corners - = shape
            HideAllCorners();
        }
        else if (leftDiggedOut && rightDiggedOut && !topDiggedOut && downDiggedOut)
        {
            right_left_down_tile.SetActive(true);

            // No corners - U shape
            HideAllCorners();
        }
        else if (!leftDiggedOut && !rightDiggedOut && !topDiggedOut && downDiggedOut)
        {
            down_tile.SetActive(true);

            SetCornerState((-1, -1), false);
            SetCornerState((1, -1), false);
        }
        else if (!leftDiggedOut && rightDiggedOut && !topDiggedOut && downDiggedOut)
        {
            right_down_tile.SetActive(true);

            //corner_top_left.SetActive(false);
            SetCornerState((-1, 1), false);
        }
        else if (leftDiggedOut && !rightDiggedOut && !topDiggedOut && downDiggedOut)
        {
            left_down_tile.SetActive(true);

            //corner_top_right.SetActive(false);
            SetCornerState((1, 1), false);
        }
        else if (leftDiggedOut && rightDiggedOut && topDiggedOut && !downDiggedOut)
        {
            left_top_right_tile.SetActive(true);

            // No corners - U shape
            HideAllCorners();
        }
        else if (!leftDiggedOut && rightDiggedOut && topDiggedOut && !downDiggedOut)
        {
            right_top_tile.SetActive(true);

            //corner_bottom_left.SetActive(false);
            SetCornerState((-1, -1), false);
        }
        else if (leftDiggedOut && !rightDiggedOut && topDiggedOut && !downDiggedOut)
        {
            left_top_tile.SetActive(true);

            //corner_bottom_right.SetActive(false);
            SetCornerState((1, -1), false);
        }
        else if (!leftDiggedOut && !rightDiggedOut && topDiggedOut && !downDiggedOut)
        {
            top_tile.SetActive(true);

            //corner_bottom_left.SetActive(false);
            //corner_bottom_right.SetActive(false);
            SetCornerState((-1, -1), false);
            SetCornerState((1, -1), false);
        }
        else if (leftDiggedOut && rightDiggedOut && !topDiggedOut && !downDiggedOut)
        {
            left_right_tile.SetActive(true);

            // No corners - = shape
            HideAllCorners();
        }
        else if (!leftDiggedOut && rightDiggedOut && !topDiggedOut && !downDiggedOut)
        {
            right_tile.SetActive(true);

            //corner_bottom_left.SetActive(false);
            //corner_top_left.SetActive(false);
            SetCornerState((-1, -1), false);
            SetCornerState((-1, 1), false);
        }
        else if (leftDiggedOut && !rightDiggedOut && !topDiggedOut && !downDiggedOut)
        {
            left_tile.SetActive(true);

            //corner_bottom_left.SetActive(false);
            //corner_bottom_right.SetActive(false);
            SetCornerState((-1, -1), false);
            SetCornerState((1, -1), false);
        }
        else
        {
            none_tile.SetActive(true);
            IsVisible = false;

            //corner_bottom_left.SetActive(false);
            //corner_bottom_right.SetActive(false);
            //corner_top_left.SetActive(false);
            //corner_top_right.SetActive(false);

            SetCornerState((1, -1), false);
            SetCornerState((1, 1), false);
            SetCornerState((-1, 1), false);
            SetCornerState((-1, -1), false);
        }

        // Update corners
        //var leftDiggedOut = leftNeighbor != null ? leftNeighbor.IsDiggedOut : false;
        //var rightDiggedOut = rightNeighbor != null ? rightNeighbor.IsDiggedOut : false;
        //var topDiggedOut = topNeighbor != null ? topNeighbor.IsDiggedOut : false;
        //var downDiggedOut = downNeighbor != null ? downNeighbor.IsDiggedOut : false;
    }

    //private void TryRemoveCriticalPoint(bool isDiggedOut, Collider2D collider)
    //{
    //    if (isDiggedOut)
    //    {
    //        CriticalPointManager.Instance.TryRemoveCriticalPointFromCollider(collider);
    //    }
    //}

    private void TryRemoveCriticalPoint()
    {
        CriticalPointManager.Instance.TryRemoveCriticalPointFromCollider();
    }

    /// <summary>
    /// Optimalize if problems with performance
    /// </summary>
    private void SetAllTileTypesInactive()
    {
        background_only_tile.SetActive(false);
        none_tile.SetActive(false);
        left_tile.SetActive(false);
        right_tile.SetActive(false);
        left_right_tile.SetActive(false);
        top_tile.SetActive(false);
        left_top_right_tile.SetActive(false);
        left_top_tile.SetActive(false);
        right_top_tile.SetActive(false);
        down_tile.SetActive(false);
        left_down_tile.SetActive(false);
        right_down_tile.SetActive(false);
        right_left_down_tile.SetActive(false);
        top_down_tile.SetActive(false);
        top_left_down_tile.SetActive(false);
        top_right_down_tile.SetActive(false);
        top_right_left_down_tile.SetActive(false);
    }

    /// <summary>
    /// Called on me
    /// </summary>
    public override void DigInto(int numberOfDigs)
    {
        numberOfDigsToMine -= numberOfDigs;
        if(numberOfDigsToMine <= 0)
        {
            if (tilesManager == null)
            {
                Debug.LogWarning("TilesManager is null in method Mine");
                return;
            }

            IsDiggedOut = true;
            
            //gameObject.SetActive(false);


            tilesManager.UpdateTileAndNeighbors(coordinates.x, coordinates.y);
        }
    }

    private MiningControllerBase GetTileMiningControllerNextToThisTile(Collider2D collider)
    {
        if (tilesManager == null)
        {
            Debug.LogWarning("TilesManager is null in method Mine");
            return null;
        }

        if(collider == leftCollider)
        {
            return tilesManager.GetOrAddNewTile(coordinates.x - 1, coordinates.y);
        }

        if (collider == rightCollider)
        {
            return tilesManager.GetOrAddNewTile(coordinates.x + 1, coordinates.y);
        }

        if (collider == topCollider)
        {
            return tilesManager.GetOrAddNewTile(coordinates.x, coordinates.y + 1);
        }

        if (collider == bottomCollider)
        {
            return tilesManager.GetOrAddNewTile(coordinates.x, coordinates.y - 1);
        }
        Debug.LogWarning("No known collider hit in method GetTileNextToThisTile");
        return null;
    }

    private Direction? GetMiningDirection(Collider2D collider)
    {
        if (tilesManager == null)
        {
            Debug.LogWarning("TilesManager is null in method Mine");
            return null;
        }

        if (collider == leftCollider)
        {
            return Direction.Left;
        }

        if (collider == rightCollider)
        {
            return Direction.Right;
        }

        if (collider == topCollider)
        {
            return Direction.Top;
        }

        if (collider == bottomCollider)
        {
            return Direction.Down;
        }
        Debug.LogWarning("No known collider hit in method GetMiningDirection");
        return null;
    }

    private void HideAllCorners()
    {
        corner_bottom_left.SetActive(false);
        corner_bottom_right.SetActive(false);
        corner_top_left.SetActive(false);
        corner_top_right.SetActive(false);
    }

    public override void UpdateCornerState((int x, int y) direction)
    {
        if (IsDiggedOut)
        {
            return;
        }

        if (direction.x == 1 && direction.y == 1)
        {
            var xNeighbor = tilesManager.GetTile(coordinates.x + 1, coordinates.y);
            var yNeighbor = tilesManager.GetTile(coordinates.x, coordinates.y + 1);

            if (xNeighbor != null && yNeighbor != null && xNeighbor.IsVisible && yNeighbor.IsVisible)
            {
                var cornerNeighbour = tilesManager.GetTile(coordinates.x + 1, coordinates.y + 1);
                if (cornerNeighbour != null ? cornerNeighbour.IsDiggedOut : false)
                {
                    //Debug.DrawLine(transform.position, cornerNeighbour.transform.position, Color.yellow, 50f);
                    SetCornerState(direction, true);
                }
            }
            //corner_top_right.SetActive(state);
            //hiding_corner_top_right.SetActive(!state);
        }
        else if (direction.x == 1 && direction.y == -1)
        {
            //corner_bottom_right.SetActive(state);
            //hiding_corner_bottom_right.SetActive(!state);
        }
        else if (direction.x == -1 && direction.y == 1)
        {
            //corner_top_left.SetActive(state);
            //hiding_corner_top_left.SetActive(!state);
        }
        else if (direction.x == -1 && direction.y == -1)
        {
            //corner_bottom_left.SetActive(state);
            //hiding_corner_bottom_left.SetActive(!state);
        }

        

        var leftDiggedOut = leftNeighbor != null ? leftNeighbor.IsDiggedOut : false;
        var rightDiggedOut = rightNeighbor != null ? rightNeighbor.IsDiggedOut : false;
        var topDiggedOut = topNeighbor != null ? topNeighbor.IsDiggedOut : false;
        var downDiggedOut = downNeighbor != null ? downNeighbor.IsDiggedOut : false;

        leftCollider.enabled = leftDiggedOut;
        rightCollider.enabled = rightDiggedOut;
        topCollider.enabled = topDiggedOut;
        bottomCollider.enabled = downDiggedOut;

        //Debug.Log($"{gameObject.name}   X:{coordinates.x}, Y:{coordinates.y}");
        //Debug.Log($"leftDiggetOut:{leftDiggedOut}");
        //Debug.Log($"rightDiggedOut:{rightDiggedOut}");
        //Debug.Log($"topDiggedOut:{topDiggedOut}");
        //Debug.Log($"downDiggedOut:{downDiggedOut}");
        //Debug.Log("-----------------");

        //TryRemoveCriticalPoint(leftDiggetOut, leftCollider);
        //TryRemoveCriticalPoint(rightDiggedOut, rightCollider);
        //TryRemoveCriticalPoint(topDiggedOut, topCollider);
        //TryRemoveCriticalPoint(downDiggedOut, bottomCollider);

        IsVisible = true;

        if (leftDiggedOut && rightDiggedOut && topDiggedOut && downDiggedOut)
        {
            top_right_left_down_tile.SetActive(true);

            // No corners
        }
        else if (!leftDiggedOut && rightDiggedOut && topDiggedOut && downDiggedOut)
        {
            top_right_down_tile.SetActive(true);

            // No corners - U shape
        }
        else if (leftDiggedOut && !rightDiggedOut && topDiggedOut && downDiggedOut)
        {
            top_left_down_tile.SetActive(true);

            // No corners - U shape
        }
        else if (!leftDiggedOut && !rightDiggedOut && topDiggedOut && downDiggedOut)
        {
            top_down_tile.SetActive(true);

            // No corners - = shape
        }
        else if (leftDiggedOut && rightDiggedOut && !topDiggedOut && downDiggedOut)
        {
            right_left_down_tile.SetActive(true);

            // No corners - U shape
        }
        else if (!leftDiggedOut && !rightDiggedOut && !topDiggedOut && downDiggedOut)
        {
            down_tile.SetActive(true);
        }
        else if (!leftDiggedOut && rightDiggedOut && !topDiggedOut && downDiggedOut)
        {
            right_down_tile.SetActive(true);

            // Corners _| shape

            if (coordinates.x == -1 && coordinates.y == 1)
            {
                Debug.Log("-------------------");
            }

            if (topNeighbor != null && leftNeighbor != null && topNeighbor.IsVisible && leftNeighbor.IsVisible)
            {
                if (coordinates.x == -1 && coordinates.y == 1)
                {
                    Debug.Log("HERE1");
                }

                var cornerNeighbour = tilesManager.GetTile(coordinates.x - 1, coordinates.y + 1);
                if (cornerNeighbour != null ? cornerNeighbour.IsDiggedOut : false)
                {
                    if (coordinates.x == -1 && coordinates.y == 1)
                    {
                        Debug.Log("HERE2");
                    }

                    Debug.DrawLine(transform.position, cornerNeighbour.transform.position, Color.yellow, 50f);
                }
            }
        }
        else if (leftDiggedOut && !rightDiggedOut && !topDiggedOut && downDiggedOut)
        {
            left_down_tile.SetActive(true);
        }
        else if (leftDiggedOut && rightDiggedOut && topDiggedOut && !downDiggedOut)
        {
            left_top_right_tile.SetActive(true);

            // No corners - U shape
        }
        else if (!leftDiggedOut && rightDiggedOut && topDiggedOut && !downDiggedOut)
        {
            right_top_tile.SetActive(true);
        }
        else if (leftDiggedOut && !rightDiggedOut && topDiggedOut && !downDiggedOut)
        {
            left_top_tile.SetActive(true);
        }
        else if (!leftDiggedOut && !rightDiggedOut && topDiggedOut && !downDiggedOut)
        {
            top_tile.SetActive(true);
        }
        else if (leftDiggedOut && rightDiggedOut && !topDiggedOut && !downDiggedOut)
        {
            left_right_tile.SetActive(true);

            // No corners - = shape
        }
        else if (!leftDiggedOut && rightDiggedOut && !topDiggedOut && !downDiggedOut)
        {
            right_tile.SetActive(true);
        }
        else if (leftDiggedOut && !rightDiggedOut && !topDiggedOut && !downDiggedOut)
        {
            left_tile.SetActive(true);
        }
        else
        {
            none_tile.SetActive(true);
            IsVisible = false;
        }
    }

    private void SetCornerState((int x, int y) direction, bool state)
    {
        if (direction.x == 1 && direction.y == 1)
        {
            corner_top_right.SetActive(state);
            hiding_corner_top_right.SetActive(!state);
        }
        else if(direction.x == 1 && direction.y == -1)
        {
            corner_bottom_right.SetActive(state);
            hiding_corner_bottom_right.SetActive(!state);
        }
        else if (direction.x == -1 && direction.y == 1)
        {
            corner_top_left.SetActive(state);
            hiding_corner_top_left.SetActive(!state);
        }
        else if (direction.x == -1 && direction.y == -1)
        {
            corner_bottom_left.SetActive(state);
            hiding_corner_bottom_left.SetActive(!state);
        }
    }
}
