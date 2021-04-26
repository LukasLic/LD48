using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject rootObject;

    private Dictionary<(int x, int y), MiningControllerBase> tiles = new Dictionary<(int x, int y), MiningControllerBase>();

    public void Start()
    {
        var tileComponents = rootObject.GetComponentsInChildren<MiningControllerBase>();

        // Scan for tiles
        foreach (var tileComponent in tileComponents)
        {
            var roundedX = Convert.ToInt32(Math.Round(tileComponent.gameObject.transform.position.x, 0));
            int roundedY = Convert.ToInt32(Math.Round(tileComponent.gameObject.transform.position.y, 0));

            tiles.Add((roundedX, roundedY), tileComponent);
        }

        // Surround empty tiles
        var emptyTileComponents = rootObject.GetComponentsInChildren<EmptyMiningController>();
        foreach (var tileComponent in emptyTileComponents)
        {
            var roundedX = Convert.ToInt32(Math.Round(tileComponent.gameObject.transform.position.x, 0));
            int roundedY = Convert.ToInt32(Math.Round(tileComponent.gameObject.transform.position.y, 0));

            tileComponent.Surround((roundedX, roundedY), this);
        }

        // Rescan
        tiles = new Dictionary<(int x, int y), MiningControllerBase>(); // Recreate dictionary
        tileComponents = rootObject.GetComponentsInChildren<MiningControllerBase>();

        // Prepare grid with empty tiles
        foreach (var tileComponent in tileComponents)
        {
            // TODO: Optimize
            var roundedX = Convert.ToInt32(Math.Round(tileComponent.gameObject.transform.position.x, 0));
            int roundedY = Convert.ToInt32(Math.Round(tileComponent.gameObject.transform.position.y, 0));

            tiles.Add((roundedX, roundedY), tileComponent);
            tileComponent.Init(this, (roundedX, roundedY));
            tileComponent.IsDiggedOut = false;
        }

        foreach (var tileComponent in tileComponents)
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
        //// All combinations except 0,0
        //for (int i = -1; i <= 1; i++)
        //{
        //    for (int j = -1; j <= 1; j++)
        //    {
        //        if (i == 0 && j == 0) { continue; }

        //        var neighbor = GetOrAddNewTile(x + i, y + j);

        //        if(neighbor != null && !neighbor.IsDiggedOut)
        //        {
        //            neighbor.UpdateState();
        //        }
        //    }
        //}

        GetOrAddNewTile(x - 1, y).UpdateState();
        GetOrAddNewTile(x + 1, y).UpdateState();
        GetOrAddNewTile(x, 1).UpdateState();
        GetOrAddNewTile(x, 1).UpdateState();

        GetTile(x, y).UpdateState();

        //var leftNeighbor = GetOrAddNewTile(x - 1, y);
        //var rightNeighbor = GetOrAddNewTile(x + 1, y);
        //var topNeighbor = GetOrAddNewTile(x, y + 1);
        //var downNeighbor = GetOrAddNewTile(x, y - 1);

        //if (leftNeighbor != null && !leftNeighbor.IsDiggedOut)
        //{
        //    leftNeighbor.UpdateState();
        //}
        //if(rightNeighbor != null && !rightNeighbor.IsDiggedOut)
        //{
        //    rightNeighbor.UpdateState();
        //}
        //if (topNeighbor != null && !topNeighbor.IsDiggedOut)
        //{
        //    topNeighbor.UpdateState();
        //}
        //if (downNeighbor != null && !downNeighbor.IsDiggedOut)
        //{
        //    downNeighbor.UpdateState();
        //}
    }

    private MiningControllerBase AddNewTile(int originX, int originY)
    {
        Debug.Log($"Adding tile at [{originX},{originY}]");

        var tileGameObject = Instantiate(tilePrefab, new Vector3(originX, originY, 0), 
            Quaternion.identity, rootObject.transform);
        tileGameObject.SetActive(true);

        try
        {
            var miningController = tileGameObject.GetComponent<MiningControllerBase>();
            tiles.Add((originX, originY), miningController);
            miningController.Init(this, (originX, originY));
            return miningController;
        }
        catch (ArgumentException)
        {
            Debug.LogWarning($"Trying to add already added tile object to TilesManager with coordinates {originX} {originY}");
            return null;
        }
    }

    /// <summary>
    /// Forces creation of tile on x,y. Tile is not initialized.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void ForceTile(int x, int y)
    {
        if (tiles.ContainsKey((x, y)))
        {
            return;
        }
        else
        {
            var tileGameObject = Instantiate(tilePrefab, new Vector3(x, y, 0),
            Quaternion.identity, rootObject.transform);
            tileGameObject.SetActive(true);

            var miningController = tileGameObject.GetComponent<MiningControllerBase>();
            tiles.Add((x, y), miningController);
        }
    }
}
