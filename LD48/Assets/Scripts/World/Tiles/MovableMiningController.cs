using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableMiningController : MiningControllerBase
{
    public ItemPopper itemPopper;

    private TilesManager tilesManager;
    private (int x, int y) coordinates;
    public int numberOfDigsToMine;

    [Header("Colliders")]
    public Collider2D leftCollider;
    public Collider2D rightCollider;
    public Collider2D topCollider;
    public Collider2D bottomCollider;

    [Header("digged variants")]
    public GameObject background_only_tile;
    public GameObject none_tile;
    public GameObject left_tile;
    public GameObject right_tile;
    public GameObject top_tile;
    public GameObject bottom_tile;

    public override bool IsDiggedOut { get; set; }

    public override void Init(TilesManager tilesManager, (int x, int y) coordinates)
    {
        this.tilesManager = tilesManager;
        this.coordinates = coordinates;

        transform.position = new Vector3(coordinates.x, coordinates.y, 0f);

        itemPopper.Init(numberOfDigsToMine);
    }

    public override void Mine(Collider2D collider, int numberOfDigs)
    {
        var direction = GetMiningDirection(collider);

        if(direction == null)
        {
            Debug.LogWarning("Direction is null in method Mine");
            return;
        }

        // Pop items
        if (numberOfDigsToMine <= numberOfDigs)
        {
            itemPopper.PopAllRemaining(collider.transform.position, direction.Value);
        }
        else
        {
            itemPopper.Pop(numberOfDigs, collider.transform.position, direction.Value);
        }
        

        DigInto(numberOfDigs);
    }

    public override void UpdateState()
    {
        TryRemoveCriticalPoint();

        if (IsDiggedOut)
        {
            leftCollider.enabled = false;
            rightCollider.enabled = false;
            topCollider.enabled = false;
            bottomCollider.enabled = false;

            background_only_tile.SetActive(true);

            left_tile.SetActive(false);
            right_tile.SetActive(false);
            top_tile.SetActive(false);
            bottom_tile.SetActive(false);
            none_tile.SetActive(false);

            return;
        }
        else
        {
            none_tile.SetActive(true);
            background_only_tile.SetActive(false);
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

        left_tile.SetActive(leftDiggedOut);
        right_tile.SetActive(rightDiggedOut);
        top_tile.SetActive(topDiggedOut);
        bottom_tile.SetActive(downDiggedOut);
    }

    private void TryRemoveCriticalPoint()
    {
        CriticalPointManager.Instance.TryRemoveCriticalPointFromCollider();
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
            tilesManager.UpdateTileAndNeighbors(coordinates.x, coordinates.y);
        }
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
}
