using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyMiningController : MiningControllerBase
{
    private TilesManager tilesManager;
    private (int x, int y) coordinates;

    public override bool IsDiggedOut { get => true; set { } }

    public override void Init(TilesManager tilesManager, (int x, int y) coordinates)
    {
        this.tilesManager = tilesManager;
        this.coordinates = coordinates;

        transform.position = new Vector3(coordinates.x, coordinates.y, 0f);
    }

    public override void Mine(Collider2D collider, int numberOfDigs)
    {
        Debug.LogWarning("Should not call Mine at EmptyMiningController!");
    }

    public override void UpdateState()
    {
        return;
    }

    /// <summary>
    /// Called on me
    /// </summary>
    public override void DigInto(int numberOfDigs)
    {
        Debug.LogWarning("Should not call DigInto at EmptyMiningController!");
    }

    public void Surround((int x, int y) coordinates, TilesManager tilesManager)
    {
        // All combinations except 0,0
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if(i == 0 && j == 0) { continue; }

                tilesManager.ForceTile(coordinates.x + i, coordinates.y + j);
            }
        }
    }
}
