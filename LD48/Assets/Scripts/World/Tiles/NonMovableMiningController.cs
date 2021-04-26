using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonMovableMiningController : MiningControllerBase
{
    private TilesManager tilesManager;
    private (int x, int y) coordinates;

    [Header("Colliders")]
    public Collider2D leftCollider;
    public Collider2D rightCollider;
    public Collider2D topCollider;
    public Collider2D bottomCollider;

    [Header("digged variants")]
    public GameObject left_tile;
    public GameObject right_tile;
    public GameObject top_tile;
    public GameObject bottom_tile;

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
        CriticalPointManager.Instance.TryRemoveCriticalPointFromCollider();

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

    public override void DigInto(int numberOfDigs)
    {

    }

}
