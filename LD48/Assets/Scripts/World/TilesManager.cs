using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject rootObject;

    private readonly Dictionary<(int x, int y), MiningControllerBase> tiles = new Dictionary<(int x, int y), MiningControllerBase>();

    public void Start()
    {
        var tileComponents = rootObject.GetComponentsInChildren<MiningControllerBase>();
        foreach (var tileComponent in tileComponents)
        {
            var roundedX = Convert.ToInt32(Math.Round(tileComponent.gameObject.transform.position.x, 0));
            int roundedY = Convert.ToInt32(Math.Round(tileComponent.gameObject.transform.position.y, 0));

            tiles.Add((roundedX, roundedY), tileComponent);
            tileComponent.Init(this, (roundedX, roundedY));
            tileComponent.IsDiggedOut = true;
        }

        foreach(var tileComponent in tileComponents)
        {
            tileComponent.UpdateState();
        }
    }

    public MiningControllerBase GetTile(int x, int y)
    {
        if (tiles.TryGetValue((x, y), out var tile))
        {
            return tile;
        }
        return null;
    }

    public MiningControllerBase GetOrAddNewTile(int x, int y)
    {
        if (tiles.TryGetValue((x, y), out var tile))
        {
            return tile;
        }
        return AddNewTile(x, y);
    }

    public void UpdateTileAndNeighbors(int x, int y)
    {
        var tile = GetTile(x, y);
        var leftNeighbor = GetOrAddNewTile(x - 1, y);
        var rightNeighbor = GetOrAddNewTile(x + 1, y);
        var topNeighbor = GetOrAddNewTile(x, y + 1);
        var downNeighbor = GetOrAddNewTile(x, y - 1);

        if (leftNeighbor != null && !leftNeighbor.IsDiggedOut)
        {
            leftNeighbor.UpdateState();
        }
        if(rightNeighbor != null && !rightNeighbor.IsDiggedOut)
        {
            rightNeighbor.UpdateState();
        }
        if (topNeighbor != null && !topNeighbor.IsDiggedOut)
        {
            topNeighbor.UpdateState();
        }
        if (downNeighbor != null && !downNeighbor.IsDiggedOut)
        {
            downNeighbor.UpdateState();
        }

        tile.UpdateState();
    }

    private MiningControllerBase AddNewTile(int originalX, int originalY)
    {
        var tileGameObject = Instantiate(tilePrefab, new Vector3(originalX, originalY, 0), 
            Quaternion.identity, rootObject.transform);
        tileGameObject.SetActive(false);
        try
        {
            var miningController = tileGameObject.GetComponent<MiningControllerBase>();
            tiles.Add((originalX, originalY), miningController);
            miningController.Init(this, (originalX, originalY));
            return miningController;
        }
        catch (ArgumentException)
        {
            Debug.LogWarning($"Trying to add already added tile object to TilesManager with coordinates {originalX} {originalY}");
            return null;
        }
    }
}
