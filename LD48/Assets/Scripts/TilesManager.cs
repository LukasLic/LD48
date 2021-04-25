using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject rootObject;

    private readonly Dictionary<(int x, int y), MiningController> tiles = new Dictionary<(int x, int y), MiningController>();

    public void Start()
    {
        var tileComponents = rootObject.GetComponentsInChildren<MiningController>();
        foreach (var tileComponent in tileComponents)
        {
            var roundedX = Convert.ToInt32(Math.Round(tileComponent.gameObject.transform.position.x, 0));
            int roundedY = Convert.ToInt32(Math.Round(tileComponent.gameObject.transform.position.y, 0));

            tiles.Add((roundedX, roundedY), tileComponent);
            tileComponent.Init(this, (roundedX, roundedY));
            tileComponent.isDiggedOut = true;
            //UpdateTileWithNeighbors(roundedX, roundedY);
        }

        foreach(var tileComponent in tileComponents)
        {
            tileComponent.UpdateState();
        }
    }

    public MiningController GetTile(int x, int y)
    {
        if (tiles.TryGetValue((x, y), out var tile))
        {
            return tile;
        }
        return null;
    }

    public MiningController GetOrAddNewTile(int x, int y)
    {
        if (tiles.TryGetValue((x, y), out var tile))
        {
            return tile;
        }
        return AddNewTile(x, y);
    }

    public void UpdateTileWithNeighbors(int x, int y)
    {
        var tile = GetTile(x, y);
        var leftNeighbor = GetTile(x - 1, y);
        var rightNeighbor = GetTile(x + 1, y);
        var topNeighbor = GetTile(x, y + 1);
        var downNeighbor = GetTile(x, y - 1);
        bool leftUpdate = false;
        bool rightUpdate = false;
        bool topUpdate = false;
        bool downUpdate = false;
        if (leftNeighbor != null)
        {
            leftNeighbor.UpdateState();

            leftUpdate = leftNeighbor.isDiggedOut;
        }
        if(rightNeighbor != null)
        {
            rightNeighbor.UpdateState();

            rightUpdate = rightNeighbor.isDiggedOut;
        }
        if (topNeighbor != null)
        {
            topNeighbor.UpdateState();

            topUpdate = topNeighbor.isDiggedOut;
        }
        if (downNeighbor != null)
        {
            downNeighbor.UpdateState();

            downUpdate = downNeighbor.isDiggedOut;
        }
        tile.UpdateState();
    }

    private MiningController AddNewTile(int originalX, int originalY)
    {
        var tileGameObject = Instantiate(tilePrefab, new Vector3(originalX, originalY, 0), 
            Quaternion.identity, rootObject.transform);
        tileGameObject.SetActive(false);
        try
        {
            var miningController = tileGameObject.GetComponent<MiningController>();
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
