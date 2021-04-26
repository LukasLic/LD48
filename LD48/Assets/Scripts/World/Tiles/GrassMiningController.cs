using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassMiningController : MiningControllerBase
{
    private TilesManager tilesManager;
    private (int x, int y) coordinates;

    [Header("Colliders")]
    public Collider2D bottomCollider;

    public override bool IsDiggedOut { get; set; } = false;

    public override void Init(TilesManager tilesManager, (int x, int y) coordinates)
    {
        this.tilesManager = tilesManager;
        this.coordinates = coordinates;

        transform.position = new Vector3(coordinates.x, coordinates.y, 0f);
    }

    public override void Mine(Collider2D collider, int numberOfDigs)
    {
    }

    public override void UpdateState()
    {
        if (bottomCollider == null)
        {
            return;
        }

        CriticalPointManager.Instance.TryRemoveCriticalPointFromCollider();

        var downNeighbor = tilesManager.GetTile(coordinates.x, coordinates.y - 1);
        var downDiggedOut = downNeighbor != null ? downNeighbor.IsDiggedOut : false;

        bottomCollider.enabled = downDiggedOut;
    }

    public override void DigInto(int numberOfDigs)
    {

    }

}
