using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningController : MonoBehaviour
{
    private TilesManager tilesManager;
    private (int x, int y) coordinates;
    public int numberOfDigsToMine;
    public bool isDiggedOut;

    public BoxCollider2D leftCollider;
    public BoxCollider2D rightCollider;
    public BoxCollider2D topCollider;
    public BoxCollider2D bottomCollider;

    [Header("digged variants")]
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

    public void Init(TilesManager tilesManager, (int x, int y) coordinates)
    {
        this.tilesManager = tilesManager;
        this.coordinates = coordinates;

        transform.position = new Vector3(coordinates.x, coordinates.y, 0f);
    }

    public void Mine(Collider2D collider)
    {
        var tileMiningController = GetTileMiningControllerNextToThisTile(collider);
        if(tileMiningController == null)
        {
            Debug.LogWarning("MininingComponent is null in method Mine");
            return;
        }
        tileMiningController.DigInto();
    }

    public void UpdateState()
    {
        var leftNeighbor = tilesManager.GetTile(coordinates.x - 1, coordinates.y);
        var rightNeighbor = tilesManager.GetTile(coordinates.x + 1, coordinates.y);
        var topNeighbor = tilesManager.GetTile(coordinates.x, coordinates.y + 1);
        var downNeighbor = tilesManager.GetTile(coordinates.x, coordinates.y - 1);
        var leftDiggetOut = leftNeighbor != null ? leftNeighbor.isDiggedOut : false;
        var rightDiggedOut = rightNeighbor != null ? rightNeighbor.isDiggedOut : false;
        var topDiggedOut = topNeighbor != null ? topNeighbor.isDiggedOut : false;
        var downDiggedOut = downNeighbor != null ? downNeighbor.isDiggedOut : false;

        leftCollider.enabled = !leftDiggetOut;
        rightCollider.enabled = !rightDiggedOut;
        topCollider.enabled = !topDiggedOut;
        bottomCollider.enabled = !downDiggedOut;

        SetAllTileTypesInactive();
        if (leftDiggetOut && rightDiggedOut && topDiggedOut && downDiggedOut)
        {
            none_tile.SetActive(true);
        }
        else if (!leftDiggetOut && rightDiggedOut && topDiggedOut && downDiggedOut)
        {
            left_tile.SetActive(true);
        }
        else if (leftDiggetOut && !rightDiggedOut && topDiggedOut && downDiggedOut)
        {
            right_tile.SetActive(true);
        }
        else if (!leftDiggetOut && !rightDiggedOut && topDiggedOut && downDiggedOut)
        {
            left_right_tile.SetActive(true);
        }
        else if (leftDiggetOut && rightDiggedOut && !topDiggedOut && downDiggedOut)
        {
            top_tile.SetActive(true);
        }
        else if (!leftDiggetOut && !rightDiggedOut && !topDiggedOut && downDiggedOut)
        {
            left_top_right_tile.SetActive(true);
        }
        else if (!leftDiggetOut && rightDiggedOut && !topDiggedOut && downDiggedOut)
        {
            left_top_tile.SetActive(true);
        }
        else if (leftDiggetOut && !rightDiggedOut && !topDiggedOut && downDiggedOut)
        {
            right_top_tile.SetActive(true);
        }
        else if (leftDiggetOut && rightDiggedOut && topDiggedOut && !downDiggedOut)
        {
            down_tile.SetActive(true);
        }
        else if (!leftDiggetOut && rightDiggedOut && topDiggedOut && !downDiggedOut)
        {
            left_down_tile.SetActive(true);
        }
        else if (leftDiggetOut && !rightDiggedOut && topDiggedOut && !downDiggedOut)
        {
            right_down_tile.SetActive(true);
        }
        else if (!leftDiggetOut && !rightDiggedOut && topDiggedOut && !downDiggedOut)
        {
            right_left_down_tile.SetActive(true);
        }
        else if (leftDiggetOut && rightDiggedOut && !topDiggedOut && !downDiggedOut)
        {
            top_down_tile.SetActive(true);
        }
        else if (!leftDiggetOut && rightDiggedOut && !topDiggedOut && !downDiggedOut)
        {
            top_left_down_tile.SetActive(true);
        }
        else if (leftDiggetOut && !rightDiggedOut && !topDiggedOut && !downDiggedOut)
        {
            top_right_down_tile.SetActive(true);
        }
        else
        {
            top_right_left_down_tile.SetActive(true);
        }
    }

    private void SetAllTileTypesInactive()
    {
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
    /// called on neighbour
    /// </summary>
    private void DigInto()
    {
        --numberOfDigsToMine;
        if(numberOfDigsToMine <= 0)
        {
            if (tilesManager == null)
            {
                Debug.LogWarning("TilesManager is null in method Mine");
                return;
            }

            gameObject.SetActive(true);
            isDiggedOut = true;
            tilesManager.UpdateTileWithNeighbors(coordinates.x, coordinates.y);
        }
    }

    private MiningController GetTileMiningControllerNextToThisTile(Collider2D collider)
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
}
