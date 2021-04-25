using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonMovableMiningController : MiningControllerBase
{
    public override bool IsDiggedOut { get; set; } = true;

    public override void Init(TilesManager tilesManager, (int x, int y) coordinates)
    {
    }

    public override void Mine(Collider2D collider, int numberOfDigs)
    {
    }

    public override void UpdateState()
    {
    }

    public override void DigInto(int numberOfDigs)
    {

    }

}
